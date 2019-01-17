using System.Collections.Generic;

using Newtonsoft.Json;

namespace VotingApp
{
    class TeamsQueryDataResponse
    {
        [JsonProperty("teams")]
        public List<TeamScore> Teams { get; set; }
    }
}
