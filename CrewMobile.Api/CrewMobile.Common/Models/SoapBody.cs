namespace CrewMobile.Common.Models
{
    using Newtonsoft.Json;

    public class SoapBody
    {
        [JsonProperty("getFlifoResponse")]
        public Ns3GetFlifoResponse Ns3GetFlifoResponse { get; set; }
    }
}