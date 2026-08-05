using System;
using System.Threading.Tasks;
using CrewMobile.Common.Models;
using CrewMobile.Domain.Models;

namespace CrewMobileApi.Apis.Interfaces
{
    public interface ICopaAPIs
    {
        Task<Response> GetSSR(
             string flightNumber,
             DateTime departureDate,
             string operatorCarrier,
             string origin,
             string destination);

        Task<Response> GetPrefer(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination);

        Task<Response> GetPassengerList(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination,
            string cabinClass);

        Task<Response> GetCounts(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination);

        Task<Response> GetPassengerListStandbyList(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination);

        Task<Response> GetApiFlightList(
            DateTime departureFrom,
            DateTime departureTo);

        Task<Response> GetApiFlightInformation(
            DateTime departureFrom,
            DateTime departureTo,
            string flightNumber);

        Task<Response> GetApiAirportsInformation();

        Task<Response> GetSeats(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination);

        Task <Response> GetIrregularOperations();

        Task<Response> GetListTimeZone(List<Airport> airports);
    }
}