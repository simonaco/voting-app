using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Refit;
using Polly;

namespace BubbleWar
{
    public abstract class GraphQLService
    {
        #region Constant Fields
        readonly static IGraphQLAPI _graphQLApiClient = RestService.For<IGraphQLAPI>(BubbleWarUrl);
        #endregion

        #region Properties
        static string BubbleWarUrl => GraphQLSettings.Uri.ToString();
        #endregion

        #region Methods
        public static async Task<List<TeamScore>> GetTeamScoreList()
        {
            var response = await ExecutePollyFunction(() => _graphQLApiClient.Query(new QueryRequest())).ConfigureAwait(false);

            if (response.Errors != null)
                throw new AggregateException(response.Errors.Select(x => new Exception(x.Message)));

            return response.Data.Teams;
        }

        public static async Task<TeamScore> VoteForTeamAndGetCurrentScore(TeamColor teamType)
        {
            var response = await ExecutePollyFunction(() => _graphQLApiClient.Mutation(new MutationRequest(teamType))).ConfigureAwait(false);

            if (response.Errors != null)
                throw new AggregateException(response.Errors.Select(x => new Exception(x.Message)));

            return response.Data.TeamScore;
        }

        public static Task VoteForTeam(TeamColor teamType) => VoteForTeamAndGetCurrentScore(teamType);

        static Task<T> ExecutePollyFunction<T>(Func<Task<T>> action, int numRetries = 3)
        {
            return Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync
                    (
                        numRetries,
                        pollyRetryAttempt
                    ).ExecuteAsync(action);

            TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
        }
        #endregion
    }
}
