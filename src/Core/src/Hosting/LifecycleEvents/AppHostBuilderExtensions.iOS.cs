using System;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;
using UIKit;
using System.Collections.Concurrent;
using Foundation;

namespace Microsoft.Maui.LifecycleEvents
{
	public static partial class AppHostBuilderExtensions
	{
		static NSObject? _windowDidBecomeKeyObserver;
		static NSObject? _windowDidResignKeyObserver;
		static readonly ConcurrentDictionary<nint, bool> _windowActivationStates = new();
		internal static MauiAppBuilder ConfigureCrossPlatformLifecycleEvents(this MauiAppBuilder builder) =>
			builder.ConfigureLifecycleEvents(events => events.AddiOS(OnConfigureLifeCycle));

		internal static MauiAppBuilder ConfigureWindowEvents(this MauiAppBuilder builder) =>
			builder.ConfigureLifecycleEvents(events => events.AddiOS(OnConfigureWindow));

		static void OnConfigureLifeCycle(IiOSLifecycleBuilder iOS)
		{
#if MACCATALYST
			_windowDidBecomeKeyObserver = NSNotificationCenter.DefaultCenter.AddObserver(
				new NSString("NSWindowDidBecomeKeyNotification"), _ =>
				{
					if (!UIApplication.SharedApplication.Delegate.HasSceneManifest())
					{
						// For non-scene based apps, get the main window
						//UIApplication.SharedApplication.GetWindow()?.Activated();
						var window = UIApplication.SharedApplication.GetWindow();
						if (window != null)
						{
							var windowKey = (window.Handler?.PlatformView is NSObject nsObj ? (nint)nsObj.Handle : (nint)0);
							if (windowKey != nint.Zero)
							{
								bool isActive = false;
								_windowActivationStates.TryGetValue(windowKey, out isActive);
								if (!isActive)
								{
									_windowActivationStates[windowKey] = true;
									window.Focused();
								}
							}
						}
					}
				});
			_windowDidResignKeyObserver = NSNotificationCenter.DefaultCenter.AddObserver(
				new NSString("NSWindowDidResignKeyNotification"), _ =>
				{
					if (!UIApplication.SharedApplication.Delegate.HasSceneManifest())
					{
						// For non-scene based apps, get the main window
						var window = UIApplication.SharedApplication.GetWindow();
						if (window != null)
						{
							var windowKey = (window.Handler?.PlatformView is NSObject nsObj ? (nint)nsObj.Handle : nint.Zero);
							if (windowKey != nint.Zero)
							{
								bool isActive = true;
								_windowActivationStates.TryGetValue(windowKey, out isActive);
								if (isActive)
								{
									_windowActivationStates[windowKey] = false;
									window.Unfocused();
								}
							}
						}
					}
				});

#endif

			iOS = iOS
					.OnPlatformWindowCreated((window) =>
					{
						window.GetWindow()?.Created();
						if (!KeyboardAutoManagerScroll.ShouldDisconnectLifecycle)
							KeyboardAutoManagerScroll.Connect();
					})
					.WillTerminate(app =>
					{
						// Clear activation states on termination and cleanup observers
						_windowActivationStates.Clear();

						if (_windowDidBecomeKeyObserver != null)
						{
							NSNotificationCenter.DefaultCenter.RemoveObserver(_windowDidBecomeKeyObserver);
							_windowDidBecomeKeyObserver = null;
						}

						if (_windowDidResignKeyObserver != null)
						{
							NSNotificationCenter.DefaultCenter.RemoveObserver(_windowDidResignKeyObserver);
							_windowDidResignKeyObserver = null;
						}
						// By this point if we were a multi window app, the GetWindow would be null anyway
						app.GetWindow()?.Destroying();
						KeyboardAutoManagerScroll.Disconnect();
					})
					.WillEnterForeground(app =>
					{
						if (!app.Delegate.HasSceneManifest())
							app.GetWindow()?.Resumed();
					})
					.OnActivated(app =>
					{
						if (!app.Delegate.HasSceneManifest())
							app.GetWindow()?.Activated();
					})
					.OnResignActivation(app =>
					{
						if (!app.Delegate.HasSceneManifest())
							app.GetWindow()?.Deactivated();
					})
					.DidEnterBackground(app =>
					{
						if (!app.Delegate.HasSceneManifest())
							app.GetWindow()?.Stopped();
					});

			iOS
				.SceneWillEnterForeground(scene =>
				{
					if (scene.Delegate is IUIWindowSceneDelegate windowScene &&
						scene.ActivationState != UISceneActivationState.Unattached)
					{
						windowScene.GetWindow().GetWindow()?.Resumed();
					}
				})
				.SceneOnActivated(scene =>
				{
					if (scene.Delegate is IUIWindowSceneDelegate sd)
						sd.GetWindow().GetWindow()?.Activated();
				})
				.SceneOnResignActivation(scene =>
				{
					if (scene.Delegate is IUIWindowSceneDelegate sd)
						sd.GetWindow().GetWindow()?.Deactivated();
				})
				.SceneDidEnterBackground(scene =>
				{
					if (scene.Delegate is IUIWindowSceneDelegate sd)
						sd.GetWindow().GetWindow()?.Stopped();
				})
				.SceneDidDisconnect(scene =>
				{
					if (scene.Delegate is IUIWindowSceneDelegate sd)
						sd.GetWindow().GetWindow()?.Destroying();
				});
		}

		static void OnConfigureWindow(IiOSLifecycleBuilder iOS)
		{
			iOS = iOS
				.WindowSceneDidUpdateCoordinateSpace((windowScene, _, _, _) =>
				{
					// Mac Catalyst version 16+ supports effectiveGeometry property on window scenes.
					if (OperatingSystem.IsMacCatalystVersionAtLeast(16))
					{
						return;
					}

					if (windowScene.Delegate is not IUIWindowSceneDelegate wsd ||
						wsd.GetWindow() is not UIWindow platformWindow)
					{
						return;
					}

					var window = platformWindow.GetWindow();
					if (window is null)
					{
						return;
					}

					window.FrameChanged(platformWindow.Frame.ToRectangle());
				});
		}
	}
}
