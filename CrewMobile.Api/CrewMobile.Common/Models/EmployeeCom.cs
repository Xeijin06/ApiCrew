using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CrewMobile.Common.Models
{
    public class EmployeeCom
    {
        [Required(ErrorMessage = "You must enter an {0}")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Employee Id")]
        [Required(ErrorMessage = "You must enter an {0}")]
        public int Code { get; set; }
    }
}
