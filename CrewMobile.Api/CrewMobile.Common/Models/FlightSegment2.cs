namespace CrewMobile.Common.Models
{
    public class FlightSegment2
    {
        public AirlineMarketing AirlineMarketing { get; set; }

        public string FlightNumber { get; set; }

        public DateTimeDepart DateTimeDepart { get; set; }

        public Location LocationDepart { get; set; }

        public Location LocationArrive { get; set; }

        public Equipment Equipment { get; set; }
    }
}