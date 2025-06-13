using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class Ns4FlightInfoDetails
    {
        [JsonProperty("@FlightNumber")]
        public string FlightNumber { get; set; }

        [JsonProperty("FlightLegInfo")]
        public Ns4FlightLegInfo Ns4FlightLegInfo { get; set; }
    }
}