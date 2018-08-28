using Newtonsoft.Json;

namespace BubbleWar
{
    public class TeamScore
    {
        [JsonProperty("name")]
        public TeamColor Color { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }
    }
}
