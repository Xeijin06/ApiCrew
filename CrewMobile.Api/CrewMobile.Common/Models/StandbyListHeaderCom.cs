using Newtonsoft.Json;

namespace CrewMobile.Common.Models
{
    public class StandbyListHeaderCom
    {

        [JsonConstructor]
        public StandbyListHeaderCom(string bookedPassengers)
        {
            BookedPassengers = bookedPassengers;
        }

        public string OriginAirport { get; set; }

        public string DestinationAirport { get; set; }

        public string FullDestinationDescription { get; set; }

        public string Gate { get; set; }

        public FlightCom MarketingAirline { get; set; }

        public FlightCom OperatingAirline { get; set; }

        public string FlightStatus { get; set; }

        public string CabinCapacity { get; set; }

        public string BookedPassengers { get; private set; }

        public int CheckedInPassengers { get; set; }

        public int SeatRemains { get; set; }

        public int PassengerListCount { get; set; }
    }
}