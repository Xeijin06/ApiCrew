using CrewMobile.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace CrewMobile.Domain.Models
{    public class Employee : EmployeeCom
    {
        [Key]
        public int EmployeeId { get; set; }
    }
}
