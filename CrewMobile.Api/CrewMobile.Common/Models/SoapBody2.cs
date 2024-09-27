using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class SoapBody2
    {
        [JsonProperty("ns4:getFlifoResponse")]
        public Ns4GetFlifoResponse2 Ns4GetFlifoResponse { get; set; }
    }
}
