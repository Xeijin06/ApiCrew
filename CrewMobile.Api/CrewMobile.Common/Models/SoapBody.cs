namespace CrewMobile.Common.Models
{
    using Newtonsoft.Json;

    public class SoapBody
    {
        [JsonProperty("ns3:getFlifoResponse")]
        public Ns3GetFlifoResponse Ns3GetFlifoResponse { get; set; }
    }
}