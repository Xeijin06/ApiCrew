using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class FlightDetailSoapCom2
    {
        [JsonProperty("soap:Envelope")]
        public SoapEnvelope2 SoapEnvelope { get; set; }
    }
}
