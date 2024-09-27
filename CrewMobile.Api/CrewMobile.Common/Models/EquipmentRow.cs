using System.Collections.Generic;

namespace CrewMobile.Common.Models
{
    public class EquipmentRow
    {
        public int RowNumber { get; set; }

        public List<AttributeSeat> Attributes { get; set; }

        public List<Seat> Seats { get; set; }
    }
}
