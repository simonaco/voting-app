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
            var response = await PostObjectToAPI<QueryRequest, QueryResponse>(BubbleWarUrl, new QueryRequest()).ConfigureAwait(false);
            return response.Data.Teams;
        }

        public static async Task<TeamScore> VoteForTeamAndGetUpdatedScore(TeamColor teamType)
        {
            var response = await PostObjectToAPI<MutationRequest, MutationResponse>(BubbleWarUrl, new MutationRequest(teamType)).ConfigureAwait(false);
            return response.Data.TeamScore;
        }

        public static Task<HttpResponseMessage> VoteForTeam(TeamColor teamType) => PostObjectToAPI(BubbleWarUrl, new MutationRequest(teamType));
        #endregion

        #region Classes
        class QueryRequest
        {
            [JsonProperty("query")]
            public string Query => "{teams{id, name, points}}";
        }

        class QueryResponse
        {
            [JsonProperty("data")]
            public ResponseData Data { get; set; }
        }

        class ResponseData
        {
            [JsonProperty("teams")]
            public List<TeamScore> Teams { get; set; }
        }

        class MutationRequest
        {
            readonly int _teamNumber;

            public MutationRequest(TeamColor team) => _teamNumber = (int)team;

            [JsonProperty("query")]
            public string Query => "{incrementPoints(id:" + _teamNumber + "){name,points}}";
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
