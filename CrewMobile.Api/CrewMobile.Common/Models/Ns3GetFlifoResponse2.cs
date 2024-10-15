using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class Ns3GetFlifoResponse2
    {
        [JsonProperty("ns4:OTA_AirFlifoRS")]
        public Ns4OtaAirFlifoRs2 Ns4OtaAirFlifoRs { get; set; }
    }
}
