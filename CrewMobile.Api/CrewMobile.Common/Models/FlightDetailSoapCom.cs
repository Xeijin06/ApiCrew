using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class FlightDetailSoapCom
    {
        [JsonProperty("soap:Envelope")]
        public SoapEnvelope SoapEnvelope { get; set; }
    }
}
