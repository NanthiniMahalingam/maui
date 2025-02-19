namespace Maui.Controls.Sample.Issues
{
	[Issue(IssueTracker.Github, 22719_1, "The FlyoutPage flyout remains locked when changing the orientation from landscape to portrait with SplitOnLandscape", PlatformAffected.macOS)]
	public partial class Issue22719_1 : FlyoutPage, IFlyoutPageController
	{
		public Issue22719_1()
		{
			InitializeComponent();

#if WINDOWS
			FlyoutLayoutBehavior = FlyoutLayoutBehavior.SplitOnPortrait;
#else
			FlyoutLayoutBehavior = FlyoutLayoutBehavior.SplitOnLandscape;
#endif
		}

		//  iPhone by default doesn't support split modes so we set this to validate split mode behaviors 
		bool IFlyoutPageController.ShouldShowSplitMode
		{
			get
			{
				var orientation = DeviceDisplay.Current.MainDisplayInfo.Orientation;
#if WINDOWS
				return orientation == DisplayOrientation.Portrait;
#else
				return orientation == DisplayOrientation.Landscape;
#endif
			}
		}
	}
}