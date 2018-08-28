using System;

using Xamarin.Forms;

namespace BubbleWar
{
    public class SettingsPage : BaseContentPage<SettingsViewModel>
    {
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
                IsSpellCheckEnabled = false,
            };
            _graphqlApiEndpointEditor.Unfocused += HandleDeviceConnectionStringEditorCompleted;

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
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                }
            };

            grid.Children.Add(graphqlApiEndpointLabel, 0, 0);
            grid.Children.Add(_graphqlApiEndpointEditor, 0, 1);
            grid.Children.Add(createdByLabel, 0, 3);

            Content = grid;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            _graphqlApiEndpointEditor.Text = GraphQLSettings.Uri.ToString();
        }

        async void CreatedByLabelTapped() => await DependencyService.Get<IDeepLinks>().OpenTwitter().ConfigureAwait(false);

        void HandleDeviceConnectionStringEditorCompleted(object sender, EventArgs e)
        {
            var editor = sender as Editor;

            if (Uri.TryCreate(editor.Text, UriKind.Absolute, out var uri))
                GraphQLSettings.Uri = uri;
            else
                Device.BeginInvokeOnMainThread(() => DisplayAlert("Invalid Uri", "The url entered is invalid", "OK"));
        }
    }
}
