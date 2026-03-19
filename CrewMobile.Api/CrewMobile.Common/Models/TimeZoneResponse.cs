using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class TimeZoneResponse
    {
        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("IdTransaction")]
        public string IdTransaction { get; set; }

        [JsonProperty("Result")]
        public TimeZoneInformationRs Result { get; set; }
    }
}
