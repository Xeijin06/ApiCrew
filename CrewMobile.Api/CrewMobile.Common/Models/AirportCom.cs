using System.ComponentModel.DataAnnotations;

namespace CrewMobile.Common.Models
{
    public class AirportCom
    {
        [Display(Name = "Airport Code")]
        [Required(ErrorMessage = "You must enter an {0}")]
        [StringLength(3, ErrorMessage = "The field {0} must contain {1} characters", MinimumLength = 3)]
        public string AirportCode { get; set; }

        [Display(Name = "Airport Name")]
        [Required(ErrorMessage = "You must enter an {0}")]
        [StringLength(50, ErrorMessage = "The field {0} must contain maximum {1} characters")]
        public string AirportName { get; set; }

        [Display(Name = "Airport Abbreviation")]
        [Required(ErrorMessage = "You must enter an {0}")]
        [StringLength(20, ErrorMessage = "The field {0} must contain maximum {1} characters")]
        public string AirportAbbreviation { get; set; }

        [Display(Name = "Country Code")]
        [Required(ErrorMessage = "You must enter an {0}")]
        [StringLength(2, ErrorMessage = "The field {0} must contain {1} characters", MinimumLength = 2)]
        public string CountryCode { get; set; }

        [Display(Name = "Time Zone")]
        [StringLength(50, ErrorMessage = "The field {0} must contain maximum {1} characters")]
        public string TimeZone { get; set; }

        [Display(Name = "GTM Offset")]
        public int GTMOffset { get; set; }
    }
}
