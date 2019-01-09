using System.Collections.Generic;

using Newtonsoft.Json;

namespace BubbleWar
{
    class TeamsQueryDataResponse
    {
        [JsonProperty("teams")]
        public List<TeamScore> Teams { get; set; }
    }
}
