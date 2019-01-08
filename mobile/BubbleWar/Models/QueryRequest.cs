using Newtonsoft.Json;

namespace BubbleWar
{
    class QueryRequest
    {
        [JsonProperty("query")]
        public string Query => "{teams{id, name, points}}";
    }
}
