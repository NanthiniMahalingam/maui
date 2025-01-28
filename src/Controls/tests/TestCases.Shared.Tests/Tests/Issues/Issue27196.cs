using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue27196(TestDevice testDevice) : _IssuesUITest(testDevice)
{
	public override string Issue => "The editor text content is hidden when adding text on a new line";
	string editorText = "3. The newly added text appears, and the editor content is scrollable.";
	[Test]
	[Category(UITestCategories.Editor)]
	public void ShouldEditorContentAppearWithoutBeingHidden()
	{
		App.WaitForElement("Editor");
		App.Tap("Button");
		App.EnterText("Editor", editorText);
		VerifyScreenshot();
	}
}
