using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class LocationTimeZoneInformation
    {
        [JsonProperty("LocationCode")]
        public string LocationCode { get; set; }

        [JsonProperty("LocationCategoryCode")]
        public string LocationCategoryCode { get; set; }

        [JsonProperty("TimeZone")]
        public List<TimeZoneDetail> TimeZone { get; set; }
    }
}
