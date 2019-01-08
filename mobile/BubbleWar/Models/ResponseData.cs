using System.Collections.Generic;

using Newtonsoft.Json;

namespace BubbleWar
{
    class ResponseData
    {
        [JsonProperty("teams")]
        public List<TeamScore> Teams { get; set; }
    }
}
