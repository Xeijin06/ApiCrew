using System;
using System.ComponentModel.DataAnnotations;

namespace CrewMobile.Common.Models
{
    public class IrregularOperationCom
    {
        [Required]
        [Display(Name = "Flight Number")]
        public string FlightNumber { get; set; }

        [Required]
        public string Origin { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Departure Date")]
        public string DepartureDate { get; set; }
    }
}