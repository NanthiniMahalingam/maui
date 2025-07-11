using System.Collections.ObjectModel;

namespace Maui.Controls.Sample.Issues
{
	[Issue(IssueTracker.Github, 17, "VerticalGrid column in CollectionView does not shrink in Windows", PlatformAffected.Windows)]
	public partial class IssueVerticalGridShrinking : ContentPage
	{
		public ObservableCollection<string> Items { get; set; }

		public IssueVerticalGridShrinking()
		{
			InitializeComponent();
			
			Items = new ObservableCollection<string>();
			for (int i = 1; i <= 20; i++)
			{
				Items.Add($"Item {i}");
			}
			
			BindingContext = this;
		}
	}
}