using System;
using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class Ns1TimeZone
    {
        [JsonProperty("@ZoneName")]
        public string ZoneName { get; set; }

        [JsonProperty("@Offset")]
        public string Offset { get; set; }

        [JsonProperty("@BeginInstantOffsetChange")]
        public DateTimeOffset? BeginInstantOffsetChange { get; set; }

        [JsonProperty("@EndInstantOffsetChange", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? EndInstantOffsetChange { get; set; }

        [JsonProperty("ns1:TimeZoneID")]
        public string Ns1TimeZoneId { get; set; }

        [JsonProperty("ns1:DisplayZoneName")]
        public string Ns1DisplayZoneName { get; set; }
    }
}