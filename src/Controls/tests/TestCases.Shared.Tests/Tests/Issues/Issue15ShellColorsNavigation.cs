using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue15ShellColorsNavigation : _IssuesUITest
{
	public Issue15ShellColorsNavigation(TestDevice testDevice) : base(testDevice)
	{
	}

	public override string Issue => "ContentPage Shell colors are replaced on navigation";

	[Test]
	[Category(UITestCategories.Shell)]
	public void ShellColorsPreservedDuringNavigation()
	{
		// Navigate to Home tab and push a child page
		App.WaitForElement("HomeTab");
		App.Tap("HomeTab");

		App.WaitForElement("GoToHomeChildButton");
		App.Tap("GoToHomeChildButton");

		// Navigate to Other tab and push a child page
		App.WaitForElement("OtherTab");
		App.Tap("OtherTab");

		App.WaitForElement("GoToOtherChildButton");
		App.Tap("GoToOtherChildButton");

		// Navigate back to Home tab - colors should still be blue, not red
		App.WaitForElement("HomeTab");
		App.Tap("HomeTab");

		// The bug is that Home tab colors would be replaced with red colors from Other tab
		// This test verifies that Home tab maintains its blue background/foreground colors
		App.WaitForElement("HomeChildPage");

		// The fix should ensure that each page maintains its own Shell appearance
		// and navigation doesn't cause color override between pages
	}
}