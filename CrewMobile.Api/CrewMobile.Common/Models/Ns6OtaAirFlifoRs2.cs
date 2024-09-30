using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class Ns6OtaAirFlifoRs2
    {
        [JsonProperty("ns6:FlightInfoDetails")]
        public Ns6FlightInfoDetails2 Ns6FlightInfoDetails { get; set; }
    }
}
