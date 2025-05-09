﻿#if TEST_FAILS_ON_WINDOWS // On Windows AutomationId is not working for Stacklayout, Hence we used to swipe the layout here so we can't use the inner elements AutomationId. More Information:https://github.com/dotnet/maui/issues/4715
using NUnit.Framework;
using NUnit.Framework.Legacy;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class Issue9306 : _IssuesUITest
	{
		const string SwipeViewId = "SwipeViewId";
		const string SwipeItemId = "SwipeItemId";
		const string LeftCountLabelId = "LeftCountLabel";

		public Issue9306(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "[iOS] Cannot un-reveal swipe view items on iOS / Inconsistent swipe view behaviour";

		[Test]
		[Category(UITestCategories.SwipeView)]
		public void Issue9306SwipeViewCloseSwiping()
		{
			App.WaitForElement(SwipeViewId);

			App.SwipeLeftToRight(SwipeViewId);
			App.SwipeRightToLeft(SwipeViewId);
			App.SwipeLeftToRight(SwipeViewId);

			App.Tap(SwipeItemId);

			var result = App.FindElement(LeftCountLabelId).GetText();

			Assert.That(result, Is.EqualTo("1"));
		}
	}
}
#endif