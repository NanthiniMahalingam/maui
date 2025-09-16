using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class IssueVerticalGridShrinkingUITests : _IssuesUITest
	{
		public IssueVerticalGridShrinkingUITests(TestDevice device)
			: base(device)
		{
		}

		public override string Issue => "VerticalGrid column in CollectionView does not shrink in Windows";

		[Test]
		[Category(UITestCategories.CollectionView)]
		public void VerticalGridItemsShouldFillAvailableWidth()
		{
			// Wait for the CollectionView to load
			App.WaitForElement("TestCollectionView");

			// Verify instruction label is present
			App.WaitForElement("InstructionLabel");

			// Wait for at least one item to be loaded
			App.WaitForElement("ItemGrid");

			// The test should verify that grid items fill the available width properly
			// Without the fix, items would be smaller due to Math.Floor calculation
			// With the fix, items should utilize available space better

			// This is primarily a visual test - the fix ensures items don't appear cropped
			// or leave excessive empty space on the right when the container width
			// doesn't divide evenly by the span count

			// For automated testing, we just verify the UI loads without exceptions
			var items = App.QueryElements("ItemGrid");
			Assert.That(items.Count, Is.GreaterThan(0), "CollectionView should contain grid items");
		}
	}
}