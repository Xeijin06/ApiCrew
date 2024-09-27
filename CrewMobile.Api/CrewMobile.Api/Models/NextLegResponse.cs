using System;
using System.Collections.Generic;
using CrewMobile.Common.Models;

namespace CrewMobile.Api.Models
{
    public class NextLegResponse : NextNextLegResponse
    {
        //TODO: Revisar codigo comentado
        //public FlghtResponse Source { get; set; }

        //public FlghtResponse Destination { get; set; }

        //public string Airline { get; set; }

        //public string FlightNumber { get; set; }

        //public int Passangers { get; set; }

        //public string Status { get; set; }

        //public int BussinessClass { get; set; }

        //public int SSR { get; set; }

        //public int Preferred { get; set; }

        //public int Meals { get; set; }

        public ServiceStatus ServiceStatus { get; set; }

        public TimeLeftResponse TimeLeft { get; set; }

        public int Thru { get; set; }

        public bool IsFinal { get; set; }

        public string Equipment { get; set; }

        public string Gate { get; set; }

        public string Roll { get; set; }

        public List<Count> PreferCountsList { get; set; }

        public List<Count> SSRCountsList { get; set; }

        public List<Count> MealCountsList { get; set; }

        public List<Count> BussinesCountsList { get; set; }

        public List<PassengerListCom> PassengerList;

        public NextNextLegResponse NextLeg { get; set; }

        public List<EquipmentRow> EquipmentRows { get; set; }

        public bool ShowFullFare { get; set; }
    }
}