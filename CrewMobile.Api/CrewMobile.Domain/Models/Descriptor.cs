using System.ComponentModel.DataAnnotations;
using CrewMobile.Common.Models;

namespace CrewMobile.Domain.Models
{
    public class Descriptor : DescriptorCom
    {
        [Key]
        public int DescriptorId { get; set; }
    }
}