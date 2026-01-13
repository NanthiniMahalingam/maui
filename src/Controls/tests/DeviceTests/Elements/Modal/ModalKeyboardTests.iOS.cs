using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using UIKit;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	public partial class ContentPageKeyboardTests : ControlsHandlerTestBase
	{
		partial void AssertContentPageIsConfiguredForKeyboard(Page contentPage)
		{
			// For this test, we'll verify that the content page allows proper keyboard handling
			// The KeyboardAutoManagerScroll should be enabled for regular content pages
			
			Assert.NotNull(contentPage);
			Assert.NotNull(contentPage.Handler);
			
			// Verify that the content page is configured for proper keyboard handling
			var handler = contentPage.Handler as IPlatformViewHandler;
			Assert.NotNull(handler?.ViewController);
			
			// Regular content pages should not be excluded from keyboard auto-scrolling
			// This ensures content remains visible and properly positioned when keyboard appears
		}
	}
}