using System;
using System.IO;
using System.Threading.Tasks;
using CrewMobileApi.Apis.Interfaces;
using CrewMobile.Common.Models;
using Newtonsoft.Json;
using CrewMobile.Domain.Models;

namespace CrewMobileApi.Mocks
{
    public class CopaAPIsMock : ICopaAPIs
    {
        public const string VARPATH = @"C:\GitRepo\CrewMobile_API\CrewMobile.Api\CrewMobile.Api\Mocks\";
        public Task<Response> GetApiFlightInformation(DateTime departureFrom, DateTime departureTo, string flightNumber)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetApiAirportsInformation()
        {
            throw new NotImplementedException();
        }

        public async Task<Response> GetCounts(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination)
        {
            var jsonString = File.ReadAllText($"{VARPATH}Counts.json");
            var json = JsonConvert.DeserializeObject<FlightCountHeaderModel>(jsonString);

            return new Response
            {
                IsSuccess = true,
                Result = json
            };
        }

        public async Task<Response> GetPassengerList(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination,
            string cabinClass)
        {
            var jsonString = File.ReadAllText($"{VARPATH}PassengerList.json");
            var json = JsonConvert.DeserializeObject<PassengerListHeaderCom>(jsonString);

            return new Response
            {
                IsSuccess = true,
                Result = json
            };
        }

        public Task<Response> GetPassengerListStandbyList(string flightNumber, DateTime departureDate, string operatorCarrier, string origin, string destination)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> GetPrefer(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination)
        {
            //string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            var jsonString = File.ReadAllText($"{VARPATH}PreferList.json");
            var json = JsonConvert.DeserializeObject<PassengerListHeaderCom>(jsonString);

            return new Response
            {
                IsSuccess = true,
                Result = json
            };
        }

        public async Task<Response> GetSeats(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination)
        {
            var jsonString = File.ReadAllText($"{VARPATH}SeatsList.json");
            var json = JsonConvert.DeserializeObject<SeatsHeaderModel>(jsonString);

            return new Response
            {
                IsSuccess = true,
                Result = json
            };
        }

        public async Task<Response> GetSSR(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination)
        {

            var jsonString = File.ReadAllText($"{VARPATH}SSRList.json");
            var json = JsonConvert.DeserializeObject<SSRCom>(jsonString);

            return new Response
            {
                IsSuccess = true,
                Result = json
            };
        }

        public async Task<Response> GetIrregularOperations()
        {

            var jsonString = File.ReadAllText($"{VARPATH}IrrOps.json");
            var json = JsonConvert.DeserializeObject<FlightIrropHeader>(jsonString);

            return new Response
            {
                IsSuccess = true,
                Result = json
            };
        }

        //TODO: Definir respueesta Mockeada
        public async Task<Response> GetListTimeZone(List<Airport> airports)
        {
            return new Response
            {
                IsSuccess = true,
                Result = null //json
            };
        }
    }
}