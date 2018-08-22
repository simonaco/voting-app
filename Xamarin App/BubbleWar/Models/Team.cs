using System.Collections.Generic;

using Newtonsoft.Json;

namespace BubbleWar
{
    public class TeamsResponse
    {
        [JsonProperty("data")]
        public TeamsData Data { get; set; }
    }

    public class TeamsData
    {
        [JsonProperty("teams")]
        public List<Team> Teams { get; set; }
    }

    public class Team
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }
    }
}
