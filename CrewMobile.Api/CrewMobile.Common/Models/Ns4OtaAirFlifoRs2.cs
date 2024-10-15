using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class Ns4OtaAirFlifoRs2
    {
        [JsonProperty("ns4:FlightInfoDetails")]
        public Ns4FlightInfoDetails2 Ns4FlightInfoDetails { get; set; }
    }
}
