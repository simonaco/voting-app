using Newtonsoft.Json;

namespace VotingApp
{
    class GraphQLRequest
    {
        public GraphQLRequest(string query, string variables = null) => (Query, Variables) = (query, variables);

        [JsonProperty("query")]
        public string Query { get; }

        [JsonProperty("variables")]
        public string Variables { get; }
    }
}
