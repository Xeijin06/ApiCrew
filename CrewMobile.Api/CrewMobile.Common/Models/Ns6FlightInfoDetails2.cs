using Newtonsoft.Json;
using System.Collections.Generic;

namespace CrewMobile.Common.Models
{
    public class Ns6FlightInfoDetails2
    {
        [JsonProperty("@FlightNumber")]
        public string FlightNumber { get; set; }

        [JsonProperty("ns6:FlightLegInfo")]
        public List<Ns6FlightLegInfo> Ns6FlightLegInfo { get; set; }
    }
}
