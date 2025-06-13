using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class SoapBody2
    {
        [JsonProperty("ns3:getFlifoResponse")]
        public Ns3GetFlifoResponse2 Ns3GetFlifoResponse { get; set; }
    }
}
