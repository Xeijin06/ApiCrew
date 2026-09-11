using Newtonsoft.Json;
using System;

namespace CrewMobile.Common.Models
{
    public class FlightDetailCom
    {
        //TODO: Revisar la necesidad de los JsonProperty comentados, si no son necesarios eliminarlos por el cambio de endpoint.
        //[JsonProperty("scheduled_departure")]
        [JsonProperty("FlightDate")]
        public string ScheduledDeparture { get; set; }

        //[JsonProperty("flight_number")]
        [JsonProperty("FlightNumber")]
        public string FlightNumber { get; set; }

        //[JsonProperty("airline")]
        [JsonProperty("AcOwner")]
        public string Airline { get; set; }

        //[JsonProperty("scheduled_departure_dttm")]
        [JsonProperty("SchedDepart")]
        public string ScheduledDepartureDttm { get; set; }

        //[JsonProperty("origin_airport")]
        [JsonProperty("DepartureStation")]
        public string OriginAirport { get; set; }

        //[JsonProperty("destination_airport")]
        [JsonProperty("ArrivalStation")]
        public string DestinationAirport { get; set; }

        //[JsonProperty("scheduled_arrive_dttm")]
        [JsonProperty("SchedArrive")]
        public string ScheduledArriveDttm { get; set; }

        /*[JsonProperty("leg_sequence")]
        public int LegSequence { get; set; }

        [JsonProperty("business_seats")]
        public int BusinessSeats { get; set; }

        [JsonProperty("economy_seats")]
        public int EconomySeats { get; set; }*/

        //[JsonProperty("fleetType")]
        [JsonProperty("AcSubtypeIATA")]
        public string FleetType { get; set; }

        [JsonProperty("AirlineDesignator")]
        public string AirlineDesignator { get; set; }

        [JsonProperty("AircraftRegistrationID")]
        public string? AircraftRegistrationID { get; set; }

        public DateTime SourceDate { get; set; }

        public DateTime DestinationDate { get; set; }
    }
}