using Newtonsoft.Json;
using System.Collections.Generic;

namespace CrewMobile.Common.Models
{
    public class Ns0TimeZones
    {
        [JsonProperty("Location")]
        public List<LocationTimeZone> Locations { get; set; }
    }
}