using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class TimeZoneSoapCom
    {
        [JsonProperty("SOAP-ENV:Envelope")]
        public SoapEnvEnvelope SoapEnvEnvelope { get; set; }
    }
}
