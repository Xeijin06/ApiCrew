using CrewMobile.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace CrewMobile.Domain.Models
{
    public class FlightCrew : FlightCrewCom
    {
        [Key]
        public int FlightCrewId { get; set; }
    }
}