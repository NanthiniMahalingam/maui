using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 15, "ContentPage Shell colors are replaced on navigation", PlatformAffected.Android)]
public class Issue15ShellColorsNavigation : TestShell
{
	public const string HomeTabAutomationId = "HomeTab";
	public const string OtherTabAutomationId = "OtherTab";
	public const string GoToHomeChildButtonAutomationId = "GoToHomeChildButton";
	public const string GoToOtherChildButtonAutomationId = "GoToOtherChildButton";
	public const string HomeChildPageAutomationId = "HomeChildPage";
	public const string OtherChildPageAutomationId = "OtherChildPage";

	protected override void Init()
	{
		// Create Home tab with Blue colors
		var homeTab = new Tab
		{
			Title = "Home",
			AutomationId = HomeTabAutomationId,
			Items =
			{
				new ShellContent
				{
					Content = CreateHomePage()
				}
			}
		};

		// Create Other tab with Red colors
		var otherTab = new Tab
		{
			Title = "Other",
			AutomationId = OtherTabAutomationId,
			Items =
			{
				new ShellContent
				{
					Content = CreateOtherPage()
				}
			}
		};

		Items.Add(homeTab);
		Items.Add(otherTab);
	}

	private ContentPage CreateHomePage()
	{
		var homePage = new ContentPage
		{
			Title = "Home Page",
			Content = new StackLayout
			{
				Children =
				{
					new Label { Text = "Home Page - Blue colors" },
					new Button
					{
						Text = "Go to child",
						AutomationId = GoToHomeChildButtonAutomationId,
						Command = new Command(async () =>
						{
							var childPage = new ContentPage
							{
								Title = "Home Child",
								AutomationId = HomeChildPageAutomationId,
								Content = new StackLayout
								{
									Children =
									{
										new Label { Text = "Home Child Page - Blue colors" }
									}
								}
							};
                            
                            // Set blue colors for Home child
                            Shell.SetBackgroundColor(childPage, Colors.Blue);
							Shell.SetForegroundColor(childPage, Colors.White);

							await Navigation.PushAsync(childPage);
						})
					}
				}
			}
		};

		// Set blue colors for Home
		Shell.SetBackgroundColor(homePage, Colors.Blue);
		Shell.SetForegroundColor(homePage, Colors.White);

		return homePage;
	}

	private ContentPage CreateOtherPage()
	{
		var otherPage = new ContentPage
		{
			Title = "Other Page",
			Content = new StackLayout
			{
				Children =
				{
					new Label { Text = "Other Page - Red colors" },
					new Button
					{
						Text = "Go to child",
						AutomationId = GoToOtherChildButtonAutomationId,
						Command = new Command(async () =>
						{
							var childPage = new ContentPage
							{
								Title = "Other Child",
								AutomationId = OtherChildPageAutomationId,
								Content = new StackLayout
								{
									Children =
									{
										new Label { Text = "Other Child Page - Red colors" }
									}
								}
							};
                            
                            // Set red colors for Other child
                            Shell.SetBackgroundColor(childPage, Colors.Red);
							Shell.SetForegroundColor(childPage, Colors.White);

							await Navigation.PushAsync(childPage);
						})
					}
				}
			}
		};

		// Set red colors for Other
		Shell.SetBackgroundColor(otherPage, Colors.Red);
		Shell.SetForegroundColor(otherPage, Colors.White);

		return otherPage;
	}
}