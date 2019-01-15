using Newtonsoft.Json;

namespace BubbleWar
{
    class IncrementPointsDataResponse
    {
        [JsonProperty("incrementPoints")]
        public TeamScore TeamScore { get; set; }
    }
}
