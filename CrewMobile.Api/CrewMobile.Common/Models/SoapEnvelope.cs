using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class SoapEnvelope
    {
        [JsonProperty("soap:Body")]
        public SoapBody SoapBody { get; set; }
    }
}