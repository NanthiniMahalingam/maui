namespace Maui.Controls.Sample.Issues
{
	[Issue(IssueTracker.Github, 27196, "The editor text content is hidden when adding text on a new line", PlatformAffected.UWP)]
	public class Issue27196 : TestContentPage
	{
		protected override void Init()
		{
			var stackLayout = new StackLayout
			{
				Padding = 20,
			};

			var editor = new Editor
			{
				Text = "1. Enter text into the editor with a new line.\n2. Ensure that the newly added editor text content appears without being hidden.",
				AutomationId = "Editor",

			};
			Button button = new Button()
			{
				Text = "Click Me",
				AutomationId = "Button",

			};

			button.Clicked += (s, e) =>
			{
				editor.CursorPosition = editor.Text.Length;
				editor.Focus();
				editor.Text += "\n";
			};

			stackLayout.Children.Add(button);
			stackLayout.Children.Add(editor);

			var scrollView = new ScrollView
			{
				Content = stackLayout
			};

			Content = scrollView;

		}
	}
}