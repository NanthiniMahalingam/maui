using System;
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Tests.Issues;

public class Issue19246 : _IssuesUITest
{
        public Issue19246(TestDevice testDevice) : base(testDevice)
		{
		}

		public override string Issue => "Button doesn't reset to normal if scrolled quickly to the top in iOS"; 
		
		[Test]
		[Category(UITestCategories.Button)]
		public void ButtonDoesNotResetToNormalState()
		{
			App.ScrollTo("Button19");
			App.Tap("Button18");
			App.Screenshot("ButtonDoesNotResetToNormalState");
		}
}
