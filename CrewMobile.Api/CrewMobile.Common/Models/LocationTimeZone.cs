namespace CrewMobile.Common.Models
{
    using Newtonsoft.Json;

    public class LocationTimeZone
    {
        [JsonProperty("@xmlns:ns1")]
        public string XmlnsNs1 { get; set; }

        [JsonProperty("@LocationCode")]
        public string LocationCode { get; set; }

        [JsonProperty("@LocationCategoryCode")]
        public string LocationCategoryCode { get; set; }

        [JsonProperty("ns1:TimeZone")]
        public Ns1TimeZone Ns1TimeZone { get; set; }
    }
}