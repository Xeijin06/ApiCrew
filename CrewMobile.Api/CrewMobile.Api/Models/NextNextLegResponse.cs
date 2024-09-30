namespace CrewMobile.Api.Models
{
    public class NextNextLegResponse
    {
        public FlightResponse Source { get; set; }

        public FlightResponse Destination { get; set; }

        public string Airline { get; set; }

        public string FlightNumber { get; set; }

        public int Passangers { get; set; }

        public string Status { get; set; }

        public int BussinessClass { get; set; }

        public int SSR { get; set; }

        public int Preferred { get; set; }

        public int Meals { get; set; }
    }
}