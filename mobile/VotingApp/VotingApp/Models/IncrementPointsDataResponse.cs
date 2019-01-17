using Newtonsoft.Json;

namespace VotingApp
{
    class IncrementPointsDataResponse
    {
        [JsonProperty("incrementPoints")]
        public TeamScore TeamScore { get; set; }
    }
}
