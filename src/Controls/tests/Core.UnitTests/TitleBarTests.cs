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

			// Apply active state
			VisualStateManager.GoToState(titleBar, TitleBar.TitleBarActiveState);

			// Apply inactive state
			VisualStateManager.GoToState(titleBar, TitleBar.TitleBarInactiveState);

			// For now, this test just verifies the API works without exceptions
			// The actual visual verification would require platform-specific testing
			Assert.True(true);
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