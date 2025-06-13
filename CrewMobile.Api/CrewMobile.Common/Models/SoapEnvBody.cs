using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class SoapEnvBody
    {
        [JsonProperty("ns0:COPA_TimeZoneInformationRS")]
        public Ns0CopaTimeZoneInformationRs Ns0CopaTimeZoneInformationRs { get; set; }
    }
}