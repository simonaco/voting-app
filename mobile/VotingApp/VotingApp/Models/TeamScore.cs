using Newtonsoft.Json;

namespace VotingApp
{
    class TeamScore
    {
        [JsonProperty("name")]
        public TeamColor Color { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }
    }
}
