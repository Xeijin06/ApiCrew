using System;
using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class Ns6DateTime
    {
        [JsonProperty("@Scheduled")]
        public DateTimeOffset Scheduled { get; set; }

        [JsonProperty("@Estimated")]
        public DateTimeOffset Estimated { get; set; }

        [JsonProperty("@Actual")]
        public DateTimeOffset Actual { get; set; }
    }
}