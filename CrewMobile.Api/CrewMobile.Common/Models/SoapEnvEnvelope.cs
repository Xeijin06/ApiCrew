using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class SoapEnvEnvelope
    {
        [JsonProperty("SOAP-ENV:Body")]
        public SoapEnvBody SoapEnvBody { get; set; }
    }
}