using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class Ns4Airport
    {
        [JsonProperty("@LocationCode")]
        public string LocationCode { get; set; }

        [JsonProperty("@Gate")]
        public string Gate { get; set; }
    }
}