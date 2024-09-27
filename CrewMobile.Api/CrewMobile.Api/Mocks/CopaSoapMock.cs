using System;
using System.Collections.Generic;
using System.IO;
using CrewMobileApi.Apis.Interfaces;
using CrewMobile.Common.Models;
using CrewMobile.Domain.Models;
using Newtonsoft.Json;

namespace CrewMobileApi.Mocks
{
    public class CopaSoapMock : ICopaSoap
    {
        public const string VARPATH = @"C:\GitRepo\CrewMobile_API\CrewMobile.Api\CrewMobile.Api\Mocks\";

        public FlightDetailSoapCom2 GetFlightInformation(string flightNumber, string date)
        {
            var jsonString = File.ReadAllText($"{VARPATH}FlightInformation.json");
            var result = JsonConvert.DeserializeObject<FlightDetailSoapCom2>(jsonString);

            return result;
        }

        public string GetFlightInformation2(string flightNumber, string date)
        {
            throw new NotImplementedException();
        }

        public TimeZoneSoapCom GetListTimeZone(List<Airport> airports)
        {
            throw new NotImplementedException();
        }
    }
}