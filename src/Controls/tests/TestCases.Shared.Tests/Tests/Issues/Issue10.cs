using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class Issue10 : _IssuesUITest
	{
		public Issue10(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "iOS app crashes with EXC_BAD_ACCESS (SIGSEGV) after updating from .NET 8 to .NET 9 in CollectionView";

		[Test]
		[Category(UITestCategories.CollectionView)]
		[Category(UITestCategories.Compatibility)]
		public void CollectionViewShouldNotCrashOnNet9iOS()
		{
			App.WaitForElement("Go");
			App.Tap("Go");

			// Wait for the test to complete
			App.WaitForElement("TestResult");
			
			// Check that the test succeeded and didn't crash
			var result = App.WaitForElement("TestResult");
			Assert.That(result.GetText(), Is.EqualTo("Success"), "The CollectionView test should succeed without crashing");
		}
	}
}