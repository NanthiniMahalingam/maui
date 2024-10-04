using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
namespace Maui.Controls.Sample.Issues;

[XamlCompilation(XamlCompilationOptions.Compile)]
[Issue(IssueTracker.Github, 19246, "Button doesn't reset to normal if scrolled quickly to the top in iOS", PlatformAffected.iOS)]
public partial class Issue19246 : ContentPage
{
   	public Issue19246()
	{
		InitializeComponent();
	}
}