using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Refit;
using Polly;

namespace VotingApp
{
    static class GraphQLService
    {
        #region Constant Fields
        readonly static Lazy<IGraphQLAPI> _graphQLApiClientHolder = new Lazy<IGraphQLAPI>(() => RestService.For<IGraphQLAPI>(BubbleWarUrl));
        #endregion

        #region Properties
        static string BubbleWarUrl => GraphQLSettings.Uri.ToString();
        static IGraphQLAPI GraphQLApiClient => _graphQLApiClientHolder.Value;
        #endregion

        #region Methods
        public static async Task<List<TeamScore>> GetTeamScoreList()
        {
            const string requestString = "query{teams{name, points}}";

            var data = await ExecuteGraphQLFunction(() => GraphQLApiClient.TeamsQuery(new GraphQLRequest(requestString))).ConfigureAwait(false);

            return data.Teams;
        }

        public static async Task<TeamScore> VoteForTeamAndGetCurrentScore(TeamColor teamType)
        {
            var request = "mutation {incrementPoints(id:" + (int)teamType + ") {name, points}}";

            var data = await ExecuteGraphQLFunction(() => GraphQLApiClient.IncrementPoints(new GraphQLRequest(request))).ConfigureAwait(false);

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
        #endregion
    }
}
