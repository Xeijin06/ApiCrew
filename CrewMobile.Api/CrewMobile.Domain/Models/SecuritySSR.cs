using CrewMobile.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace CrewMobile.Domain.Models
{
    public class SecuritySSR : SecuritySSRCom
    {
        [Key]
        public int SecuritySSId { get; set; }
    }
}