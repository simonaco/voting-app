using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace BubbleWar
{
    public abstract class GraphQLService : BaseGraphQLService
    {
        #region Properties
        static string BubbleWarUrl => GraphQLSettings.Uri.ToString();
        #endregion

        #region Methods
        public static async Task<List<TeamScore>> GetTeamScoreList()
        {
            var query = @"
                query {
                    teams {
                        name
                        points
                    }
                }";

            var response = await PostObjectToAPI<QueryResponse>(BubbleWarUrl, query).ConfigureAwait(false);
            return response.Data.Teams;
        }

        public static async Task<TeamScore> VoteForTeamAndGetUpdatedScore(TeamColor teamType)
        {
            var mutation = @"
                mutation {
                    incrementPoints(id:" + (int)teamType + @") {
                        name
                        points
                    }
                }";

            var response = await PostObjectToAPI<MutationResponse>(BubbleWarUrl, mutation).ConfigureAwait(false);
            return response.Data.TeamScore;
        }

        public static Task<HttpResponseMessage> VoteForTeam(TeamColor teamType)
        {
            var mutation = @"
                mutation {
                    incrementPoints(id:" + (int)teamType + @") {
                    }
                }";

            return PostObjectToAPI(BubbleWarUrl, mutation);
        }
        #endregion

        #region Classes
        class QueryResponse
        {
            [JsonProperty("data")]
            public QueryData Data { get; set; }
        }

        class QueryData
        {
            [JsonProperty("teams")]
            public List<TeamScore> Teams { get; set; }
        }
        
        class MutationResponse
        {
            [JsonProperty("data")]
            public MutationData Data { get; set; }
        }

        class MutationData
        {
            [JsonProperty("incrementPoints")]
            public TeamScore TeamScore { get; set; }
        }
        #endregion
    }
}
