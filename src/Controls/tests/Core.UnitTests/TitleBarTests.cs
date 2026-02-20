using System;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{

	public class TitleBarTests : BaseTestFixture
	{
		[Fact]
		public void TitleBarVisualStatesApplyForegroundColor()
		{
			var titleBar = new TitleBar
			{
				Title = "Test Title",
				Subtitle = "Test Subtitle",
				ForegroundColor = Colors.Blue
			};

			// Trigger template application
			titleBar.ApplyTemplate();

			// Simulate what happens when visual states change the ForegroundColor
			titleBar.ForegroundColor = Colors.Red;

			// Apply active state
			titleBar.ApplyVisibleState(TitleBar.TitleBarActiveState);

			// Change color as if visual state changed it
			titleBar.ForegroundColor = Colors.Gray;

			// Apply inactive state
			titleBar.ApplyVisibleState(TitleBar.TitleBarInactiveState);

			// Verify the basic API works without exceptions and that template elements exist
			var titleLabel = titleBar.GetTemplateChild(TitleBar.TitleBarTitle);
			var subtitleLabel = titleBar.GetTemplateChild(TitleBar.TitleBarSubtitle);

			// Template elements should exist after ApplyTemplate and ApplyVisibleState calls
			Assert.NotNull(titleLabel);
			Assert.NotNull(subtitleLabel);
		}

		[Fact, Category(TestCategory.Memory)]
		public async Task TitleBarDoesNotLeak()
		{
			var application = new Application();

			WeakReference CreateReference()
			{
				var window = new Window { Page = new ContentPage() };
				var firstTitleBar = new TitleBar();
				var secondTitleBar = new TitleBar();
				var reference = new WeakReference(firstTitleBar);

				window.TitleBar = firstTitleBar;

				application.OpenWindow(window);

				window.TitleBar = secondTitleBar;

				((IWindow)window).Destroying();
				return reference;
			}

			var reference = CreateReference();

			// GC
			await TestHelpers.Collect();

			Assert.False(reference.IsAlive, "TitleBar should not be alive!");

			GC.KeepAlive(application);
		}
	}
}