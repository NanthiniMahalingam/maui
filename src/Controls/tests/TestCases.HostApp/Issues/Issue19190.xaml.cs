namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 19190, "Setting an implicit style on a Label causes spans with FontAttributes to be ignored on Android", PlatformAffected.iOS)]
public partial class Issue19190 : ContentPage
{
	public Issue19190()
	{
		InitializeComponent();
		BindingContext = new Issue19190ViewModel();
	}
}

public class Issue19190ViewModel
{
	public string NotBold { get; set; }
	public string Bold { get; set; }

	public Issue19190ViewModel()
	{
		NotBold = "This is not bold text";
		Bold = "This is bold text";
	}
}

public class Issue19190Label : Label
{
	public Issue19190Label()
	{
	}
}