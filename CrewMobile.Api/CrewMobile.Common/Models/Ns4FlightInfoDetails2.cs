using Newtonsoft.Json;
using System.Collections.Generic;

namespace CrewMobile.Common.Models
{
    public class Ns4FlightInfoDetails2
    {
        [JsonProperty("@FlightNumber")]
        public string FlightNumber { get; set; }

        [JsonProperty("ns4:FlightLegInfo")]
        public List<Ns4FlightLegInfo> Ns4FlightLegInfo { get; set; }
    }
}
