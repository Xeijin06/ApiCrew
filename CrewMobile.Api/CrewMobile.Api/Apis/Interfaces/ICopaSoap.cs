using CrewMobile.Common.Models;
using CrewMobile.Domain.Models;
using System.Collections.Generic;

namespace CrewMobileApi.Apis.Interfaces
{
    public interface ICopaSoap
    {
        Task<FlightDetailSoapCom2> GetFlightInformation(string flightNumber, string date);

        string GetFlightInformation2(string flightNumber, string date);

        TimeZoneSoapCom GetListTimeZone(List<Airport> airports);
    }
}