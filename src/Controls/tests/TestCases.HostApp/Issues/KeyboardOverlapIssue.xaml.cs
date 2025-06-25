namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 6, "Keyboard Overlaps ContentPage Content and Prevents Vertical Centering When Entry Is Focused", PlatformAffected.iOS | PlatformAffected.Android)]
public partial class KeyboardOverlapIssue : ContentPage
{
	public KeyboardOverlapIssue()
	{
		InitializeComponent();
	}

	private void OnEntryFocused(object sender, FocusEventArgs e)
	{
		// This test case validates that when an Entry inside a Border with VerticalOptions="End" is focused,
		// the keyboard appears without overlapping the content and allows proper vertical centering
		System.Diagnostics.Debug.WriteLine("Entry focused - testing keyboard overlap fix");
	}
}