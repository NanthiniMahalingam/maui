namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 29824, "Shell Colors Do Not Update Correctly When Switching Between TabBar Items", PlatformAffected.Android)]
public class Issue29824Shell : Shell
{
	public Issue29824Shell()
	{
		// Register route for navigation
		Routing.RegisterRoute(nameof(Issue29824HomeDetailPage), typeof(Issue29824HomeDetailPage));
		Routing.RegisterRoute(nameof(Issue29824SettingsDetailPage), typeof(Issue29824SettingsDetailPage));

		// Create tabs
		var homeTab = new ShellContent
		{
			Title = "Home",
			ContentTemplate = new DataTemplate(() => new Issue29824HomePage())
		};

		var settingsTab = new ShellContent
		{
			Title = "Settings",
			ContentTemplate = new DataTemplate(() => new Issue29824SettingsPage())
		};

		var tabBar = new TabBar();
		tabBar.Items.Add(homeTab);
		tabBar.Items.Add(settingsTab);

		Items.Add(tabBar);
	}
}

public class Issue29824HomePage : ContentPage
{
	public Issue29824HomePage()
	{
		Title = "Home";

		var label = new Label
		{
			Text = "Welcome to the Home Page!",
			FontSize = 24,
			HorizontalOptions = LayoutOptions.Center
		};

		var button = new Button
		{
			Text = "Go to Detail Page",
			AutomationId = "HomePageButton",
			Margin = new Thickness(0, 20, 0, 0)
		};
		button.Clicked += async (s, e) =>
		{
			await Shell.Current.GoToAsync(nameof(Issue29824HomeDetailPage));
		};

		Content = new StackLayout
		{
			Padding = 20,
			Children = { label, button }
		};
	}
}

public class Issue29824HomeDetailPage : ContentPage
{
	public Issue29824HomeDetailPage()
	{
		Shell.SetForegroundColor(this, Colors.Pink);
		Shell.SetTitleColor(this, Colors.Cyan);
		Shell.SetBackgroundColor(this, Colors.Red);
		Title = "Home Detail";

		var label = new Label
		{
			Text = "This is the Home Detail Page.",
			HorizontalOptions = LayoutOptions.Center
		};

		Content = new StackLayout
		{
			Children = { label }
		};
	}
}

public class Issue29824SettingsPage : ContentPage
{
	public Issue29824SettingsPage()
	{
		Title = "Settings";

		var button = new Button
		{
			Text = "Go to Detail Page",
			AutomationId = "SettingsPageButton"
		};
		button.Clicked += async (s, e) =>
		{
			await Shell.Current.GoToAsync(nameof(Issue29824SettingsDetailPage));
		};

		Content = new StackLayout
		{
			Children = { button }
		};
	}
}

public class Issue29824SettingsDetailPage : ContentPage
{
	public Issue29824SettingsDetailPage()
	{
		Shell.SetForegroundColor(this, Colors.Pink);
		Shell.SetTitleColor(this, Colors.Cyan);
		Shell.SetBackgroundColor(this, Colors.Green);
		Title = "Setting Detail";

		var label = new Label
		{
			Text = "This is the Setting Detail Page.",
			HorizontalOptions = LayoutOptions.Center
		};

		Content = new StackLayout
		{
			Children = { label }
		};
	}
}

