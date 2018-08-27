using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace BubbleWar
{
    public class VoteViewModel : BaseViewModel
    {
        #region Fields
        int _redScore, _greenScore;
        ICommand _voteButtonCommand, _updateScoreCommand;
        #endregion

        #region Constructors
        public VoteViewModel() => UpdateScoreCommand?.Execute(null);
        #endregion

        #region Events
        public event EventHandler<string> GraphQLConnectionFailed;
        #endregion

        #region Properties
        public ICommand VoteButtonCommand => _voteButtonCommand ??
            (_voteButtonCommand = new Command<TeamColor>(async teamColor => await ExecuteVoteButtonCommand(teamColor).ConfigureAwait(false)));

        public int RedScore
        {
            get => _redScore;
            set => SetProperty(ref _redScore, value);
        }

        public int GreenScore
        {
            get => _greenScore;
            set => SetProperty(ref _greenScore, value);
        }

        ICommand UpdateScoreCommand => _updateScoreCommand ??
            (_updateScoreCommand = new Command(async () => await UpdateScores().ConfigureAwait(false)));
        #endregion

        #region Methods
        async Task ExecuteVoteButtonCommand(TeamColor teamColor)
        {
            try
            {
                await GraphQLSerqvice.VoteForTeam(teamColor).ConfigureAwait(false);
                await UpdateScores().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                OnGraphQLConnectionFailed(e.Message);
            }
        }

        async Task UpdateScores()
        {
            try
            {
                var teamScoreList = await GraphQLSerqvice.GetTeamScoreList().ConfigureAwait(false);

                RedScore = teamScoreList.Where(x => x.Color.Equals(TeamColor.Red))?.FirstOrDefault()?.Points ?? 0;
                GreenScore = teamScoreList.Where(x => x.Color.Equals(TeamColor.Green))?.FirstOrDefault()?.Points ?? 0;
            }
            catch (Exception e)
            {
                OnGraphQLConnectionFailed(e.Message);
            }
        }

        void OnGraphQLConnectionFailed(string message) => GraphQLConnectionFailed?.Invoke(this, message);
        #endregion
    }
}
