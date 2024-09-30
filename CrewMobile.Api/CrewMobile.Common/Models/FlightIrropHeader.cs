using Newtonsoft.Json;
using System.Collections.Generic;

namespace CrewMobile.Common.Models
{
    public class FlightIrropHeader
    {
        [JsonProperty(PropertyName = "Irrops")]
        public List<FlightIrrop> FlightIrrops { get; set; }
    }
}