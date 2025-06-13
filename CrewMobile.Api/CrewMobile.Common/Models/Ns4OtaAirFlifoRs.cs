using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class Ns4OtaAirFlifoRs
    {
        [JsonProperty("FlightInfoDetails")]
        public Ns4FlightInfoDetails Ns4FlightInfoDetails { get; set; }
    }
}