using System;
using System.ComponentModel.DataAnnotations;

namespace CrewMobile.Common.Models
{

    public class FlighStatusMockCom
    {
        [Required]
        [Display(Name = "Fligh Number")]
        public int FlighNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Status (ON-TIME, DELAYED, CANCELLED)")]
        public string Status { get; set; }
    }
}