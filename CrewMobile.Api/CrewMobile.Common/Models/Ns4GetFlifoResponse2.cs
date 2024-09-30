using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class Ns4GetFlifoResponse2
    {
        [JsonProperty("ns6:OTA_AirFlifoRS")]
        public Ns6OtaAirFlifoRs2 Ns6OtaAirFlifoRs { get; set; }
    }
}
