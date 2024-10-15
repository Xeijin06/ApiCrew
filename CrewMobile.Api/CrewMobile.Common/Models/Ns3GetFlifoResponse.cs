using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class Ns3GetFlifoResponse
    {
        [JsonProperty("ns4:OTA_AirFlifoRS")]
        public Ns4OtaAirFlifoRs Ns4OtaAirFlifoRs { get; set; }
    }
}