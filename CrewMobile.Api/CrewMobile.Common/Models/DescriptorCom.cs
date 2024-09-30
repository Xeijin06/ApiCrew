using System.ComponentModel.DataAnnotations;

namespace CrewMobile.Common.Models
{
    public class DescriptorCom
    {
        [Required(ErrorMessage = "You must enter an {0}")]
        [StringLength(50, ErrorMessage = "The field {0} must contain {1} characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must enter an {0}")]
        [StringLength(20, ErrorMessage = "The field {0} must contain maximum {1} characters")]
        public string Abbreviation { get; set; }

        public int Order { get; set; }
    }
}