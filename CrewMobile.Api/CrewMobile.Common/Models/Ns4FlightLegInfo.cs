using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class Ns4FlightLegInfo
    {
        [JsonProperty("@FlightStatus")]
        public string FlightStatus { get; set; }

        [JsonProperty("DepartureAirport")]
        public Ns4Airport Ns4DepartureAirport { get; set; }

        [JsonProperty("ArrivalAirport")]
        public Ns4Airport Ns4ArrivalAirport { get; set; }

        [JsonProperty("MarketingAirline")]
        public Ns4MarketingAirline Ns4MarketingAirline { get; set; }

        [JsonProperty("Equipment")]
        public Ns4Equipment Ns4Equipment { get; set; }

        [JsonProperty("DepartureDateTime")]
        public Ns4DateTime Ns4DepartureDateTime { get; set; }

        [JsonProperty("ArrivalDateTime")]
        public Ns4DateTime Ns4ArrivalDateTime { get; set; }
    }
}