using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace VotingApp
{
    class VotePage : BaseContentPage<VoteViewModel>
    {
        public VotePage()
        {
            ViewModel.GraphQLConnectionFailed += HandleGraphQLConnectionFailed;

            Padding = new Thickness(20);
            Title = "Vote";
            Icon = "Vote";

            var loadingLabel = new Label
            {
                Text = "Loading Data...",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            loadingLabel.SetBinding(IsVisibleProperty, nameof(ViewModel.IsDataLoading));

            var scorePieChart = new TeamScorePieChart { BackgroundColor = Color.Transparent };
            scorePieChart.SetBinding(TeamScorePieChart.ItemSourceProperty, nameof(ViewModel.TeamScoreCollection));

            var voteGreenTeamButton = new VotingButton(TeamColor.Green);
            voteGreenTeamButton.SetBinding(Button.CommandProperty, nameof(ViewModel.VoteButtonCommand));

            var voteRedTeamButton = new VotingButton(TeamColor.Red);

            voteRedTeamButton.SetBinding(Button.CommandProperty, nameof(ViewModel.VoteButtonCommand));

            var grid = new Grid
            {
                Margin = new Thickness(10),

                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },

                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                }
            };

            grid.Children.Add(loadingLabel, 0, 0);
            Grid.SetColumnSpan(loadingLabel, 2);

            grid.Children.Add(scorePieChart, 0, 0);
            Grid.SetColumnSpan(scorePieChart, 2);

            grid.Children.Add(voteGreenTeamButton, 0, 1);
            grid.Children.Add(voteRedTeamButton, 1, 1);

            Content = grid;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            AppCenterService.TrackEvent(AppCenterConstants.VotePageAppeared);
        }

        void HandleGraphQLConnectionFailed(object sender, string message)
        {
            ViewModel.GraphQLConnectionFailed -= HandleGraphQLConnectionFailed;

            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Could Not Connect to GraphQL Endpoint", "Auto-Update Disabled\n\nRe-enable Auto-Update in Settings", "OK");

                ViewModel.GraphQLConnectionFailed += HandleGraphQLConnectionFailed;
            });
        }

        class VotingButton : Button
        {
            public VotingButton(TeamColor teamColor)
            {
                switch (teamColor)
                {
                    case TeamColor.Red:
                        BackgroundColor = Color.Red;
                        break;

                    case TeamColor.Green:
                        BackgroundColor = Color.Green;
                        break;

                    default:
                        throw new NotSupportedException($"{teamColor.GetType()} Not Supported");
                }

                Text = "Vote";
                CornerRadius = 10;
                TextColor = Color.White;
                CommandParameter = teamColor;
            }
        }
    }
}
