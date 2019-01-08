using System.Collections.Generic;
using Newtonsoft.Json;

namespace BubbleWar
{
    abstract class ErrorResponse
    {
        [JsonProperty("errors")]
        public List<Error> Errors { get; set; }
    }

    class Error
    {
        [JsonProperty("message")]
        public string Message { get; }

        [JsonProperty("locations")]
        public List<ErrorLocations> Locations { get; set; }
    }

    class ErrorLocations
    {
        [JsonProperty("line")]
        public long Line { get; set; }

        [JsonProperty("column")]
        public long Column { get; set; }
    }
}
