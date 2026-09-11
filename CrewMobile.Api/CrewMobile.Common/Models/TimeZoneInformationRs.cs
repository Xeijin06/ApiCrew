using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class TimeZoneInformationRs
    {
        [JsonProperty("TimeZoneInformation")]
        public List<LocationTimeZoneInformation> TimeZoneInformation { get; set; }
    }
}
