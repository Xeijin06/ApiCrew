using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class Ns4FlightLegInfo
    {
        [JsonProperty("@FlightStatus")]
        public string FlightStatus { get; set; }

        [JsonProperty("ns4:DepartureAirport")]
        public Ns4Airport Ns4DepartureAirport { get; set; }

        [JsonProperty("ns4:ArrivalAirport")]
        public Ns4Airport Ns4ArrivalAirport { get; set; }

        [JsonProperty("ns4:MarketingAirline")]
        public Ns4MarketingAirline Ns4MarketingAirline { get; set; }

        [JsonProperty("ns4:Equipment")]
        public Ns4Equipment Ns4Equipment { get; set; }

        [JsonProperty("ns4:DepartureDateTime")]
        public Ns4DateTime Ns4DepartureDateTime { get; set; }

        [JsonProperty("ns4:ArrivalDateTime")]
        public Ns4DateTime Ns4ArrivalDateTime { get; set; }
    }
}