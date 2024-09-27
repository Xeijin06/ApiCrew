using System.ComponentModel.DataAnnotations;
using CrewMobile.Common.Models;

namespace CrewMobile.Domain.Models
{
    public class CMParameter : ParameterCom
    {
        [Key]
        public int ParameterId { get; set; }
    }
}
