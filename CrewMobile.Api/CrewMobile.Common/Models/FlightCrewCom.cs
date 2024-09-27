using System;
using System.ComponentModel.DataAnnotations;

namespace CrewMobile.Common.Models
{
    public class FlightCrewCom
    {
        [Display(Name = "Flight Number")]
        public int FlightNumber { get; set; }

        public string Company { get; set; }

        [Display(Name = "Date Start")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateStart { get; set; }

        [Display(Name = "Date End")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateEnd { get; set; }

        public string Source { get; set; }

        public string Destination { get; set; }

        [Display(Name = "Crew Roll")]
        public string CrewRoll { get; set; }

        [Display(Name = "Employee Id")]
        public int CrewId { get; set; }

        [Display(Name = "Crew Name")]
        public string CrewName { get; set; }
    }
}