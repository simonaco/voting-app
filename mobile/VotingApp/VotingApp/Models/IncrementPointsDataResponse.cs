using Newtonsoft.Json;

namespace VotingApp
{
    class IncrementPointsDataResponse
    {
        public IncrementPointsDataResponse(TeamScore teamScore) => TeamScore = teamScore;

        [JsonProperty("incrementPoints")]
        public TeamScore TeamScore { get; }
    }
}
