namespace CrewMobile.Common.Models
{
    using Newtonsoft.Json;

    public class SoapBody
    {
        [JsonProperty("ns4:getFlifoResponse")]
        public Ns4GetFlifoResponse Ns4GetFlifoResponse { get; set; }
    }
}