using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using CrewMobileApi.Apis.Interfaces;
using CrewMobileApi.Apis;
using CrewMobile.Api.Models;
using CrewMobile.Domain.Models;
using CrewMobile.Common.Models;
using Microsoft.Identity.Client;
using System.IO;
using Renci.SshNet;
using CrewMobileApi.Services;
using CrewMobileApi.Business;
using GModels = Microsoft.Graph.Models;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using CrewMobileApi.Mocks;
using System.Security.Cryptography;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//TODO: Revisar y corregir la palabra Passanger a Passenger
//TODO: Validar uso de libreria de Telemetria

namespace CrewMobile.Api.Controllers
{
    /// <summary>
    /// Controller to provide information to App
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class FlightCrewsController : ControllerBase
    {
        #region Attributes

        /// <summary>
        /// The Graph Service
        /// </summary>
        private GraphService graphService;

        /// <summary>
        /// The data base context
        /// </summary>
        private ApplicationDbContext db;

        /// <summary>
        /// The API paramenters
        /// </summary>
        private CMParameter parameters;

        /// <summary>
        /// Azure domain for Grahp to get employee Id
        /// </summary>
        private string azureDomain;

        /// <summary>
        /// Read Configuration Values
        /// </summary>
        private IConfiguration configuration;

        /// <summary>
        /// The COPA API's instance
        /// </summary>
        private ICopaAPIs copaApi;

        /// <summary>
        /// The COPA Soap instance
        /// </summary>
        private ICopaSoap copaSoap;

        /// <summary>
        /// Time out for Copa APIs
        /// </summary>
        private int copaAPITimeOut;

        /// <summary>
        /// The fliht list for an employeee
        /// </summary>
        private List<PreNextLegResponseCom> listEmployeeFlights;

        /// <summary>
        /// Indicates if the passanger list could be get it
        /// </summary>
        private bool gotPassangerList;

        /// <summary>
        /// Indicates if the prefer list could be get it
        /// </summary>
        private bool gotPreferredList;

        /// <summary>
        /// Indicates if the SSR list could be get it
        /// </summary>
        private bool gotPassangerListSSR;

        /// <summary>
        /// Indicates if the counts list could be get it
        /// </summary>
        private bool gotCounts;

        /// <summary>
        /// Indicates if the seats list could be get it
        /// </summary>
        private bool gotSeats;

        /// <summary>
        /// Airports list
        /// </summary>
        private List<Airport> airports;

        /// <summary>
        /// Security SSR List
        /// </summary>
        private List<SecuritySSR> securitySSRs;

        /// <summary>
        /// Date in which the service was called
        /// </summary>
        private DateTime calledServiceDate;

        /// <summary>
        /// List of offsets for calculate local times
        /// </summary>
        private List<int> offSets;

        /// <summary>
        /// Leg index to indentificate flights with several legs
        /// </summary>
        private int legIndex;

        /// <summary>
        /// The response for next flight
        /// </summary>
        private NextLegResponse nextLegResponse;

        /// <summary>
        /// The stream writer in file log for FTP process
        /// </summary>
        private StreamWriter streamWriter;
        #endregion

        #region Constructors
        public FlightCrewsController(IConfiguration _configuration, GraphService _graphService, ApplicationDbContext context, ICopaAPIs _copaApi, ICopaSoap _copaSoap)
        {
            db = context;
            copaApi = _copaApi;
            copaSoap = _copaSoap;
            configuration = _configuration;
            graphService = _graphService;

            azureDomain = configuration["AzureAd:Domain"];
            copaAPITimeOut = int.Parse(configuration["CopaAPI:TimeOut"]);
        }
        #endregion

        #region Endpoints

        #region DB Endpoints
        /// <summary>
        /// GetFlightCrews
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetFlightCrews")]
        public IActionResult GetFlightCrews()
        {          
            var flightCrews = db.FlightCrews.ToList(); //.FirstOrDefault();
            return Ok(flightCrews);
        }

        /// <summary>
        /// GetFlightCrews
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetFlightCrews/{Id}")]
        public IActionResult GetFlightCrews(int Id)
        {
            var flightCrew = db.FlightCrews.Find(Id);
            if (flightCrew == null)
            {
                return NotFound();
            }
            return Ok(flightCrew);
        }

        /// <summary>
        /// AddFlightCrews
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPost]
        [Route("AddFlightCrews")]
        public IActionResult AddFlightCrews(FlightCrew flightCrew)
        {
            db.FlightCrews.Add(flightCrew);
            db.SaveChanges();
            return Ok(flightCrew);
        }


        /// <summary>
        /// UpdateFlightCrews
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPut]
        [Route("UpdateFlightCrews")]
        public IActionResult UpdateFlightCrews(FlightCrew flightCrew)
        {
            var flightCrewOld = db.FlightCrews.Find(flightCrew.FlightCrewId);
            if (flightCrewOld == null)
            {
                return NotFound();
            }
            //db.Update(flightCrew);
            db.Entry(flightCrewOld).CurrentValues.SetValues(flightCrew);
            db.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// DeleteFlightCrews
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpDelete]
        [Route("DeleteFlightCrews/{Id}")]
        public IActionResult DeleteFlightCrews(int Id)
        {
            var flightCrew = db.FlightCrews.Find(Id);
            if (flightCrew != null)
            {
                db.FlightCrews.Remove(flightCrew);
                db.SaveChanges();
            }
            return NoContent();
        }

        /// <summary>
        /// GetFlightCrewFileLogs
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetFlightCrewFileLogs")]
        public IActionResult GetFlightCrewFileLogs()
        {
            var fcFileLogs = db.ProccessFileLogs.ToList();
            return Ok(fcFileLogs);
        }

        // TODO: Validar el nombre de la tabla ProccessFileLogs
        
        /// <summary>
        /// GetFlightCrewFileLogs
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetFlightCrewFileLogs/{Id}")]
        public IActionResult GetFlightCrewFileLogs(int Id)
        {
            var fcFileLog = db.ProccessFileLogs.Find(Id);
            if (fcFileLog == null)
            {
                return NotFound();
            }
            return Ok(fcFileLog);
        }

        /// <summary>
        /// AddFlightCrewFileLogs
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPost]
        [Route("AddFlightCrewFileLogs")]
        public IActionResult AddFlightCrewFileLogs(ProccessFileLog fcFileLog)
        {
            db.ProccessFileLogs.Add(fcFileLog);
            db.SaveChanges();
            return Ok(fcFileLog);
        }


        /// <summary>
        /// UpdateParameters
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPut]
        [Route("UpdateFlightCrewFileLogs")]
        public IActionResult UpdateFlightCrewFileLogs(ProccessFileLog fcFileLog)
        {
            var fcFileLogOld = db.ProccessFileLogs.Find(fcFileLog.ProccessFileLogId);
            if (fcFileLogOld == null)
            {
                return NotFound();
            }
            //db.Update(parameter);
            db.Entry(fcFileLogOld).CurrentValues.SetValues(fcFileLog);
            db.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// DeleteFlightCrewFileLogs
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpDelete]
        [Route("DeleteFlightCrewFileLogs/{Id}")]
        public IActionResult DeleteFlightCrewFileLogs(int Id)
        {
            var fcFileLog = db.ProccessFileLogs.Find(Id);
            if (fcFileLog != null)
            {
                db.ProccessFileLogs.Remove(fcFileLog);
                db.SaveChanges();
            }
            return NoContent();
        }
        #endregion

        /// <summary>
        /// Get passenger list
        /// </summary>
        /// <param name="flightNumber">The flight number</param>
        /// <param name="departureDate">The departure date</param>
        /// <param name="operatorCarrier">The operator carrier</param>
        /// <param name="origin">The origin</param>
        /// <param name="destination">The destination</param>
        /// <param name="cabinClass">The cabin class</param>
        /// <returns>The passenger list</returns>
        [Authorize]
        [HttpGet]
        [Route("GetApiPassengerlist/{flightNumber}/{departureDate}/{operatorCarrier}/{origin}/{destination}/{cabinClass}")]
        public async Task<IActionResult> GetApiPassengerlist(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination,
            string cabinClass)
        {
            var result = await this.copaApi.GetPassengerList(
                flightNumber,
                departureDate,
                operatorCarrier,
                origin,
                destination,
                cabinClass);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Result.ToString());
            }

            return Ok(result.Result);
        }


        /// <summary>
        /// Get SSR list
        /// </summary>
        /// <param name="flightNumber">The flight number</param>
        /// <param name="departureDate">The departure date</param>
        /// <param name="operatorCarrier">The operator carrier</param>
        /// <param name="origin">The origin</param>
        /// <param name="destination">The destination</param>
        /// <returns>The passenger SSR list</returns>
        [Authorize]
        [HttpGet]
        [Route("GetApiSSR/{flightNumber}/{departureDate}/{operatorCarrier}/{origin}/{destination}")]
        public async Task<IActionResult> GetApiSSR(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination)
        {
            var result = await this.copaApi.GetSSR(
                flightNumber,
                departureDate,
                operatorCarrier,
                origin,
                destination);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Result.ToString());
            }

            return Ok(result.Result);
        }

        /// <summary>
        /// Get preferred list
        /// </summary>
        /// <param name="flightNumber">The flight number</param>
        /// <param name="departureDate">The departure date</param>
        /// <param name="operatorCarrier">The operator carrier</param>
        /// <param name="origin">The origin</param>
        /// <param name="destination">The destination</param>
        /// <returns>The passenger preferred list</returns>
        [Authorize]
        [HttpGet]
        [Route("GetApiPrefer/{flightNumber}/{departureDate}/{operatorCarrier}/{origin}/{destination}")]
        public async Task<IActionResult> GetApiPrefer(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination)
        {
            var result = await this.copaApi.GetPrefer(
                flightNumber,
                departureDate,
                operatorCarrier,
                origin,
                destination);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Result.ToString());
            }

            return Ok(result.Result);
        }

        /// <summary>
        /// Get passenger list meals
        /// </summary>
        /// <param name="flightNumber">The flight number</param>
        /// <param name="departureDate">The departure date</param>
        /// <param name="operatorCarrier">The operator carrier</param>
        /// <param name="origin">The origin</param>
        /// <param name="destination">The destination</param>
        /// <returns>The passenger counts</returns>
        [Authorize]
        [HttpGet]
        [Route("GetApiPassengerlistMeals/{flightNumber}/{departureDate}/{operatorCarrier}/{origin}/{destination}")]
        public async Task<IActionResult> GetApiPassengerlistMeals(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination)
        {
            var result = await this.copaApi.GetCounts(
                flightNumber,
                departureDate,
                operatorCarrier,
                origin,
                destination);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Result.ToString());
            }

            return Ok(result.Result);
        }

        /// <summary>
        /// Get passenger list stand by list
        /// </summary>
        /// <param name="flightNumber">The flight number</param>
        /// <param name="departureDate">The departure date</param>
        /// <param name="operatorCarrier">The operator carrier</param>
        /// <param name="origin">The origin</param>
        /// <param name="destination">The destination</param>
        /// <returns>The passenger stand by list</returns>
        [Authorize]
        [HttpGet]
        [Route("GetApiPassengerlistStandbyList/{flightNumber}/{departureDate}/{operatorCarrier}/{origin}/{destination}")]
        public async Task<IActionResult> GetApiPassengerlistStandbyList(
            string flightNumber,
            DateTime departureDate,
            string operatorCarrier,
            string origin,
            string destination)
        {
            var result = await this.copaApi.GetPassengerListStandbyList(
                flightNumber,
                departureDate,
                operatorCarrier,
                origin,
                destination);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Result.ToString());
            }

            return Ok(result.Result);
        }

        /// <summary>
        /// Get flight information
        /// </summary>
        /// <param name="departureFrom">Departure from</param>
        /// <param name="departureTo">Departure to</param>
        /// <param name="flightNumber">Flight number</param>
        /// <returns>Flight information</returns>
        [Authorize]
        [HttpGet]
        [Route("GetApiFlightInformation/{departureFrom}/{departureTo}/{flightNumber}")]
        public async Task<IActionResult> GetApiFlightInformation(
            DateTime departureFrom,
            DateTime departureTo,
            string flightNumber)
        {
            var result = await copaApi.GetApiFlightInformation(
                departureFrom,
                departureTo,
                flightNumber);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Result.ToString());
            }

            return Ok(result.Result);
        }

        /// <summary>
        /// Get time left for a fligth in local time from source city leg
        /// </summary>
        /// <param name="form">the fligth number, date, source and destination</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("GetTimeLeft")]
        public async Task<IActionResult> GetTimeLeft(JObject form)
        {
            string flightNumber;
            string date;
            string source;
            string destination;
            dynamic jsonObject = form;

            try
            {
                flightNumber = jsonObject.FlightNumber.Value;
                date = jsonObject.Date.Value.ToString();
                source = jsonObject.Source.Value;
                destination = jsonObject.Destination.Value;
            }
            catch (Exception ex)
            {
                return BadRequest("001. Incorrect call." + ex.ToString());
            }

            int index = 0;
            var flightInformation = await copaSoap.GetFlightInformation(flightNumber, date);
            if (flightInformation != null && flightInformation.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.FlightNumber != null)
            {
                if (flightInformation.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo.Count > 1)
                {
                    int k = 0;
                    for (; k < flightInformation.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo.Count; k++)
                    {
                        if (flightInformation.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo[k].Ns4DepartureAirport.LocationCode == source &&
                            flightInformation.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo[k].Ns4ArrivalAirport.LocationCode == destination)
                        {
                            break;
                        }
                    }

                    if (k == flightInformation.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo.Count)
                    {
                        k = 0;
                    }

                    index = k;
                }
                else
                {
                    index = 0;
                }
            }

            var departureDateScheduled = flightInformation.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo[index].Ns4DepartureDateTime.Scheduled.UtcDateTime.ToLocalTime();
            var departureDateEstimated = flightInformation.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo[index].Ns4DepartureDateTime.Estimated.UtcDateTime.ToLocalTime();
            var departureDateActual = flightInformation.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo[index].Ns4DepartureDateTime.Actual.UtcDateTime.ToLocalTime();

            int gmt = 0;
            var airport = await this.db.Airports.Where(a => a.AirportCode == source).FirstOrDefaultAsync();
            if (airport != null)
            {
                gmt = airport.GTMOffset;
            }

            DateTime departureDateUTC;

            if (string.IsNullOrEmpty(flightInformation.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo[legIndex].FlightStatus))
            {
                // Delayed
                departureDateUTC = departureDateEstimated.AddHours(gmt * -1);
            }
            else
            {
                // On time
                if (departureDateActual == DateTime.MinValue)
                {
                    departureDateUTC = departureDateScheduled.AddHours(gmt * -1);
                }
                else
                {
                    departureDateUTC = departureDateActual.AddHours(gmt * -1);
                }
            }

            var localDateTimeUTC = DateTime.Now.ToUniversalTime();
            var timeLeft = departureDateUTC.Subtract(localDateTimeUTC);
            return Ok(new TimeLeftResponse
            {
                Hours = timeLeft.Days * 24 + timeLeft.Hours,
                Minutes = timeLeft.Minutes,
            });
        }

        /// <summary>
        /// Get the next leg information
        /// </summary>
        /// <param name="form">the date and employee email</param>
        /// <returns>Whole the next flight information</returns>
        [Authorize]
        [HttpPost]
        [Route("GetNextLeg")]
        public async Task<IActionResult> GetNextLeg(JObject form)
        {
            string email = "";
            int employeeId = 0;
            dynamic jsonObject = form;

            try
            {
                try
                {
                    email = jsonObject.Email.Value;
                }
                catch (Exception ex)
                {
                    return BadRequest("001. Incorrect call." + ex.ToString());
                }

                var dateString = DateTime.Now.ToUniversalTime().ToString();

                nextLegResponse = new NextLegResponse();
                nextLegResponse.ServiceStatus = new ServiceStatus();
                listEmployeeFlights = new List<PreNextLegResponseCom>();
                parameters = await db.Parameters.FirstOrDefaultAsync();

                if (parameters.MockedEmployees)
                {
                    var employee = await db.Employees.Where(e => e.Email.ToLower().Equals(email.ToLower())).FirstOrDefaultAsync();

                    if (employee == null)
                    {
                        return BadRequest("002. The employee information can't be recovered.");
                    }

                    employeeId = employee.Code;
                }
                else
                {
                    var user = await GetUser(email);

                    if (user == null)
                    {
                        return BadRequest("002. The employee information can't be recovered.");
                    }

                    var userinfo = user["value"]?.FirstOrDefault()?.ToObject<GModels.User>();

                    employeeId = int.Parse(userinfo.EmployeeId);
                }

                var i = await GetFlightsInTheNext24Hours(dateString, employeeId);
                if (i == -1)
                {
                    return BadRequest("003. No available flights information.");
                }

                //TODO: Validar la necesidad de este codigo
                /*if (i + 1 == listEmployeeFlights.Count)
                {
                    return BadRequest("003. No available flights information.");
                }*/

                nextLegResponse.ShowFullFare = parameters.ShowFullFare;

                var task1 = GetPassangerList(i);
                var task2 = GetPreferredList(i);
                var task3 = GetPassangerListSSR(i);
                var task4 = GetCounts(i);
                var task5 = GetSeats(i);

                await Task.WhenAll(task1, task2, task3, task4, task5);

                nextLegResponse.ServiceStatus.GotPassangerList = gotPassangerList;
                if (!gotPassangerList)
                {
                    if (parameters.FailOneServiceFailAllServices)
                    {
                        return BadRequest("004. Service error in Passanger List.");
                    }
                }

                nextLegResponse.ServiceStatus.GotPreferredList = gotPreferredList;
                if (!gotPreferredList)
                {
                    if (parameters.FailOneServiceFailAllServices)
                    {
                        return BadRequest("005. Service error in Preferred List.");
                    }
                }

                nextLegResponse.ServiceStatus.GotPassangerListSSR = gotPassangerListSSR;
                if (!gotPassangerListSSR)
                {
                    if (parameters.FailOneServiceFailAllServices)
                    {
                        return BadRequest("006. Service error in SSR List.");
                    }
                }

                nextLegResponse.ServiceStatus.GotCounts = gotCounts;
                if (!gotCounts)
                {
                    if (parameters.FailOneServiceFailAllServices)
                    {
                        return BadRequest("007. Service error in Counts.");
                    }
                }

                nextLegResponse.ServiceStatus.GotSeats = gotSeats;
                if (!gotSeats)
                {
                    if (parameters.FailOneServiceFailAllServices)
                    {
                        return BadRequest("008. Service error in seats.");
                    }
                }

                nextLegResponse.PreferCountsList = new List<Count>();
                nextLegResponse.SSRCountsList = new List<Count>();
                nextLegResponse.MealCountsList = new List<Count>();
                nextLegResponse.BussinesCountsList = new List<Count>();

                GetAirportNames(i);
                await GetNextNextLeg(i);
                await MakeSSRAndMealsCounts(i);
                await UnifyPassangerListAndFixNames(i);
                MakePreferredCounts(i);
                GetBusinessCounts(i);
                SetRoll(i);
                await FixDescriptorsAndOrderBusinessAndPreferrredCounts();
                await MakeFinalResponse(i);
            }
            catch (Exception ex)
            {
                return BadRequest("001. Incorrect call." + ex.ToString());
            }

            return Ok(nextLegResponse);

        }

        /// <summary>
        /// Get Flights With Actual Information
        /// </summary>
        /// <param name="form">the fligth number, date, source and destination</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("GetFlightsWithActual")]
        public async Task<IActionResult> GetFlightsWithActual(JObject form)
        {
            var flightsWithActual = new List<string>();
            var date = DateTime.Now.ToUniversalTime();
            this.airports = await this.db.Airports.ToListAsync();
            var flightCrews = await db.FlightCrews.Where(fc => fc.DateStart >= date).ToListAsync();
            var flightCrewsUnique = flightCrews.GroupBy(fc => fc.FlightNumber).Select(fc => fc.First()).ToList();

            foreach (var flightCrewUnique in flightCrewsUnique)
            {
                int offset = 0;
                var airport = this.airports.Where(a => a.AirportCode == flightCrewUnique.Source).FirstOrDefault();
                if (airport != null)
                {
                    offset = airport.GTMOffset;
                }

                var dateFormated = string.Format("{0:yyyy-MM-dd}", flightCrewUnique.DateStart.AddHours(offset).Date);
                var result = this.copaSoap.GetFlightInformation2(flightCrewUnique.FlightNumber.ToString(), dateFormated);

                if (result.ToLower().Contains("actual"))
                {
                    flightsWithActual.Add($"Date: {dateFormated}, Flight number: {flightCrewUnique.FlightNumber} Crew Id: {flightCrewUnique.CrewId}");
                }
            }

            return Ok(flightsWithActual);
        }

        /// <summary>
        /// Get the next leg after previous flight was cancelled
        /// </summary>
        /// <param name="form">the date and employee email</param>
        /// <returns>Whole the next flight information after cancel previous</returns>
        [Authorize]
        [HttpPost]
        [Route("GetNextLegAfterCancelled")]
        public async Task<IActionResult> GetNextLegAfterCancelled(JObject form)
        {
            string email = "";
            int employeeId = 0;
            dynamic jsonObject = form;

            try
            {
                email = jsonObject.Email.Value;
            }
            catch (Exception ex)
            {
                return BadRequest("001. Incorrect call." + ex.ToString());
            }

            var dateString = DateTime.Now.ToUniversalTime().ToString();

            listEmployeeFlights = new List<PreNextLegResponseCom>();
            parameters = await db.Parameters.FirstOrDefaultAsync();

            if (parameters.MockedEmployees)
            {
                var employee = await db.Employees.Where(e => e.Email.ToLower().Equals(email.ToLower())).FirstOrDefaultAsync();

                if (employee == null)
                {
                    return BadRequest("002. The employee information can't be recovered.");
                }

                employeeId = employee.Code;
            }
            else
            {
                var user = await GetUser(email);

                if (user == null)
                {
                    return BadRequest("002. The employee information can't be recovered.");
                }

                var userinfo = user["value"]?.FirstOrDefault()?.ToObject<GModels.User>();

                employeeId = int.Parse(userinfo.EmployeeId);
            }

            var i = await GetFlightsInTheNext24Hours(dateString, employeeId);
            if (i == -1)
            {
                return BadRequest("003. No available flights information.");
            }

            if (i + 1 == listEmployeeFlights.Count)
            {
                return BadRequest("003. No available flights information.");
            }

            // Move to next leg after the cancellation
            i++;

            var task1 = GetPassangerList(i);
            var task2 = GetPreferredList(i);
            var task3 = GetPassangerListSSR(i);
            var task4 = GetCounts(i);
            var task5 = GetSeats(i);

            await Task.WhenAll(task1, task2, task3, task4, task5);

            nextLegResponse.ServiceStatus.GotPassangerList = gotPassangerList;
            if (!gotPassangerList)
            {
                if (parameters.FailOneServiceFailAllServices)
                {
                    return BadRequest("004. Service error in Passanger List.");
                }
                else
                {
                    listEmployeeFlights[i].PassengerListHeader = MockHelper.MockPassengerList();
                }
            }

            nextLegResponse.ServiceStatus.GotPreferredList = gotPreferredList;
            if (!gotPreferredList)
            {
                if (parameters.FailOneServiceFailAllServices)
                {
                    return BadRequest("005. Service error in Preferred List.");
                }
                else
                {
                    listEmployeeFlights[i].Prefer = MockHelper.MockPrefer();
                }
            }

            nextLegResponse.ServiceStatus.GotPassangerListSSR = gotPassangerListSSR;
            if (!gotPassangerListSSR)
            {
                if (parameters.FailOneServiceFailAllServices)
                {
                    return BadRequest("006. Service error in SSR List.");
                }
                else
                {
                    listEmployeeFlights[i].SSRCom = MockHelper.MockSSR();
                }
            }

            nextLegResponse.ServiceStatus.GotCounts = gotCounts;
            if (!gotCounts)
            {
                if (parameters.FailOneServiceFailAllServices)
                {
                    return BadRequest("007. Service error in Counts.");
                }
                else
                {
                    listEmployeeFlights[i].FlightCountHeader = MockHelper.MockCounts();
                }
            }

            nextLegResponse.ServiceStatus.GotSeats = gotSeats;
            if (!gotSeats)
            {
                if (parameters.FailOneServiceFailAllServices)
                {
                    return BadRequest("008. Service error in seats.");
                }
                else
                {
                    listEmployeeFlights[i].SeatsHeaderModel = MockHelper.MockSeats();
                }
            }

            nextLegResponse = new NextLegResponse();
            nextLegResponse.PreferCountsList = new List<Count>();
            nextLegResponse.SSRCountsList = new List<Count>();
            nextLegResponse.MealCountsList = new List<Count>();
            nextLegResponse.BussinesCountsList = new List<Count>();

            GetAirportNames(i);
            await GetNextNextLeg(i);
            await MakeSSRAndMealsCounts(i);
            await UnifyPassangerListAndFixNames(i);
            MakePreferredCounts(i);
            GetBusinessCounts(i);
            SetRoll(i);
            await FixDescriptorsAndOrderBusinessAndPreferrredCounts();
            await MakeFinalResponse(i);

            return Ok(nextLegResponse);
        }

        //TODO: Validar si se debe agregar Autorhize y route
        /// <summary>
        /// Call the process file FTP to get flight atendance agenda
        /// </summary>
        /// <returns>None</returns>       
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            await this.ProcessFile();
            return Ok("Ok");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Passanger List
        /// </summary>
        /// <param name="i">Flight index</param>
        /// <returns>None</returns>
        private async Task GetPassangerList(int i)
        {
            //this.gotPassangerList = false;
            //return;

            listEmployeeFlights[i].PassengerListHeader = new PassengerListHeaderCom();
            if (parameters.MockServices && parameters.AlwaysMockServices)
            {
                listEmployeeFlights[i].PassengerListHeader = MockHelper.MockPassengerList();
            }
            else
            {
                var result = await copaApi.GetPassengerList(
                    listEmployeeFlights[i].FlightCrew.FlightNumber.ToString(),
                    listEmployeeFlights[i].FlightCrew.DateStart.AddHours(offSets[i]),
                    listEmployeeFlights[i].FlightCrew.Company,
                    listEmployeeFlights[i].FlightCrew.Source,
                    listEmployeeFlights[i].FlightCrew.Destination,
                    "All");


                if (result.IsSuccess)
                {
                    listEmployeeFlights[i].PassengerListHeader = (PassengerListHeaderCom)result.Result;

                    result = null;

                    List<Ns4FlightLegInfo> allLegs = listEmployeeFlights[i].FlightDetail.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo;
                    if (allLegs.Count >= 2)
                    {
                        result = await copaApi.GetPassengerList(
                                                                listEmployeeFlights[i].FlightCrew.FlightNumber.ToString(),
                                                                listEmployeeFlights[i].FlightCrew.DateStart.AddHours(offSets[i]),
                                                                listEmployeeFlights[i].FlightCrew.Company,
                                                                allLegs[0].Ns4DepartureAirport.LocationCode,
                                                                allLegs[allLegs.Count - 1].Ns4ArrivalAirport.LocationCode,
                                                                "All");

                        PassengerListHeaderCom othePaxs = (PassengerListHeaderCom)result.Result;
                        listEmployeeFlights[i].PassengerListHeader.PassengerList.AddRange(othePaxs.PassengerList);
                    }




                    if (parameters.MockServices &&
                        (listEmployeeFlights[i].PassengerListHeader.PassengerList == null ||
                        listEmployeeFlights[i].PassengerListHeader.PassengerList.Count == 0))
                    {
                        listEmployeeFlights[i].PassengerListHeader = MockHelper.MockPassengerList();
                    }
                }
                else
                {
                    if (parameters.MockServices)
                    {
                        listEmployeeFlights[i].PassengerListHeader = MockHelper.MockPassengerList();
                    }
                    else
                    {
                        gotPassangerList = false;
                        return;
                    }
                }
            }



            gotPassangerList = true;
        }

        /// <summary>
        /// Get Preferred List
        /// </summary>
        /// <param name="i">Flight index</param>
        /// <returns>None</returns>
        private async Task GetPreferredList(int i)
        {
            //this.gotPreferredList = false;
            //return;

            if (parameters.MockServices && parameters.AlwaysMockServices)
            {
                listEmployeeFlights[i].Prefer = MockHelper.MockPrefer();
            }
            else
            {
                var result = await copaApi.GetPrefer(
                    listEmployeeFlights[i].FlightCrew.FlightNumber.ToString(),
                    listEmployeeFlights[i].FlightCrew.DateStart.AddHours(offSets[i]),
                    listEmployeeFlights[i].FlightCrew.Company,
                    listEmployeeFlights[i].FlightCrew.Source,
                    listEmployeeFlights[i].FlightCrew.Destination);
                if (result.IsSuccess)
                {
                    listEmployeeFlights[i].Prefer = (PassengerListHeaderCom)result.Result;

                    if (parameters.MockServices &&
                        (listEmployeeFlights[i].Prefer.PassengerList == null ||
                        listEmployeeFlights[i].Prefer.PassengerList.Count == 0))
                    {
                        listEmployeeFlights[i].Prefer = MockHelper.MockPrefer();
                    }
                }
                else
                {
                    if (parameters.MockServices)
                    {
                        listEmployeeFlights[i].Prefer = MockHelper.MockPrefer();
                    }
                    else
                    {
                        gotPreferredList = false;
                        return;
                    }
                }
            }

            gotPreferredList = true;
        }

        /// <summary>
        /// Get Passanger List SSR
        /// </summary>
        /// <param name="i">Flight index</param>
        /// <returns>None</returns>
        private async Task GetPassangerListSSR(int i)
        {
            //this.gotPassangerListSSR = false;
            //return;

            if (parameters.MockServices && parameters.AlwaysMockServices)
            {
                listEmployeeFlights[i].SSRCom = MockHelper.MockSSR();
            }
            else
            {
                var result = await copaApi.GetSSR(
                    listEmployeeFlights[i].FlightCrew.FlightNumber.ToString(),
                    listEmployeeFlights[i].FlightCrew.DateStart.AddHours(offSets[i]),
                    listEmployeeFlights[i].FlightCrew.Company,
                    listEmployeeFlights[i].FlightCrew.Source,
                    listEmployeeFlights[i].FlightCrew.Destination);
                if (result.IsSuccess)
                {
                    listEmployeeFlights[i].SSRCom = (SSRCom)result.Result;

                    if (parameters.MockServices &&
                        (listEmployeeFlights[i].SSRCom.PassengerList == null ||
                        listEmployeeFlights[i].SSRCom.PassengerList.Count == 0))
                    {
                        listEmployeeFlights[i].SSRCom = MockHelper.MockSSR();
                    }
                }
                else
                {
                    if (parameters.MockServices)
                    {
                        listEmployeeFlights[i].SSRCom = MockHelper.MockSSR();
                    }
                    else
                    {
                        gotPassangerListSSR = false;
                        return;
                    }
                }
            }

            gotPassangerListSSR = true;
        }

        /// <summary>
        /// Get counts
        /// </summary>
        /// <param name="i">Flight index</param>
        /// <returns>None</returns>
        private async Task GetCounts(int i)
        {
            if (parameters.MockServices && parameters.AlwaysMockServices)
            {
                listEmployeeFlights[i].FlightCountHeader = MockHelper.MockCounts();
            }
            else
            {
                var result = await copaApi.GetCounts(
                    listEmployeeFlights[i].FlightCrew.FlightNumber.ToString(),
                    listEmployeeFlights[i].FlightCrew.DateStart.AddHours(offSets[i]),
                    listEmployeeFlights[i].FlightCrew.Company,
                    listEmployeeFlights[i].FlightCrew.Source,
                    listEmployeeFlights[i].FlightCrew.Destination);
                if (result.IsSuccess)
                {
                    listEmployeeFlights[i].FlightCountHeader = (FlightCountHeaderModel)result.Result;
                }
                else
                {
                    if (parameters.MockServices)
                    {
                        listEmployeeFlights[i].FlightCountHeader = MockHelper.MockCounts();
                    }
                    else
                    {
                        gotCounts = false;
                        return;
                    }
                }
            }

            gotCounts = true;
        }

        /// <summary>
        /// Get Seats
        /// </summary>
        /// <param name="i">Flight index</param>
        /// <returns>True if the service return the data</returns>
        private async Task GetSeats(int i)
        {
            //this.gotSeats = false;
            //return;

            if (parameters.MockServices && parameters.AlwaysMockServices)
            {
                listEmployeeFlights[i].SeatsHeaderModel = MockHelper.MockSeats();
            }
            else
            {
                var result = await copaApi.GetSeats(
                    listEmployeeFlights[i].FlightCrew.FlightNumber.ToString(),
                    listEmployeeFlights[i].FlightCrew.DateStart.AddHours(offSets[i]),
                    listEmployeeFlights[i].FlightCrew.Company,
                    listEmployeeFlights[i].FlightCrew.Source,
                    listEmployeeFlights[i].FlightCrew.Destination);
                if (result.IsSuccess)
                {
                    listEmployeeFlights[i].SeatsHeaderModel = (SeatsHeaderModel)result.Result;
                }
                else
                {
                    if (parameters.MockServices)
                    {
                        listEmployeeFlights[i].SeatsHeaderModel = MockHelper.MockSeats();
                    }
                    else
                    {
                        gotSeats = false;
                        return;
                    }
                }
            }

            gotSeats = true;
        }

        /// <summary>
        /// Get Flights In The Next 24 Hours
        /// </summary>
        /// <param name="dateString">The date in string format</param>
        /// <param name="employeeId">The employee Id</param>
        /// <returns>-1 if it doesn't exists flight, or index value for the current flight</returns>
        private async Task<int> GetFlightsInTheNext24Hours(string dateString, int employeeId)
        {
            airports = await db.Airports.ToListAsync();
            var parameter = await db.Parameters.FirstOrDefaultAsync();
            var hoursToShowLegs = parameter.HoursToShowLegs;
            if (hoursToShowLegs <= 0)
            {
                hoursToShowLegs = 24;
            }

            //--Define DateFrom and DateTo in order to consult flights
            calledServiceDate = Convert.ToDateTime(dateString);
            var dateFrom = calledServiceDate.AddMinutes(parameters.MinuteToKeepFlight * -1);
            var dateTo = dateFrom.AddHours(hoursToShowLegs);

            //--Get Flights for specific Employee based on UTC Time
            var employeeFlights = await db.FlightCrews.
                Where(fc => fc.CrewId == employeeId && fc.DateEnd >= dateFrom && fc.DateEnd <= dateTo).
                ToListAsync();
            offSets = new List<int>();


            // Get next flight
            int i = 0;
            foreach (var currentEmpFlight in employeeFlights)
            {
                int offset = GetAirportTimeZone(currentEmpFlight.Source);

                var dateFormated = string.Format("{0:yyyy-MM-dd}", currentEmpFlight.DateStart.AddHours(offset).Date);
                //var flightInformation = copaSoap.GetFlightInformation(currentEmpFlight.FlightNumber.ToString(), dateFormated);
                var flightInformation = parameters.AlwaysMockServices ? MockHelper.GetFlightInformation() : await copaSoap.GetFlightInformation(currentEmpFlight.FlightNumber.ToString(), dateFormated);

                if (flightInformation == null)
                {
                    return -2;
                }

                if (flightInformation != null && flightInformation.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.FlightNumber != null)
                {
                    listEmployeeFlights.Add(new PreNextLegResponseCom
                    {
                        FlightCrew = currentEmpFlight,
                        FlightDetail = flightInformation,
                    });

                    offSets.Add(offset);
                }
                i++;
            }//--Fin de recorrer los vuelos del tripulante

            if (listEmployeeFlights.Count == 0)
            {
                return -1;
            }


            listEmployeeFlights = listEmployeeFlights.
                                    OrderBy(f => f.FlightDetail.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo[0].Ns4DepartureDateTime.Scheduled).
                                    ToList();

            int currentFlightIndex = 0;

            // Get next flight
            for (; currentFlightIndex < listEmployeeFlights.Count; currentFlightIndex++)
            {
                bool flightFound = false;
                Ns4FlightInfoDetails2 flightInfo = listEmployeeFlights[currentFlightIndex].FlightDetail.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails;
                //--Determina la pierna correcta del vuelo
                for (int k = 0; k < flightInfo.Ns4FlightLegInfo.Count; k++)
                {
                    if (flightInfo.Ns4FlightLegInfo[k].Ns4DepartureAirport.LocationCode == listEmployeeFlights[currentFlightIndex].FlightCrew.Source &&
                        flightInfo.Ns4FlightLegInfo[k].Ns4ArrivalAirport.LocationCode == listEmployeeFlights[currentFlightIndex].FlightCrew.Destination)
                    {
                        legIndex = k;
                        int arrivalOffSet = GetAirportTimeZone(listEmployeeFlights[currentFlightIndex].FlightCrew.Destination);
                        DateTime arrivalDate = GetCorrectDepartureOrArrivalTime(flightInfo.Ns4FlightLegInfo[k].Ns4ArrivalDateTime).AddHours(arrivalOffSet * -1);
                        if (calledServiceDate <= arrivalDate.AddMinutes(parameters.MinuteToKeepFlight))
                        {
                            flightFound = true;
                            break;
                        }
                    }
                }

                if (flightFound)
                    break;
            }
            return currentFlightIndex;
        }

        /// <summary>
        /// Determina la hora correcta del vuelo (Programada, Estimada y Real)
        /// </summary>
        /// <param name="flightDateTime">Información del vuelo</param>
        /// <returns>Hora Correcta</returns>
        private DateTime GetCorrectDepartureOrArrivalTime(Ns4DateTime flightDateTime)
        {
            if (flightDateTime.Actual.DateTime != DateTime.MinValue)
            {
                return flightDateTime.Actual.DateTime;
            }
            else if (flightDateTime.Estimated.DateTime != DateTime.MinValue)
            {
                return flightDateTime.Estimated.DateTime;
            }
            else
            {
                return flightDateTime.Scheduled.DateTime;
            }
        }

        /// <summary>
        /// Get the next next leg
        /// </summary>
        /// <param name="i">Flight index</param>
        private async Task GetNextNextLeg(int i)
        {
            if (listEmployeeFlights.Count < 2)
            {
                return;
            }

            if (i == listEmployeeFlights.Count - 1)
            {
                return;
            }

            nextLegResponse.NextLeg = new NextNextLegResponse();
            List<Ns4FlightLegInfo> legs = listEmployeeFlights[i + 1].FlightDetail.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo;
            int legIndex = 0;
            for (legIndex = 0; legIndex < legs.Count; legIndex++)
            {
                if (listEmployeeFlights[i + 1].FlightCrew.Source.ToUpper() == legs[legIndex].Ns4DepartureAirport.LocationCode.ToUpper() &&
                    listEmployeeFlights[i + 1].FlightCrew.Destination.ToUpper() == legs[legIndex].Ns4ArrivalAirport.LocationCode.ToUpper())
                {
                    break;
                }
            }
            var departureDate = GetCorrectDepartureOrArrivalTime(legs[legIndex].Ns4DepartureDateTime);
            var arrivalDate = GetCorrectDepartureOrArrivalTime(legs[legIndex].Ns4ArrivalDateTime);

            var source = airports.
                Where(a => a.AirportCode == listEmployeeFlights[i + 1].FlightCrew.Source).
                FirstOrDefault();

            if (source != null)
            {
                nextLegResponse.NextLeg.Source = new FlightResponse
                {
                    Abbreviation = listEmployeeFlights[i + 1].FlightCrew.Source.ToUpper(),
                    Country = source.CountryCode,
                    Date = departureDate,
                    Name = source.AirportAbbreviation,
                };
            }
            else
            {
                nextLegResponse.NextLeg.Source = new FlightResponse
                {
                    Abbreviation = listEmployeeFlights[i + 1].FlightCrew.Source.ToUpper(),
                    Country = "ND",
                    Date = departureDate,
                    Name = "No available",
                };
            }

            var destination = airports.
                Where(a => a.AirportCode == listEmployeeFlights[i + 1].FlightCrew.Destination).
                FirstOrDefault();

            if (destination != null)
            {
                nextLegResponse.NextLeg.Destination = new FlightResponse
                {
                    Abbreviation = listEmployeeFlights[i + 1].FlightCrew.Destination.ToUpper(),
                    Country = destination.CountryCode,
                    Date = arrivalDate,
                    Name = destination.AirportAbbreviation,
                };
            }
            else
            {
                nextLegResponse.NextLeg.Destination = new FlightResponse
                {
                    Abbreviation = listEmployeeFlights[i + 1].FlightCrew.Destination.ToUpper(),
                    Country = "ND",
                    Date = arrivalDate,
                    Name = "No available",
                };
            }

            await GetCounts(i + 1);
            nextLegResponse.NextLeg.Airline = listEmployeeFlights[i + 1].FlightDetail.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo[0].Ns4MarketingAirline.Code;
            nextLegResponse.NextLeg.FlightNumber = listEmployeeFlights[i + 1].FlightDetail.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.FlightNumber;
            await this.SetFlighStatusNextLeg(nextLegResponse.NextLeg, i + 1, true);
            SetCountersNextLeg(i);
        }

        /// <summary>
        /// SEt flight status for next leg
        /// </summary>
        /// <param name="i">Flight index</param>
        /// <returns>None</returns>
        private async Task SetFlighStatusNextLeg(NextNextLegResponse legResponse, int i, bool isNextLeg = false)
        {
            try
            {
                var fligthNumber = int.Parse(legResponse.FlightNumber);
                var date = legResponse.Source.Date.Date;

                var flighStatusMock = await db.FlighStatusMocks.Where(f => f.FlighNumber == fligthNumber && f.Date == date).FirstOrDefaultAsync();
                if (flighStatusMock == null)
                {
                    int legIndex = 0;
                    if (!isNextLeg)
                        legIndex = this.legIndex;

                    string currentStatus = listEmployeeFlights[i].FlightDetail.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo[legIndex].FlightStatus;
                    if (!string.IsNullOrEmpty(currentStatus))
                    {
                        if (currentStatus.ToLower().Contains("cancelled"))
                            legResponse.Status = "CANCELLED";
                        else
                            legResponse.Status = currentStatus;
                    }
                    else
                    {
                        if (legResponse.Source.DateEstimated > legResponse.Source.Date)
                        {

                            nextLegResponse.Status = "DELAYED";
                            legResponse.Source.Date = legResponse.Source.DateEstimated;
                            legResponse.Destination.Date = legResponse.Destination.DateEstimated;
                        }
                        else
                            legResponse.Status = "On-Time";
                    }
                }
                else
                {
                    legResponse.Status = flighStatusMock.Status;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Set counters for next leg
        /// </summary>
        /// <param name="i">Flight index</param>
        private void SetCountersNextLeg(int i)
        {
            if (listEmployeeFlights[i + 1].FlightCountHeader == null)
            {
                return;
            }

            int j = 0;

            if (listEmployeeFlights[i + 1].FlightCountHeader != null &&
                listEmployeeFlights[i + 1].FlightCountHeader.FlightCount.Count > 0 &&
                listEmployeeFlights[i + 1].FlightCountHeader.FlightCount[j].Counts.Count > 1)
            {
                nextLegResponse.NextLeg.Passangers = listEmployeeFlights[i + 1].FlightCountHeader.FlightCount[j].TotalPassengers;
                nextLegResponse.NextLeg.Meals = listEmployeeFlights[i + 1].FlightCountHeader.FlightCount[j].Counts[0].Meals +
                                                     listEmployeeFlights[i + 1].FlightCountHeader.FlightCount[j].Counts[1].Meals;
            }

            if (listEmployeeFlights[i + 1].FlightCountHeader != null &&
                listEmployeeFlights[i + 1].FlightCountHeader.FlightCount.Count > 0 &&
                listEmployeeFlights[i + 1].FlightCountHeader.FlightCount[j].Counts.Count > 0)
            {
                nextLegResponse.NextLeg.BussinessClass = listEmployeeFlights[i + 1].FlightCountHeader.FlightCount[j].Counts[0].LocalPassengers;
            }
        }

        /// <summary>
        /// Make SSR And Meals Counts
        /// </summary>
        /// <param name="i">Flight index</param>
        private async Task MakeSSRAndMealsCounts(int i)
        {
            //TODO: Validar necesidad de codigo comentado
            securitySSRs = await db.SecuritySSRs.ToListAsync();
            int specialMealsY = 0;
            int specialMealsC = 0;
            if (listEmployeeFlights[i].SSRCom != null && listEmployeeFlights[i].SSRCom.PassengerList != null)
            {
                foreach (var passanger in listEmployeeFlights[i].SSRCom.PassengerList)
                {
                    var lstobj = (from obj in passanger.SSRCodes
                                  select obj).GroupBy(n => new { n.Code, n.Description })
                                           .Select(g => g.FirstOrDefault())
                                           .ToList();
                    foreach (var ssr in lstobj)
                    {
                        if (!string.IsNullOrEmpty(ssr.Description) && ssr.Description.ToLower().Contains(" meal"))
                        {
                            if (passanger.CabinClass == "C")
                            {
                                specialMealsC++;
                            }
                            else
                            {
                                specialMealsY++;
                            }
                        }

                        var currentSSR = nextLegResponse.SSRCountsList.Where(s => s.Name == ssr.Code).FirstOrDefault();
                        if (currentSSR != null)
                        {
                            currentSSR.Value++;
                        }
                        else
                        {
                            var securitySSR = securitySSRs.Where(s => s.Code == ssr.Code).FirstOrDefault();
                            nextLegResponse.SSRCountsList.Add(new Count
                            {
                                Name = ssr.Code,
                                Value = 1,
                                IsSpecial = securitySSR != null,
                            });
                        }
                    }
                }
            }

            nextLegResponse.SSRCountsList = nextLegResponse.SSRCountsList.OrderByDescending(l => l.IsSpecial).ThenByDescending(l => l.Value).ToList();
            if (listEmployeeFlights[i].SSRCom != null && listEmployeeFlights[i].SSRCom.PassengerList != null)
            {
                if (nextLegResponse.IsFinal)
                {

                    //var result = from list in this.preNextLegResponses[i].SSRCom.PassengerList
                    //             .Where(p => string.IsNullOrEmpty(p.Seat))
                    //             from list2 in list.SSRCodes
                    //             .GroupBy(n => new { n.Code, n.Description })
                    //                       .Select(g => g.FirstOrDefault())
                    //                       .ToList()

                    //             select list2;

                    nextLegResponse.SSR = listEmployeeFlights[i].SSRCom.PassengerList.Where(p => string.IsNullOrEmpty(p.Seat)).ToList().Count;
                    //this.nextLegResponse.SSR = result.Count();
                }
                else
                {
                    //var result = from list in this.preNextLegResponses[i].SSRCom.PassengerList
                    //             from list2 in list.SSRCodes
                    //             .GroupBy(n => new { n.Code, n.Description })
                    //                       .Select(g => g.FirstOrDefault())
                    //                       .ToList()
                    //             select list2;

                    //var s = result.ToString();
                    nextLegResponse.SSR = listEmployeeFlights[i].SSRCom.PassengerList.Count;
                    //this.nextLegResponse.SSR = result.Count();
                }
            }

            int j = 0;
            if (nextLegResponse.IsFinal)
            {
                j = 1;
            }

            nextLegResponse.MealCountsList.Add(new Count
            {
                Name = "YC MEALS",
                Value = listEmployeeFlights[i].FlightCountHeader != null && listEmployeeFlights[i].FlightCountHeader.FlightCount.Count > 0 && listEmployeeFlights[i].FlightCountHeader.FlightCount[j].Counts.Count > 1 ? listEmployeeFlights[i].FlightCountHeader.FlightCount[j].Counts[1].Meals : 0,
            });

            nextLegResponse.MealCountsList.Add(new Count
            {
                Name = "BC MEALS",
                Value = listEmployeeFlights[i].FlightCountHeader != null && listEmployeeFlights[i].FlightCountHeader.FlightCount.Count > 0 && listEmployeeFlights[i].FlightCountHeader.FlightCount[j].Counts.Count > 1 ? listEmployeeFlights[i].FlightCountHeader.FlightCount[j].Counts[0].Meals : 0,
            });

            nextLegResponse.MealCountsList.Add(new Count
            {
                Name = "YC SPECIAL MEALS",
                Value = specialMealsY,

            });

            nextLegResponse.MealCountsList.Add(new Count
            {
                Name = "BC SPECIAL MEALS",
                Value = specialMealsC,
            });
        }

        /// <summary>
        /// Unify Passanger List And Fix Names
        /// </summary>
        /// <param name="i">Flight index</param>
        private async Task UnifyPassangerListAndFixNames(int i)
        {
            if (listEmployeeFlights[i].PassengerListHeader == null)
            {
                return;
            }

            foreach (var passanger in listEmployeeFlights[i].PassengerListHeader.PassengerList)
            {
                if (listEmployeeFlights[i].SSRCom != null && listEmployeeFlights[i].SSRCom.PassengerList != null)
                {
                    var ssr = listEmployeeFlights[i].SSRCom.PassengerList.
                        Where(s => s.GivenName == passanger.GivenName && s.Surname == passanger.Surname && s.Seat == passanger.Seat).
                        FirstOrDefault();
                    if (ssr != null)
                    {
                        var list = new List<SSRCode>();
                        foreach (var item in ssr.SSRCodes.Distinct().ToList())
                        {
                            var securitySSR = securitySSRs.Where(s => s.Code == item.Code).FirstOrDefault();
                            list.Add(new SSRCode
                            {
                                Code = item.Code,
                                Description = item.Description,
                                IsSpecial = securitySSR != null,
                            });
                        }

                        passanger.SSRCodes = list;
                    }
                }

                if (passanger.LoyaltyPrograms != null)
                {
                    foreach (var loyaltyProgram in passanger.LoyaltyPrograms)
                    {
                        if (string.IsNullOrEmpty(loyaltyProgram.StatusDescription))
                        {
                            loyaltyProgram.StatusDescription = "Member";
                        }
                    }
                }

                if (!string.IsNullOrEmpty(passanger.GivenName) && passanger.GivenName.EndsWith("MR"))
                {
                    passanger.Treatment = "MR";
                    passanger.GivenName = passanger.GivenName.Substring(0, passanger.GivenName.Length - 2);
                }
                else if (!string.IsNullOrEmpty(passanger.GivenName) && passanger.GivenName.EndsWith("DR"))
                {
                    passanger.Treatment = "DR";
                    passanger.GivenName = passanger.GivenName.Substring(0, passanger.GivenName.Length - 2);
                }
                else if (!string.IsNullOrEmpty(passanger.GivenName) && passanger.GivenName.EndsWith("MRS"))
                {
                    passanger.Treatment = "MRS";
                    passanger.GivenName = passanger.GivenName.Substring(0, passanger.GivenName.Length - 3);
                }
                else if (!string.IsNullOrEmpty(passanger.GivenName) && passanger.GivenName.EndsWith("MISS"))
                {
                    passanger.Treatment = "MISS";
                    passanger.GivenName = passanger.GivenName.Substring(0, passanger.GivenName.Length - 4);
                }

                var airport = airports.Where(a => a.AirportCode == passanger.FinalPassengerDestination).FirstOrDefault();
                if (airport != null)
                {
                    passanger.FinalPassengerDestination += $", {airport.CountryCode}";
                }
                else
                {
                    passanger.FinalPassengerDestination += ", NA";
                }

                var passengerIrregularOperation = await db.Passangers.Where(p => p.ConfirmationID == passanger.ConfirmationID).FirstOrDefaultAsync();
                //if (passanger.Surname.ToUpper().Trim() == "CRUZGUTIERREZ")
                //{
                //    var sddd = "dssdds";
                //    var s = passanger.ConfirmationID;
                //}
                if (passengerIrregularOperation != null)
                {
                    var currentflighNumber = int.Parse(listEmployeeFlights[i].FlightDetail.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.FlightNumber);
                    var irropFligthNumber = int.Parse(passengerIrregularOperation.IrregularOperation.FlightNumber);
                    if (currentflighNumber != irropFligthNumber)
                    {
                        if (nextLegResponse.Source.Abbreviation.Equals(passengerIrregularOperation.IrregularOperation.Destination) &&
                            passanger.FinalPassengerDestination.StartsWith(nextLegResponse.Destination.Abbreviation))
                        {
                            passanger.IrreguarOperation = new Common.Models.IrregularOperation
                            {
                                Code = "IRROP",
                                Description = passengerIrregularOperation.IrregularOperation.Description,
                            };
                        }
                    }
                }
            }

            listEmployeeFlights[i].PassengerListHeader.PassengerList = listEmployeeFlights[i].PassengerListHeader.PassengerList.OrderBy(p => p.Seat).ToList();
        }

        /// <summary>
        /// Make Preferred Counts
        /// </summary>
        /// <param name="i">Flight index</param>
        private void MakePreferredCounts(int i)
        {
            var objList = new List<PassengerListCom>();
            if (listEmployeeFlights[i].Prefer != null && listEmployeeFlights[i].Prefer.PassengerList != null)
            {

                objList = listEmployeeFlights[i]
                    .Prefer
                    .PassengerList
                    .Where(x => x.LoyaltyPrograms != null
                            && x.LoyaltyPrograms.Count > 0
                            && !x.LoyaltyPrograms.Exists(e => string.IsNullOrEmpty(e.StatusDescription))
                            && !x.LoyaltyPrograms.Exists(e => string.IsNullOrEmpty(e.Description))
                        && x.LoyaltyPrograms.Exists(e => e.Description.ToUpper().Equals("CONNECTMILES"))
                        && (x.LoyaltyPrograms.Exists(e => e.StatusDescription.ToUpper().Equals("SILVER"))
                        || x.LoyaltyPrograms.Exists(e => e.StatusDescription.ToUpper().Equals("GOLD"))
                        || x.LoyaltyPrograms.Exists(e => e.StatusDescription.ToUpper().Equals("PLATINUM"))
                        || x.LoyaltyPrograms.Exists(e => e.StatusDescription.ToUpper().Equals("PRESIDENTIAL")))).ToList();
                foreach (var passanger in objList)
                {
                    foreach (var loyaltyProgram in passanger.LoyaltyPrograms.Distinct())
                    {
                        var description = loyaltyProgram.StatusDescription.ToUpper();
                        var currentPrefer = nextLegResponse.PreferCountsList.Where(s => s.Name.ToUpper() == description).FirstOrDefault();
                        if (currentPrefer == null)
                        {
                            nextLegResponse.PreferCountsList.Add(new Count
                            {
                                Name = description,
                                Value = 1
                            });
                        }
                        else
                        {
                            currentPrefer.Value++;
                        }
                    }
                }
            }

            if (objList.Any())
            {
                if (nextLegResponse.IsFinal)
                {
                    nextLegResponse.Preferred = objList.Where(p => !string.IsNullOrEmpty(p.Seat)).ToList().Count;
                }
                else
                {
                    nextLegResponse.Preferred = objList.Count;
                }
            }

            //TODO: Validar necesidad de codigo comentado
            #region Codigo Anterior
            //if (this.preNextLegResponses[i].Prefer != null && this.preNextLegResponses[i].Prefer.PassengerList != null)
            //{
            //    foreach (var passanger in this.preNextLegResponses[i].Prefer.PassengerList)
            //    {
            //        if (passanger.LoyaltyPrograms == null || passanger.LoyaltyPrograms.Count == 0)
            //        {
            //            var currentPrefer = this.nextLegResponse.PreferCountsList.Where(s => s.Name == "MEMBERS").FirstOrDefault();
            //            if (currentPrefer != null)
            //            {
            //                currentPrefer.Value++;
            //            }
            //            else
            //            {
            //                this.nextLegResponse.PreferCountsList.Add(new Count
            //                {
            //                    Name = "MEMBERS",
            //                    Value = 1,
            //                });
            //            }
            //        }
            //        else
            //        {
            //            foreach (var loyaltyProgram in passanger.LoyaltyPrograms.Distinct())
            //            {
            //                var description = loyaltyProgram.StatusDescription;

            //                if (string.IsNullOrEmpty(description))
            //                {
            //                    description = "Members";
            //                }

            //                description = description.ToUpper();

            //                if (description == "MEMBER" ||
            //                    description == "MEMBER MEMBER" ||
            //                    description == "MEMBER MEMBER MEMBER" ||
            //                    description == "MEMBER MEMBER MEMBER MEMBER" ||
            //                    description == "MEMBER MEMBER MEMBER MEMBER MEMBER")
            //                {
            //                    description = "MEMBERS";
            //                }

            //                if (description == "PRESIDENTIAL" ||
            //                    description == "PRESIDENTIAL PRESIDENTIAL" ||
            //                    description == "PRESIDENTIAL PRESIDENTIAL PRESIDENTIAL" ||
            //                    description == "PRESIDENTIAL PRESIDENTIAL PRESIDENTIAL PRESIDENTIAL" ||
            //                    description == "PRESIDENTIAL PRESIDENTIAL PRESIDENTIAL PRESIDENTIAL PRESIDENTIAL")
            //                {
            //                    description = "PRESIDENTIAL";
            //                }

            //                var currentPrefer = this.nextLegResponse.PreferCountsList.Where(s => s.Name.ToUpper() == description).FirstOrDefault();
            //                if (currentPrefer != null)
            //                {
            //                    currentPrefer.Value++;
            //                }
            //                else
            //                {
            //                    this.nextLegResponse.PreferCountsList.Add(new Count
            //                    {
            //                        Name = description,
            //                        Value = 1,
            //                    });
            //                }
            //            }
            //        }
            //    }
            //}

            //if (this.preNextLegResponses[i].Prefer != null && this.preNextLegResponses[i].Prefer.PassengerList != null)
            //{
            //    if (this.nextLegResponse.IsFinal)
            //    {
            //        this.nextLegResponse.Preferred = this.preNextLegResponses[i].Prefer.PassengerList.Where(p => !string.IsNullOrEmpty(p.Seat)).ToList().Count;
            //    }
            //    else
            //    {
            //        this.nextLegResponse.Preferred = this.preNextLegResponses[i].Prefer.PassengerList.Count;
            //    }
            //}
            #endregion

        }

        /// <summary>
        /// Get Business Counts
        /// </summary>
        /// <param name="i">Flight index</param>
        private void GetBusinessCounts(int i)
        {
            if (listEmployeeFlights[i].PassengerListHeader == null)
            {
                return;
            }

            var businessPax = listEmployeeFlights[i].PassengerListHeader.PassengerList.Where(p => p.CabinClass == "C").ToList();
            foreach (var passanger in businessPax)
            {
                if (passanger.LoyaltyPrograms == null || passanger.LoyaltyPrograms.Count == 0)
                {
                    var currentPrefer = nextLegResponse.BussinesCountsList.Where(s => s.Name == "BUSINESS PAX").FirstOrDefault();
                    if (currentPrefer != null)
                    {
                        currentPrefer.Value++;
                    }
                    else
                    {
                        nextLegResponse.BussinesCountsList.Add(new Count
                        {
                            Name = "BUSINESS PAX",
                            Value = 1,
                        });
                    }
                }
                else
                {
                    foreach (var loyaltyProgram in passanger.LoyaltyPrograms.Distinct())
                    {
                        var name = loyaltyProgram.StatusDescription;
                        if (string.IsNullOrEmpty(name))
                        {
                            name = "BUSINESS PAX";
                        }
                        else
                        {
                            name = name.ToUpper();
                        }

                        if (string.IsNullOrEmpty(name))
                        {
                            name = loyaltyProgram.Description.ToUpper();
                        }

                        var currentPrefer = nextLegResponse.BussinesCountsList.Where(s => s.Name == name).FirstOrDefault();
                        if (currentPrefer != null)
                        {
                            currentPrefer.Value++;
                        }
                        else
                        {
                            nextLegResponse.BussinesCountsList.Add(new Count
                            {
                                Name = name,
                                Value = 1,
                            });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set the roll for crew
        /// </summary>
        /// <param name="i">Flight index</param>
        private void SetRoll(int i)
        {
            //TODO: Validar cambiar valores quemados a algo mas configurado en base de datos
            if (listEmployeeFlights[i].FlightCrew.CrewRoll.ToUpper().Contains("JC"))
            {
                nextLegResponse.Roll = "JC";
            }
            else if (listEmployeeFlights[i].FlightCrew.CrewRoll.ToUpper().Contains("BC"))
            {
                nextLegResponse.Roll = "BC";
            }
            else if (listEmployeeFlights[i].FlightCrew.CrewRoll.ToUpper().Contains("FA"))
            {
                nextLegResponse.Roll = "TCP";
            }
            else
            {
                nextLegResponse.Roll = "NA";
            }
        }

        /// <summary>
        /// Fix Descriptors And Order Business And Preferrred Counts
        /// </summary>
        /// <returns>None</returns>
        private async Task FixDescriptorsAndOrderBusinessAndPreferrredCounts()
        {
            var descriptors = await db.Descriptors.ToListAsync();

            foreach (var business in nextLegResponse.BussinesCountsList)
            {
                var descriptor = descriptors.Where(d => d.Name.ToUpper() == business.Name.ToUpper()).FirstOrDefault();
                if (descriptor == null)
                {
                    business.Order = 9;
                }
                else
                {
                    business.Name = descriptor.Abbreviation;
                    business.Order = descriptor.Order;
                }
            }

            nextLegResponse.BussinesCountsList = nextLegResponse.BussinesCountsList.OrderBy(b => b.Order).ToList();

            foreach (var preferred in nextLegResponse.PreferCountsList)
            {
                var descriptor = descriptors.Where(d => d.Name.ToUpper() == preferred.Name.ToUpper()).FirstOrDefault();
                if (descriptor == null)
                {
                    preferred.Order = 9;
                }
                else
                {
                    preferred.Name = descriptor.Abbreviation;
                    preferred.Order = descriptor.Order;
                }
            }

            nextLegResponse.PreferCountsList = nextLegResponse.PreferCountsList.OrderBy(b => b.Order).ToList();
        }

        /// <summary>
        /// Make the final response
        /// </summary>
        /// <param name="i">Flight index</param>
        private async Task MakeFinalResponse(int i)
        {
            nextLegResponse.Airline = listEmployeeFlights[i].FlightDetail.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo[legIndex].Ns4MarketingAirline.Code;
            nextLegResponse.FlightNumber = listEmployeeFlights[i].FlightDetail.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.FlightNumber;
            nextLegResponse.Gate = listEmployeeFlights[i].FlightDetail.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo[legIndex].Ns4DepartureAirport.Gate;
            nextLegResponse.Equipment = listEmployeeFlights[i].FlightDetail.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo[legIndex].Ns4Equipment.AirEquipType;

            await this.SetFlighStatusNextLeg(nextLegResponse, i);
            SetCounters(i);

            if (listEmployeeFlights[i].SeatsHeaderModel != null &&
                listEmployeeFlights[i].SeatsHeaderModel.FlightSegment != null &&
                listEmployeeFlights[i].SeatsHeaderModel.FlightSegment.EquipmentRows != null)
            {
                nextLegResponse.EquipmentRows = listEmployeeFlights[i].SeatsHeaderModel.FlightSegment.EquipmentRows;
            }

            if (listEmployeeFlights[i].PassengerListHeader != null)
            {
                if (nextLegResponse.IsFinal)
                {
                    nextLegResponse.PassengerList = listEmployeeFlights[i].PassengerListHeader.PassengerList.Where(p => !string.IsNullOrEmpty(p.Seat)).ToList();
                }
                else
                {
                    nextLegResponse.PassengerList = listEmployeeFlights[i].PassengerListHeader.PassengerList;
                }
            }
        }

        /// <summary>
        /// Set the flight counter
        /// </summary>
        /// <param name="i">Flight index</param>
        private void SetCounters(int i)
        {
            var hourDifference = nextLegResponse.Source.Date.Subtract(calledServiceDate.AddHours(offSets[i]));
            int j = 0;
            nextLegResponse.IsFinal = false;
            //TODO: Validar cambiar los 55 a un valor configurado en base de datos
            if (hourDifference.TotalMinutes <= 55)
            {
                nextLegResponse.IsFinal = true;
                j = 1;
                if (listEmployeeFlights[i].FlightCountHeader != null && listEmployeeFlights[i].FlightCountHeader.FlightCount.Count == 1)
                {
                    j = 0;
                }
            }

            if (listEmployeeFlights[i].FlightCountHeader != null && listEmployeeFlights[i].FlightCountHeader.FlightCount.Count > 0 && listEmployeeFlights[i].FlightCountHeader.FlightCount[j].Counts.Count > 1)
            {
                nextLegResponse.Passangers = listEmployeeFlights[i].FlightCountHeader.FlightCount[j].TotalPassengers;

                nextLegResponse.Meals = listEmployeeFlights[i].FlightCountHeader.FlightCount[j].Counts[0].Meals +
                                             listEmployeeFlights[i].FlightCountHeader.FlightCount[j].Counts[1].Meals;

                nextLegResponse.Thru = listEmployeeFlights[i].FlightCountHeader.FlightCount[j].Counts[0].ThruPassengers +
                                            listEmployeeFlights[i].FlightCountHeader.FlightCount[j].Counts[1].ThruPassengers;
            }

            if (listEmployeeFlights[i].FlightCountHeader != null && listEmployeeFlights[i].FlightCountHeader.FlightCount.Count > 0 && listEmployeeFlights[i].FlightCountHeader.FlightCount[j].Counts.Count > 0)
            {
                nextLegResponse.BussinessClass = listEmployeeFlights[i].FlightCountHeader.FlightCount[j].Counts[0].LocalPassengers +
                                                        listEmployeeFlights[i].FlightCountHeader.FlightCount[j].Counts[0].NonRevPassengers +
                                                        listEmployeeFlights[i].FlightCountHeader.FlightCount[j].Counts[0].ConnectingPassengers +
                                                        listEmployeeFlights[i].FlightCountHeader.FlightCount[j].Counts[0].ThruPassengers;

            }
        }

        /// <summary>
        /// Get Airport Names
        /// </summary>
        /// <param name="i">Flight index</param>
        /// <returns>None</returns>
        private void GetAirportNames(int i)
        {
            var source = airports.
                Where(a => a.AirportCode == listEmployeeFlights[i].FlightCrew.Source).
                FirstOrDefault();

            var departureDateScheduled = listEmployeeFlights[i].FlightDetail.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo[legIndex].Ns4DepartureDateTime.Scheduled.UtcDateTime.ToLocalTime();
            var departureDateEstimated = listEmployeeFlights[i].FlightDetail.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo[legIndex].Ns4DepartureDateTime.Estimated.UtcDateTime.ToLocalTime();
            var departureDateActual = listEmployeeFlights[i].FlightDetail.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo[legIndex].Ns4DepartureDateTime.Actual.UtcDateTime.ToLocalTime();
            var arrivalDateScheduled = listEmployeeFlights[i].FlightDetail.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo[legIndex].Ns4ArrivalDateTime.Scheduled.UtcDateTime.ToLocalTime();
            var arrivalDateEstimated = listEmployeeFlights[i].FlightDetail.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo[legIndex].Ns4ArrivalDateTime.Estimated.UtcDateTime.ToLocalTime();
            var arrivalDateActual = listEmployeeFlights[i].FlightDetail.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo[legIndex].Ns4ArrivalDateTime.Actual.UtcDateTime.ToLocalTime();

            if (departureDateEstimated == DateTime.MinValue)
            {
                departureDateEstimated = departureDateScheduled;
            }

            if (arrivalDateEstimated == DateTime.MinValue)
            {
                arrivalDateEstimated = arrivalDateScheduled;
            }

            if (departureDateActual != DateTime.MinValue)
            {
                //departureDateScheduled = departureDateActual;
                departureDateEstimated = departureDateActual;
            }

            if (arrivalDateActual != DateTime.MinValue)
            {
                //arrivalDateScheduled = arrivalDateActual;
                arrivalDateEstimated = arrivalDateActual;
            }


            DateTime departureDateUTC;

            if (string.IsNullOrEmpty(listEmployeeFlights[i].FlightDetail.SoapEnvelope.SoapBody.Ns3GetFlifoResponse.Ns4OtaAirFlifoRs.Ns4FlightInfoDetails.Ns4FlightLegInfo[legIndex].FlightStatus))
            {
                // Delayed
                departureDateUTC = departureDateEstimated.AddHours(offSets[i] * -1);
            }
            else
            {
                // On time
                departureDateUTC = departureDateScheduled.AddHours(offSets[i] * -1);
            }

            var localDateTimeUTC = DateTime.Now.ToUniversalTime();
            var timeLeft = departureDateUTC.Subtract(localDateTimeUTC);
            nextLegResponse.TimeLeft = new TimeLeftResponse
            {
                Hours = timeLeft.Days * 24 + timeLeft.Hours,
                Minutes = timeLeft.Minutes,
            };

            if (source != null)
            {
                nextLegResponse.Source = new FlightResponse
                {
                    Abbreviation = listEmployeeFlights[i].FlightCrew.Source.ToUpper(),
                    Country = source.CountryCode,
                    Date = departureDateScheduled,
                    DateEstimated = departureDateEstimated,
                    Name = source.AirportAbbreviation,
                };
            }
            else
            {
                nextLegResponse.Source = new FlightResponse
                {
                    Abbreviation = listEmployeeFlights[i].FlightCrew.Source.ToUpper(),
                    Country = "ND",
                    Date = departureDateScheduled,
                    DateEstimated = departureDateEstimated,
                    Name = "No available",
                };
            }

            var destination = airports.
                Where(a => a.AirportCode == listEmployeeFlights[i].FlightCrew.Destination).
                FirstOrDefault();

            if (destination != null)
            {
                nextLegResponse.Destination = new FlightResponse
                {
                    Abbreviation = listEmployeeFlights[i].FlightCrew.Destination.ToUpper(),
                    Country = destination.CountryCode,
                    Date = arrivalDateScheduled,
                    DateEstimated = arrivalDateEstimated,
                    Name = destination.AirportAbbreviation,
                };
            }
            else
            {
                nextLegResponse.Destination = new FlightResponse
                {
                    Abbreviation = listEmployeeFlights[i].FlightCrew.Destination.ToUpper(),
                    Country = "ND",
                    DateEstimated = arrivalDateEstimated,
                    Name = "No available",
                };
            }
        }

        /// <summary>
        /// Get Airport Time Zone
        /// </summary>
        /// <param name="airportCode">Airport IATA Code</param>
        /// <returns>The hour difference</returns>
        private int GetAirportTimeZone(string airportCode)
        {
            int offset = 0;
            var airport = airports.Where(a => a.AirportCode == airportCode).FirstOrDefault();
            if (airport != null)
            {
                offset = airport.GTMOffset;
            }
            return offset;
        }

        /// <summary>
        /// Process file FTP to get flight atendance agenda
        /// </summary>
        /// <returns>None</returns>
        private async Task ProcessFile()
        {
            var localLog = Path.Combine(Directory.GetCurrentDirectory(), "Content",
                                "Files", $"log{DateTime.Now:yyyyMMddHHmm}.txt");
            this.streamWriter = System.IO.File.CreateText(localLog);
            await this.SaveLog("Start Proceess", true, true);
            var host = configuration["SFTP:Site"];
            var port = int.Parse(configuration["SFTP:Port"]);
            var remoteFileName = configuration["SFTP:File"];
            var random = new Random();
            var localDestinationFilename = Path.Combine(Directory.GetCurrentDirectory(),
                "Content", "Files", $"crewmobile{DateTime.Now.Hour}{random.Next(0, 99)}.txt");
            var username = configuration["SFTP:User"];
            var password = configuration["SFTP:Password"];

            try
            {
                using (var sftp = new SftpClient(host, port, username, password))
                {
                    sftp.Connect();
                    await this.SaveLog("FTP Connected", true, false);

                    using (var file = System.IO.File.OpenWrite(localDestinationFilename))
                    {
                        sftp.DownloadFile(remoteFileName, file);
                        await this.SaveLog("File get", true, false);
                        //TODO: To uncomment in production mode
                        //sftp.DeleteFile(remoteFileName);
                        //await this.SaveLog("file remote delete", true, false);
                    }

                    sftp.Disconnect();
                }
            }
            catch (Exception ex)
            {
                await this.SaveLog(string.Format("Error: {0}", ex.Message), false, true);
                return;
            }

            await this.SaveLog(string.Format("Starts process file: {0}", localDestinationFilename), true, false);
            var processFile = new ProcessFile(localDestinationFilename, db);
            var rowsProcessed = await processFile.Process(this.streamWriter);
            await this.SaveLog(string.Format("Ends process file with {0} lines.", rowsProcessed), true, true);
            this.streamWriter.Close();
        }

        /// <summary>
        /// Save Log on file and DB
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="wasSucces">Indicates if the operation was succssfull</param>
        /// <param name="toDB">Indicates if save record on table log</param>
        /// <returns>None</returns>
        private async Task SaveLog(string message, bool wasSucces, bool toDB)
        {
            this.streamWriter.WriteLine(string.Format("{0} - {1}", DateTime.Now, message));
            this.streamWriter.Flush();
            if (toDB)
            {
                var proccessFileLog2 = new ProccessFileLog
                {
                    Date = DateTime.Now,
                    Steps = message,
                    WasSuccess = wasSucces,
                };

                //TODO: Validar el uso de una tabla para logs en la bd.
                db.ProccessFileLogs.Add(proccessFileLog2);
                await db.SaveChangesAsync();
            }
        }

        // TODO: Validar la necesidad de este codigo.
        /// <summary>
        /// Save file log
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>None</returns>
        private void SaveLog(string message)
        {
            this.streamWriter.WriteLine(message);
            this.streamWriter.Flush();
        }

        /// <summary>
        /// Get user from graph
        /// </summary>
        /// <param name="email">The user email</param>
        /// <returns>Graph user</returns>
        /// TODO: Revisar la logica de este metodo
        private async Task<JObject> GetUser(string email)
        {
            try
            {
                var user = await graphService.GetUserInformationByEmailAsync(email);
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }        
        }

        //TODO: Evaluar tema de ExtendedProperties
        /// <summary>
        /// Get extended properties
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>Extended properties</returns>
        private IDictionary<string, object> GetExtendedProperties(JObject user)
        {
            try
            {
                var graphUser = user["value"]?.FirstOrDefault()?.ToObject<GModels.User>();

                var userinfo = graphService.GetExtendedProperties(graphUser);
                return userinfo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}
