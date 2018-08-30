using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace BubbleWar
{
    public class VoteViewModel : BaseViewModel
    {
        #region Fields
        List<TeamScore> _teamScoreCollection = new List<TeamScore>();
        ICommand _voteButtonCommand, _startUpdateScoreTimerCommand, _updateScoreCommand;
        #endregion

        #region Constructors
        public VoteViewModel() => StartUpdateScoreTimerCommand?.Execute(null);
        #endregion

        #region Events
        public event EventHandler<string> GraphQLConnectionFailed;
        #endregion

        #region Properties
        public ICommand VoteButtonCommand => _voteButtonCommand ??
            (_voteButtonCommand = new Command<TeamColor>(async teamColor => await ExecuteVoteButtonCommand(teamColor).ConfigureAwait(false)));

        public ICommand StartUpdateScoreTimerCommand => _startUpdateScoreTimerCommand ??
        (_startUpdateScoreTimerCommand = new Command(() => Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            UpdateScoreCommand?.Execute(null);
            return GraphQLSettings.ShouldUpdateChartAutomatically;
        })));

        public List<TeamScore> TeamScoreCollection
        {
            get => _teamScoreCollection;
            set => SetProperty(ref _teamScoreCollection, value);
        }

        ICommand UpdateScoreCommand => _updateScoreCommand ??
            (_updateScoreCommand = new Command(async () => await UpdateScores().ConfigureAwait(false)));
        #endregion

        #region Methods
        async Task ExecuteVoteButtonCommand(TeamColor teamColor)
        {
            AppCenterService.TrackEvent(AppCenterConstants.VotingButtonTapped, nameof(TeamColor), teamColor.ToString());

            try
            {
                await GraphQLService.VoteForTeam(teamColor).ConfigureAwait(false);
                await UpdateScores().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                AppCenterService.Report(e);
                GraphQLSettings.ShouldUpdateChartAutomatically = false;

                OnGraphQLConnectionFailed(e.Message);
            }
        }

        async Task UpdateScores()
        {
            try
            {
                TeamScoreCollection = await GraphQLService.GetTeamScoreList().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                AppCenterService.Report(e);
                GraphQLSettings.ShouldUpdateChartAutomatically = false;

                OnGraphQLConnectionFailed(e.Message);
            }
        }

        void OnGraphQLConnectionFailed(string message) => GraphQLConnectionFailed?.Invoke(this, message);
        #endregion
    }
}
