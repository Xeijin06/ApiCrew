using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class Ns6FlightLegInfo
    {
        [JsonProperty("@FlightStatus")]
        public string FlightStatus { get; set; }

        [JsonProperty("ns6:DepartureAirport")]
        public Ns6Airport Ns6DepartureAirport { get; set; }

        [JsonProperty("ns6:ArrivalAirport")]
        public Ns6Airport Ns6ArrivalAirport { get; set; }

        [JsonProperty("ns6:MarketingAirline")]
        public Ns6MarketingAirline Ns6MarketingAirline { get; set; }

        [JsonProperty("ns6:Equipment")]
        public Ns6Equipment Ns6Equipment { get; set; }

        [JsonProperty("ns6:DepartureDateTime")]
        public Ns6DateTime Ns6DepartureDateTime { get; set; }

        [JsonProperty("ns6:ArrivalDateTime")]
        public Ns6DateTime Ns6ArrivalDateTime { get; set; }
    }
}