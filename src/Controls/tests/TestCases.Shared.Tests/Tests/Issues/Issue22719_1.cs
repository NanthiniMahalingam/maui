using NUnit.Framework;
using UITest.Appium;
using UITest.Core;
namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue22719_1 : _IssuesUITest
{
    public Issue22719_1(TestDevice device) : base(device)
    { }

    public override string Issue => "The FlyoutPage flyout remains locked when changing the orientation from landscape to portrait with SplitOnLandscape";

    [Test]
    [Category(UITestCategories.FlyoutPage)]
    public void ShouldFlyoutBeVisibleAfterMaximizingWindow_1()
    {
        App.WaitForElement("Label");
# if ANDROID || IOS
        App.SetOrientationLandscape();
        App.SetOrientationPortrait();
#endif
        VerifyScreenshot();
    }
}