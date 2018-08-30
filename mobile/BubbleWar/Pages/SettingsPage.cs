using System;
using System.Linq;

using Xamarin.Forms;

namespace BubbleWar
{
    public class SettingsPage : BaseContentPage<SettingsViewModel>
    {
        readonly Switch _shouldUpdateChartAutomaticallySwitch;
        readonly Editor _graphqlApiEndpointEditor;

        public SettingsPage()
        {
            const int labelHeight = 20;

            Padding = new Thickness(20);
            Title = "Settings";
            BackgroundColor = Color.FromHex("F4F3FA");

            var graphqlApiEndpointLabel = new Label
            {
                Text = "GraphQL API Endpoint",
                FontAttributes = FontAttributes.Bold
            };

            _graphqlApiEndpointEditor = new Editor
            {
                BackgroundColor = Color.FromHex("F2F2F2"),
                IsSpellCheckEnabled = false,
                Keyboard = Keyboard.Url
            };
            _graphqlApiEndpointEditor.Unfocused += HandleDeviceConnectionStringEditorCompleted;


            var shouldUpdateChartAutomaticallyLabel = new Label
            {
                Text = "Update Chart Automatically",
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                VerticalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold
            };

            _shouldUpdateChartAutomaticallySwitch = new Switch
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start
            };
            _shouldUpdateChartAutomaticallySwitch.Toggled += HandleIsSendDataToAzureEnabledSwitchToggled;

            var createdByLabel = new Label
            {
                Text = "App Created by Brandon Minnick",
                FontSize = 12,
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End
            };
            createdByLabel.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(CreatedByLabelTapped) });

            var grid = new Grid
            {
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(labelHeight, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(labelHeight, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(labelHeight, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                }
            };

            grid.Children.Add(graphqlApiEndpointLabel, 0, 0);
            grid.Children.Add(_graphqlApiEndpointEditor, 0, 1);
            grid.Children.Add(shouldUpdateChartAutomaticallyLabel, 0, 3);
            grid.Children.Add(_shouldUpdateChartAutomaticallySwitch, 0, 4);
            grid.Children.Add(createdByLabel, 0, 4);

            Content = grid;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            AppCenterService.TrackEvent(AppCenterConstants.VotePageAppeared);

            _graphqlApiEndpointEditor.Text = GraphQLSettings.Uri.ToString();
            _shouldUpdateChartAutomaticallySwitch.IsToggled = GraphQLSettings.ShouldUpdateChartAutomatically;
        }

        async void CreatedByLabelTapped()
        {
            AppCenterService.TrackEvent(AppCenterConstants.CreatedByLabelTapped);

            await DependencyService.Get<IDeepLinks>().OpenTwitter().ConfigureAwait(false);
        }

        void HandleDeviceConnectionStringEditorCompleted(object sender, EventArgs e)
        {
            var editor = sender as Editor;

            if (Uri.TryCreate(editor.Text, UriKind.Absolute, out var uri))
            {
                GraphQLSettings.Uri = uri;
                _shouldUpdateChartAutomaticallySwitch.IsToggled = true;
            }
            else
            {
                Device.BeginInvokeOnMainThread(() => DisplayAlert("Invalid Uri", "The url entered is invalid", "OK"));
                _shouldUpdateChartAutomaticallySwitch.IsToggled = false;
            }
        }

        void HandleIsSendDataToAzureEnabledSwitchToggled(object sender, ToggledEventArgs e)
        {
            GraphQLSettings.ShouldUpdateChartAutomatically = e.Value;

            if (e.Value)
            {
                var tabbedPage = Application.Current.MainPage as TabbedPage;
                var votePage = tabbedPage?.Children?.OfType<VotePage>()?.FirstOrDefault();
                var voteViewModel = votePage?.BindingContext as VoteViewModel;

                voteViewModel?.StartUpdateScoreTimerCommand?.Execute(null);
            }
        }
    }
}
