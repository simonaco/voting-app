using Newtonsoft.Json;

namespace BubbleWar
{
    class MutationRequest
    {
        readonly int _teamNumber;

        public MutationRequest(TeamColor team) => _teamNumber = (int)team;

        [JsonProperty("query")]
        public string Query => "mutation{incrementPoints(id:" + _teamNumber + ") {id, name, points}}";
    }
}
