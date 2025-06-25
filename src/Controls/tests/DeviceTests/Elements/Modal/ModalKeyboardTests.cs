using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Xunit;

#if ANDROID || IOS || MACCATALYST
using ShellHandler = Microsoft.Maui.Controls.Handlers.Compatibility.ShellRenderer;
#endif

#if IOS || MACCATALYST
using NavigationViewHandler = Microsoft.Maui.Controls.Handlers.Compatibility.NavigationRenderer;
using FlyoutViewHandler = Microsoft.Maui.Controls.Handlers.Compatibility.PhoneFlyoutPageRenderer;
using TabbedViewHandler = Microsoft.Maui.Controls.Handlers.Compatibility.TabbedRenderer;
#endif

namespace Microsoft.Maui.DeviceTests
{
	[Category(TestCategory.Entry)]
#if ANDROID || IOS || MACCATALYST
	[Collection(ControlsHandlerTestBase.RunInNewWindowCollection)]
#endif
	public partial class ContentPageKeyboardTests : ControlsHandlerTestBase
	{
		void SetupBuilder()
		{
			EnsureHandlerCreated(builder =>
			{
				builder.ConfigureMauiHandlers(handlers =>
				{
					handlers.AddHandler(typeof(NavigationPage), typeof(NavigationViewHandler));
					handlers.AddHandler(typeof(FlyoutPage), typeof(FlyoutViewHandler));
					handlers.AddHandler(typeof(TabbedPage), typeof(TabbedViewHandler));
					handlers.AddHandler<Window, WindowHandlerStub>();
					handlers.AddHandler<Entry, EntryHandler>();
					SetupShellHandlers(handlers);
				});
			});
		}

		[Fact]
		public async Task ContentPageHandlesKeyboardProperly()
		{
			SetupBuilder();
			var contentPage = new ContentPage()
			{
				Content = new Grid
				{
					RowDefinitions =
					{
						new RowDefinition { Height = GridLength.Star },
						new RowDefinition { Height = new GridLength(1.5, GridUnitType.Star) },
						new RowDefinition { Height = GridLength.Auto },
						new RowDefinition { Height = GridLength.Star },
						new RowDefinition { Height = new GridLength(0.75, GridUnitType.Star) }
					},
					Children =
					{
						new VerticalStackLayout
						{
							Spacing = 2,
							Margin = new Thickness(0, 4, 0, 4),
							Children =
							{
								new Border
								{
									BackgroundColor = Colors.White,
									Stroke = Color.FromArgb("#66000000"),
									StrokeThickness = 1,
									Padding = new Thickness(5, 0),
									Content = new Entry
									{
										TextColor = Color.FromArgb("#DE000000"),
										Placeholder = "Password: test_Password",
										IsPassword = true,
										HeightRequest = 40
									}
								}
							}
						}.Row(2)
					}
				}
			};

			var window = new Window(contentPage);

			await CreateHandlerAndAddToWindow<IWindowHandler>(window,
				async (_) =>
				{
					await OnLoadedAsync(contentPage.Content);

					// Test that content page is configured to handle keyboard properly
					// without overlapping content and maintaining proper layout
					AssertContentPageIsConfiguredForKeyboard(contentPage);
				});
		}

		partial void AssertContentPageIsConfiguredForKeyboard(Page contentPage);
	}
}