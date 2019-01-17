using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;

using Xamarin.Forms;

namespace VotingApp
{
    class VoteViewModel : BaseViewModel
    {
        #region Constant Fields
        readonly WeakEventManager<string> _graphQLConnectionFailedEventManager = new WeakEventManager<string>();
        #endregion

        #region Fields
        int _dataLoadingCount = 0;
        bool _isDataLoading = true;
        List<TeamScore> _teamScoreCollection = new List<TeamScore>();
        ICommand _voteButtonCommand, _startUpdateScoreTimerCommand;
        #endregion

        #region Constructors
        public VoteViewModel() => StartUpdateScoreTimerCommand?.Execute(null);
        #endregion

        #region Events
        public event EventHandler<string> GraphQLConnectionFailed
        {
            add => _graphQLConnectionFailedEventManager.AddEventHandler(value);
            remove => _graphQLConnectionFailedEventManager.RemoveEventHandler(value);
        }
        #endregion

        #region Properties
        public ICommand VoteButtonCommand => _voteButtonCommand ??
            (_voteButtonCommand = new AsyncCommand<TeamColor>(ExecuteVoteButtonCommand, continueOnCapturedContext: false));

        public ICommand StartUpdateScoreTimerCommand => _startUpdateScoreTimerCommand ??
        (_startUpdateScoreTimerCommand = new Command(() => Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            UpdateScores().SafeFireAndForget();
            return GraphQLSettings.ShouldUpdateChartAutomatically;
        })));

        public List<TeamScore> TeamScoreCollection
        {
            get => _teamScoreCollection;
            set => SetProperty(ref _teamScoreCollection, value);
        }

        public bool IsDataLoading
        {
            get => _isDataLoading;
            set => SetProperty(ref _isDataLoading, value);
        }
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
            UpdateIsDataLoading(true);

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
            finally
            {
                UpdateIsDataLoading(false);
            }
        }

        void UpdateIsDataLoading(bool isLoadingData)
        {
            if (isLoadingData)
            {
                _dataLoadingCount++;
                IsDataLoading = true;
            }
            else if (--_dataLoadingCount <= 0)
            {
                _dataLoadingCount = 0;
                IsDataLoading = false;
            }
        }

        void OnGraphQLConnectionFailed(string message) => _graphQLConnectionFailedEventManager?.HandleEvent(this, message, nameof(GraphQLConnectionFailed));
        #endregion
    }
}
