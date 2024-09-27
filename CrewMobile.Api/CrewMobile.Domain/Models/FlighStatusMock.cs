using CrewMobile.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace CrewMobile.Domain.Models
{
    public class FlighStatusMock : FlighStatusMockCom
    {
        [Key]
        public int FlighStatusMockId { get; set; }
    }
}
