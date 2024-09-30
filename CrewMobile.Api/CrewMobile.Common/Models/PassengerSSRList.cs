using Newtonsoft.Json;
using System.Collections.Generic;

namespace CrewMobile.Common.Models
{
    public class PassengerSSRList
    {
        [JsonConstructor]
        public PassengerSSRList(string finalPassengerDestination)
        {
            FinalPassengerDestination = finalPassengerDestination;
        }
        public string Surname { get; set; }

        public string GivenName { get; set; }

        public string FinalPassengerDestination { get; private set; }

        public string CabinClass { get; set; }

        public bool IsFullFare { get; set; }

        public bool IsDiscountedFare { get; set; }

        public int LegPosition { get; set; }

        public string BookingClass { get; set; }

        public string Seat { get; set; }

        public List<SSRCode> SSRCodes { get; set; }
    }
}