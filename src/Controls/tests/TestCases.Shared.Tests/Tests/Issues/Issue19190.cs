using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;
public class Issue19190 : _IssuesUITest
{
	public Issue19190(TestDevice testDevice) : base(testDevice)
	{
	}

	public override string Issue => "Setting an implicit style on a Label causes spans with FontAttributes to be ignored on Android";

	[Test]
	[Category(UITestCategories.Label)]
	public void ShouldDisplayTextWithFontAttributesInSpansOnAndroid()
	{
		App.WaitForElement("Label");
		VerifyScreenshot();
	}
}