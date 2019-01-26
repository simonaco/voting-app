using System;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Refit;
using Polly;

namespace VotingApp
{
    static class GraphQLService
    {
        #region Fields
        static IGraphQLAPI _graphQLApiClient = CreateGraphQLAPIClient(GraphQLSettings.Uri);
        #endregion

        #region Constructors
        static GraphQLService() => GraphQLSettings.UriChanged += HandleUriChanged;
        #endregion

        #region Methods
        public static async Task<List<TeamScore>> GetTeamScoreList()
        {
            const string requestString = "query{teams{name, points}}";

            var data = await ExecuteGraphQLFunction(() => _graphQLApiClient.TeamsQuery(new GraphQLRequest(requestString))).ConfigureAwait(false);

            return data.Teams;
        }

        public static async Task<TeamScore> VoteForTeamAndGetCurrentScore(TeamColor teamType)
        {
            var request = "mutation {incrementPoints(id:" + (int)teamType + ") {name, points}}";

            var data = await ExecuteGraphQLFunction(() => _graphQLApiClient.IncrementPoints(new GraphQLRequest(request))).ConfigureAwait(false);

            return data.TeamScore;
        }

        public static Task VoteForTeam(TeamColor teamType) => VoteForTeamAndGetCurrentScore(teamType);

        static async Task<T> ExecuteGraphQLFunction<T>(Func<Task<GraphQLResponse<T>>> action, int numRetries = 3)
        {
            var response = await Policy
                                    .Handle<Exception>()
                                    .WaitAndRetryAsync
                                    (
                                        numRetries,
                                        pollyRetryAttempt
                                    ).ExecuteAsync(action);

            if (response.Errors != null)
                throw new AggregateException(response.Errors.Select(x => new Exception(x.Message)));

            return response.Data;

            TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
        }

        static void HandleUriChanged(object sender, Uri uri) => _graphQLApiClient = CreateGraphQLAPIClient(uri);

        static IGraphQLAPI CreateGraphQLAPIClient(Uri uri) => RestService.For<IGraphQLAPI>(new HttpClient { BaseAddress = uri });
        #endregion
    }
}
