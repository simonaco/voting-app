using Newtonsoft.Json;

namespace BubbleWar
{
    class QueryResponse : ErrorResponse
    {
        [JsonProperty("data")]
        public ResponseData Data { get; set; }
    }
}
