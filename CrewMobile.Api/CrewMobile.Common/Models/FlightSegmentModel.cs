using System.Collections.Generic;

namespace CrewMobile.Common.Models
{
    public class FlightSegmentModel
    {
        public FlightSegment2 FlightSegment { get; set; }

        public List<EquipmentRow> EquipmentRows { get; set; }
    }
}