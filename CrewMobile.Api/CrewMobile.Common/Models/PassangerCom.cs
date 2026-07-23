using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CrewMobile.Common.Models
{
    public class PassangerCom
    {

        [Display(Name = "Confirmation ID")]
        public string? ConfirmationID { get; set; }

        public string? Surname { get; set; }

        [Display(Name = "Given Name")]
        public string? GivenName { get; set; }

        [Display(Name = "Final Passenger Destination")]
        public string? FinalPassengerDestination { get; set; }

        [Display(Name = "Cabin Class")]
        public string? CabinClass { get; set; }

        [Display(Name = "Final Passenger Destination")]
        public bool IsFullFare { get; set; }

        [Display(Name = "Is Discounted Fare")]
        public bool IsDiscountedFare { get; set; }

        [Display(Name = "Fare Kind")]
        public string? FareKind { get; set; }

        [Display(Name = "Seat Leg")]
        public int SeatLeg { get; set; }

        [Display(Name = "Booking Class")]
        public string? BookingClass { get; set; }

        public string? Seat { get; set; }

        public string? Treatment { get; set; }
    }
}