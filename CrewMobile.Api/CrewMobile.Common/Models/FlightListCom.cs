using Newtonsoft.Json;
using System;

namespace CrewMobile.Common.Models
{
    public class FlightListCom
    {
        [JsonProperty("scheduled_departure")]
        public string ScheduledDeparture { get; set; }

        [JsonProperty("flight_number")]
        public string FlightNumber { get; set; }

        [JsonProperty("airline")]
        public string Airline { get; set; }

        [JsonProperty("scheduled_departure_dttm")]
        public string ScheduledDepartureDttm { get; set; }

        [JsonProperty("origin_airport")]
        public string OriginAirport { get; set; }

        [JsonProperty("destination_airport")]
        public string DestinationAirport { get; set; }

        [JsonProperty("scheduled_arrive_dttm")]
        public string ScheduledArriveDttm { get; set; }

        [JsonProperty("leg_sequence")]
        public int LegSequence { get; set; }

        [JsonProperty("business_seats")]
        public int BusinessSeats { get; set; }

        [JsonProperty("economy_seats")]
        public int EconomySeats { get; set; }

        [JsonProperty("fleetType")]
        public string FleetType { get; set; }
    }
}
