using System.Collections.Generic;

namespace CrewMobile.Common.Models
{
    public class Seat
    {
        public int RowNumber { get; set; }

        public string Position { get; set; }

        public List<AttributeSeat2> Attributes { get; set; }

        public string SeatLocation { get; set; }

        public Status Status { get; set; }

        public string Key { get; set; }
    }
}