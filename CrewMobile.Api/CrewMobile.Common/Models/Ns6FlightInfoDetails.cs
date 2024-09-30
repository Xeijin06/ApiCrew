using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class Ns6FlightInfoDetails
    {
        [JsonProperty("@FlightNumber")]
        public string FlightNumber { get; set; }

        [JsonProperty("ns6:FlightLegInfo")]
        public Ns6FlightLegInfo Ns6FlightLegInfo { get; set; }
    }
}