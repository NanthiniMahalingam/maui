using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Xunit;

#if ANDROID || IOS || MACCATALYST
using ShellHandler = Microsoft.Maui.Controls.Handlers.Compatibility.ShellRenderer;
#endif

#if IOS || MACCATALYST
using NavigationViewHandler = Microsoft.Maui.Controls.Handlers.Compatibility.NavigationRenderer;
using FlyoutViewHandler = Microsoft.Maui.Controls.Handlers.Compatibility.PhoneFlyoutPageRenderer;
using TabbedViewHandler = Microsoft.Maui.Controls.Handlers.Compatibility.TabbedRenderer;
#endif

namespace Microsoft.Maui.DeviceTests
{
	[Category(TestCategory.Modal)]
#if ANDROID || IOS || MACCATALYST
	[Collection(ControlsHandlerTestBase.RunInNewWindowCollection)]
#endif
	public partial class ModalKeyboardTests : ControlsHandlerTestBase
	{
		void SetupBuilder()
		{
			EnsureHandlerCreated(builder =>
			{
				builder.ConfigureMauiHandlers(handlers =>
				{
					handlers.AddHandler(typeof(NavigationPage), typeof(NavigationViewHandler));
					handlers.AddHandler(typeof(FlyoutPage), typeof(FlyoutViewHandler));
					handlers.AddHandler(typeof(TabbedPage), typeof(TabbedViewHandler));
					handlers.AddHandler<Window, WindowHandlerStub>();
					handlers.AddHandler<Entry, EntryHandler>();
					SetupShellHandlers(handlers);
				});
			});
		}

		[Fact]
		public async Task ModalDialogMaintainsVerticalCenteringWithKeyboard()
		{
			SetupBuilder();
			var windowPage = new ContentPage()
			{
				Content = new Label() { Text = "Main Page" }
			};

			var modalPage = new ContentPage()
			{
				Content = new StackLayout
				{
					Children =
					{
						new Label { Text = "Modal Dialog" },
						new Entry { Placeholder = "Enter text to show keyboard" }
					}
				}
			};

			var window = new Window(windowPage);

			await CreateHandlerAndAddToWindow<IWindowHandler>(window,
				async (_) =>
				{
					await windowPage.Navigation.PushModalAsync(modalPage);
					await OnLoadedAsync(modalPage.Content);

					// Test that modal dialog is configured to maintain vertical centering
					// when keyboard appears
					AssertModalIsConfiguredForVerticalCentering(modalPage);
				});
		}

		partial void AssertModalIsConfiguredForVerticalCentering(Page modalPage);
	}
}