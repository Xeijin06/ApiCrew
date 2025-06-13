using System.ComponentModel.DataAnnotations;
using CrewMobile.Common.Models;

namespace CrewMobile.Domain.Models
{
    public class Group : GroupCom
    {
        [Key]
        public int GroupId { get; set; }
    }
}