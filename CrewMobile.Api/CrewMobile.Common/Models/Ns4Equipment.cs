using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class Ns4Equipment
    {
        [JsonProperty("@AircraftTailNumber")]
        public string AircraftTailNumber { get; set; }

        [JsonProperty("@AirEquipType")]
        public string AirEquipType { get; set; }
    }
}