using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class FlightIrrop
    {
        [JsonProperty(PropertyName = "FlightNumber")]
        public string FlightNum { get; set; }

        [JsonProperty(PropertyName = "DepartureStation")]
        public string Origin { get; set; }

        [JsonProperty(PropertyName = "ArrivalStation")]
        public string Destination { get; set; }

        [JsonProperty(PropertyName = "FlightDate")]
        public string DepartureDate { get; set; }

        [JsonProperty(PropertyName = "IrropType")]
        public string IrropDescription { get; set; }
    }
}