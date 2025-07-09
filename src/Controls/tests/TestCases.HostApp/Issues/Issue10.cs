using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample.Issues
{
	[Issue(IssueTracker.Github, 10, "iOS app crashes with EXC_BAD_ACCESS (SIGSEGV) after updating from .NET 8 to .NET 9 in CollectionView")]
	public class Issue10 : TestContentPage
	{
		const string Go = "Go";
		const string Success = "Success";
		const string Running = "Running...";

		protected override void Init()
		{
			var layout = new StackLayout();

			var instructions = new Label
			{
				Text = $"Tap the button marked '{Go}'. The CollectionView below should load items and scroll, " +
				$"and the '{Running}' label should change to '{Success}'. If the app crashes, this test has failed."
			};

			var result = new Label { Text = Running, AutomationId = "TestResult" };

			var viewModel = new Issue10ViewModel();

			var button = new Button { Text = Go, AutomationId = Go };
			button.Clicked += (obj, args) =>
			{
				try
				{
					// Simulate the scenario that causes the crash - adding many items to CollectionView
					// which triggers intensive handler creation and potential garbage collection
					for (int i = 0; i < 100; i++)
					{
						viewModel.Items.Add(new Issue10Item { Text = $"Item {i}" });
					}

					// Force garbage collection to potentially trigger the crash scenario
					GC.Collect();
					GC.WaitForPendingFinalizers();
					GC.Collect();

					result.Text = Success;
				}
				catch (Exception ex)
				{
					result.Text = $"Failed: {ex.Message}";
				}
			};

			var cv = new CollectionView { AutomationId = "TestCollectionView" };
			cv.ItemTemplate = new DataTemplate(() =>
			{
				var label = new Label();
				label.SetBinding(Label.TextProperty, new Binding("Text"));
				return label;
			});
			cv.ItemsSource = viewModel.Items;

			layout.Children.Add(instructions);
			layout.Children.Add(result);
			layout.Children.Add(button);
			layout.Children.Add(cv);

			Content = layout;
		}

		public class Issue10ViewModel
		{
			public ObservableCollection<Issue10Item> Items { get; set; }

			public Issue10ViewModel()
			{
				Items = new ObservableCollection<Issue10Item>();
				// Add initial items
				for (int n = 0; n < 10; n++)
				{
					Items.Add(new Issue10Item { Text = $"Initial Item {n}" });
				}
			}
		}

		public class Issue10Item
		{
			public string Text { get; set; }
		}
	}
}