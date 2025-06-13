using Newtonsoft.Json;
using System;

namespace CrewMobile.Api.Models
{
    public class FlightResponse
    {
        public string Abbreviation { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public DateTime Date { get; set; }

        [JsonIgnore]
        public DateTime DateEstimated { get; set; }
    }
}