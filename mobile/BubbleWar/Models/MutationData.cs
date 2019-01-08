using Newtonsoft.Json;

namespace BubbleWar
{
    class MutationData
    {
        [JsonProperty("incrementPoints")]
        public TeamScore TeamScore { get; set; }
    }
}
