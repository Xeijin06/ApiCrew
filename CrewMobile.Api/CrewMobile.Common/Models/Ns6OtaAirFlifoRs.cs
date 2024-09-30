using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class Ns6OtaAirFlifoRs
    {
        [JsonProperty("ns6:FlightInfoDetails")]
        public Ns6FlightInfoDetails Ns6FlightInfoDetails { get; set; }
    }
}