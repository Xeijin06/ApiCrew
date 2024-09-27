using CrewMobile.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace CrewMobile.Domain.Models
{
    public class Airport : AirportCom
    {
        [Key]
        public int AirportId { get; set; }
    }
}