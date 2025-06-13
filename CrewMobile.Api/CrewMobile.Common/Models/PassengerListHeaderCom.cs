using System;
using System.Collections.Generic;

namespace CrewMobile.Common.Models
{
    public class PassengerListHeaderCom
    {
        public FlightCom MarketingAirline { get; set; }

        public FlightCom OperatingAirline { get; set; }

        public string OriginDescription { get; set; }

        public string DestinationDescription { get; set; }

        public DateTime EstimatedDepartureDateTime { get; set; }

        public List<PassengerListCom> PassengerList { get; set; }
    }
}