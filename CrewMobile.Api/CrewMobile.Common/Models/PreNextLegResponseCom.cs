namespace  CrewMobile.Common.Models
{
    public class PreNextLegResponseCom
    {
        public FlightDetailSoapCom2 FlightDetail { get; set; }

        public FlightCrewCom FlightCrew { get; set; }

        public PassengerListHeaderCom PassengerListHeader { get; set; }

        public FlightCountHeaderModel FlightCountHeader { get; set; }

        public SeatsHeaderModel SeatsHeaderModel { get; set; }

        public SSRCom SSRCom { get; set; }

        public PassengerListHeaderCom Prefer { get; set; }

        public SeatsHeaderModel Seats { get; set; }
    }
}