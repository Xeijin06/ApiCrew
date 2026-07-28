using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CrewMobile.Common.Models
{
    public class AirportsResponse
    {
        [JsonPropertyName("Airports")]
        public List<AirportInfo> Airports { get; set; } = new List<AirportInfo>();
    }
}
