using Newtonsoft.Json;
using System.Collections.Generic;

namespace CrewMobile.Common.Models
{
    public class FlightDetailHeaderCom
    {
        [JsonProperty("Flight")]
        public List<FlightDetailCom> Flights { get; set; }
    }
}