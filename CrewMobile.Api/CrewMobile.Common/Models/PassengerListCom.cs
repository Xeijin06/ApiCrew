using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrewMobile.Common.Models
{
    public class PassengerListCom
    {
        [JsonConstructor]
        public PassengerListCom(string finalPassengerDestination)
        {
            FinalPassengerDestination = finalPassengerDestination;
        }

        public string Surname { get; set; }

        public string GivenName { get; set; }

        public string FinalPassengerDestination { get; set; }

        public string CabinClass { get; set; }

        public bool IsFullFare { get; set; }

        public bool IsDiscountedFare { get; set; }

        public string FareKind { get; set; }

        public int SeatLeg { get; set; }

        public string BookingClass { get; set; }
        public string BookingDescription { get; set; }

        public string Seat { get; set; }

        public string NonRevenueCategory { get; set; }

        public string Treatment { get; set; }

        [StringLength(6)]
        public string ConfirmationID { get; set; }

        public List<LoyaltyProgramCom> LoyaltyPrograms { get; set; }

        public List<SSRCode> SSRCodes { get; set; }

        public IrregularOperation IrreguarOperation { get; set; }
    }
}