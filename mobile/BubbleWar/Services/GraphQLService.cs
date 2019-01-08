using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Linq;
using System;

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

            if (response.Errors != null)
                throw new AggregateException(response.Errors.Select(x => new Exception(x.Message)));

            return response.Data.Teams;
        }

        public static async Task<TeamScore> VoteForTeamAndGetCurrentScore(TeamColor teamType)
        {
            var response = await PostObjectToAPI<MutationRequest, MutationResponse>(BubbleWarUrl, new MutationRequest(teamType)).ConfigureAwait(false);

            if (response.Errors != null)
                throw new AggregateException(response.Errors.Select(x => new Exception(x.Message)));

            return response.Data.TeamScore;
        }

        public static Task VoteForTeam(TeamColor teamType) => VoteForTeamAndGetCurrentScore(teamType);
        #endregion

        #region Classes
        class QueryRequest
        {
            [JsonProperty("query")]
            public string Query => "{teams{id, name, points}}";
        }

        class QueryResponse : ErrorResponse
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
            public string Query => "mutation{incrementPoints(id:" + _teamNumber + ") {id, name, points}}";
        }

        class MutationResponse : ErrorResponse
        {
            [JsonProperty("data")]
            public MutationData Data { get; set; }
        }

        class MutationData
        {
            [JsonProperty("incrementPoints")]
            public TeamScore TeamScore { get; set; }
        }

        abstract class ErrorResponse
        {
            [JsonProperty("errors")]
            public List<Error> Errors { get; set; }
        }

        class Error
        {
            [JsonProperty("message")]
            public string Message { get; }

            [JsonProperty("locations")]
            public List<ErrorLocations> Locations { get; set; }
        }

        class ErrorLocations
        {
            [JsonProperty("line")]
            public long Line { get; set; }

            [JsonProperty("column")]
            public long Column { get; set; }
        }
        #endregion
    }
}
