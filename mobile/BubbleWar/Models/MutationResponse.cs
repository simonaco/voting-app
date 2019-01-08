using Newtonsoft.Json;

namespace BubbleWar
{
    class MutationResponse : ErrorResponse
    {
        [JsonProperty("data")]
        public MutationData Data { get; set; }
    }
}
