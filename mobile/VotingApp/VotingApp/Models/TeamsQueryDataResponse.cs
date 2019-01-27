using System.Collections.Generic;

using Newtonsoft.Json;

namespace VotingApp
{
    class TeamsQueryDataResponse
    {
        public TeamsQueryDataResponse(List<TeamScore> teams) => Teams = teams;

        [JsonProperty("teams")]
        public List<TeamScore> Teams { get; }
    }
}
