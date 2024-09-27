using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class SoapEnvelope2
    {
        [JsonProperty("soap:Body")]
        public SoapBody2 SoapBody { get; set; }
    }
}
