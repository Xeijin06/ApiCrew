using CrewMobile.Common.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Web;

namespace CrewMobileApi.Mocks
{
    public static class MockHelper
    {
        public static FlightDetailSoapCom2 GetFlightInformation()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mocks/FlightInformation.json");
            var jsonString = File.ReadAllText(filePath);
            var json = JsonConvert.DeserializeObject<FlightDetailSoapCom2>(jsonString);
            return json;
        }

        public static PassengerListHeaderCom MockPassengerList()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mocks/PassengerList.json");
            var jsonString = File.ReadAllText(filePath);
            var json = JsonConvert.DeserializeObject<PassengerListHeaderCom>(jsonString);
            return json;
        }

        public static PassengerListHeaderCom MockPrefer()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mocks/PreferList.json");
            var jsonString = File.ReadAllText(filePath);
            var json = JsonConvert.DeserializeObject<PassengerListHeaderCom>(jsonString);
            return json;
        }

        public static SSRCom MockSSR()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mocks/SSRList.json");
            var jsonString = File.ReadAllText(filePath);
            var json = JsonConvert.DeserializeObject<SSRCom>(jsonString);
            return json;
        }

        public static FlightCountHeaderModel MockCounts()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mocks/Counts.json");
            var jsonString = File.ReadAllText(filePath);
            var json = JsonConvert.DeserializeObject<FlightCountHeaderModel>(jsonString);
            return json;
        }

        public static SeatsHeaderModel MockSeats()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mocks/SeatsList.json");
            var jsonString = File.ReadAllText(filePath);
            var json = JsonConvert.DeserializeObject<SeatsHeaderModel>(jsonString);
            return json;
        }
    }
}

