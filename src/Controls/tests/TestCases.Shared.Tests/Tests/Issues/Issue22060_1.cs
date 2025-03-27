using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue22060_1 : _IssuesUITest
{
	public Issue22060_1(TestDevice testDevice) : base(testDevice)
	{
	}

	public override string Issue => "Label text gets cropped when a width request is specified on the label inside a VerticalStackLayout";

	[Test]
	[Category(UITestCategories.Label)]
	public void ShouldPreventLabelTextCroppingWithWidthRequest()
	{
		App.WaitForElement("Label");
		VerifyScreenshot();
	}
}