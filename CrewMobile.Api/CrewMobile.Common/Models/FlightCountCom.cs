using System.Collections.Generic;

namespace CrewMobile.Common.Models
{
    public class FlightCountCom
    {
        public bool Isfinal { get; set; }

        public int TotalPassengers { get; set; }

        public List<CountCom> Counts { get; set; }
    }
}