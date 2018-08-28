using Xamarin.Forms;

namespace BubbleWar
{
    public class VotePage : BaseContentPage<VoteViewModel>
    {
        public VotePage()
        {
            Padding = new Thickness(20);
            Title = "Vote";
            Icon = "Vote";

            var scorePieChart = new TeamScorePieChart { BackgroundColor = Color.Transparent };
            scorePieChart.SetBinding(TeamScorePieChart.ItemSourceProperty, nameof(ViewModel.TeamScoreCollection));

            var voteGreenTeamButton = new Button
            {
                Text = "Vote",
                TextColor = Color.White,
                BackgroundColor = Color.Green,
                CommandParameter = TeamColor.Green
            };
            voteGreenTeamButton.SetBinding(Button.CommandProperty, nameof(ViewModel.VoteButtonCommand));

            var voteRedTeamButton = new Button
            {
                Text = "Vote",
                TextColor = Color.White,
                BackgroundColor = Color.Red,
                CommandParameter = TeamColor.Red
            };
            voteRedTeamButton.SetBinding(Button.CommandProperty, nameof(ViewModel.VoteButtonCommand));

            var grid = new Grid
            {
                Margin = new Thickness(10),

                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },

                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                }
            };

            grid.Children.Add(scorePieChart, 0, 0);
            Grid.SetColumnSpan(scorePieChart, 2);

            grid.Children.Add(voteGreenTeamButton, 0, 1);
            grid.Children.Add(voteRedTeamButton, 1, 1);

            Content = grid;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.GraphQLConnectionFailed += HandleGraphQLConnectionFailed;

            AppCenterService.TrackEvent(AppCenterConstants.VotePageAppeared);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ViewModel.GraphQLConnectionFailed -= HandleGraphQLConnectionFailed;
        }

        void HandleGraphQLConnectionFailed(object sender, string message)
        {
            ViewModel.GraphQLConnectionFailed -= HandleGraphQLConnectionFailed;

            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("GraphQL Connection Failed", message, "OK");
                ViewModel.GraphQLConnectionFailed += HandleGraphQLConnectionFailed;
            });
        }
    }
}
