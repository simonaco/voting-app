using Xamarin.Forms;

namespace BubbleWar
{
    public class VotePage : BaseContentPage<VoteViewModel>
    {
        public VotePage()
        {
            var greenScoreLabel = new Label();
            greenScoreLabel.SetBinding(Label.TextProperty, nameof(ViewModel.GreenScore));

            var redScoreLabel = new Label();
            redScoreLabel.SetBinding(Label.TextProperty, nameof(ViewModel.RedScore));

            var voteGreenTeamButton = new Button
            {
                Text = "Vote",
                BackgroundColor = Color.Green,
                CommandParameter = TeamColor.Green
            };
            voteGreenTeamButton.SetBinding(Button.CommandProperty, nameof(ViewModel.VoteButtonCommand));

            var voteRedTeamButton = new Button
            {
                Text = "Vote",
                BackgroundColor = Color.Red,
                CommandParameter = TeamColor.Red
            };
            voteRedTeamButton.SetBinding(Button.CommandProperty, nameof(ViewModel.VoteButtonCommand));

            var grid = new Grid
            {
                RowDefinitions = {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },

                ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                }
            };

            grid.Children.Add(greenScoreLabel, 0, 0);
            grid.Children.Add(redScoreLabel, 1, 0);

            grid.Children.Add(voteGreenTeamButton, 0, 1);
            grid.Children.Add(voteRedTeamButton, 1, 1);

            Content = grid;

            Title = "Vote";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.GraphQLConnectionFailed += HandleGraphQLConnectionFailed;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ViewModel.GraphQLConnectionFailed -= HandleGraphQLConnectionFailed;
        }

        void HandleGraphQLConnectionFailed(object sender, string message) => DisplayAlert("GraphQL Connection Failed", message, "OK");
    }
}
