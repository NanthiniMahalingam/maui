using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue29824 : _IssuesUITest
{
	public Issue29824(TestDevice testDevice) : base(testDevice)
	{
	}

	public override string Issue => "Shell Colors Do Not Update Correctly When Switching Between TabBar Items";

	[Test]
	[Category(UITestCategories.Shell)]
	public void ShellColorsShouldNotChangeOnNavigation()
	{
		App.WaitForElement("HomePageButton");
		App.Tap("HomePageButton");
		App.Click("Settings");
		App.WaitForElement("SettingsPageButton");
		App.Tap("SettingsPageButton");
		App.Tap("Home");
		VerifyScreenshot();
	}
}