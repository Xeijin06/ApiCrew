using Newtonsoft.Json;
using System.Collections.Generic;

namespace CrewMobile.Common.Models
{
    public class FlightListHeaderCom
    {
        [JsonProperty("Flight")]
        public List<FlightListCom> Flights { get; set; }
    }
}
