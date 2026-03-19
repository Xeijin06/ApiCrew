using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class TimeZoneDetail
    {
        [JsonProperty("BeginInstantOffsetChange")]
        public DateTime BeginInstantOffsetChange { get; set; }

        [JsonProperty("EndInstantOffsetChange")]
        public DateTime EndInstantOffsetChange { get; set; }

        [JsonProperty("Offset")]
        public string Offset { get; set; }

        [JsonProperty("ZoneName")]
        public string ZoneName { get; set; }

        [JsonProperty("IdTimeZone")]
        public string IdTimeZone { get; set; }

        [JsonProperty("DisplayZoneName")]
        public string DisplayZoneName { get; set; }
    }
}
