using CrewMobile.Api.Models;
using CrewMobile.Common.Models;
using CrewMobile.Domain.Models;
using CrewMobileApi.Apis;
using CrewMobileApi.Apis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web.Resource;


namespace CrewMobile.Api.Controllers
{
    /// <summary>
    /// Controller to provide information to App
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class IrregularOperationsController : ControllerBase
    {
        #region Attributes

        private ICopaAPIs copaApis;

        private ApplicationDbContext db;

        #endregion

        #region Constructors

        public IrregularOperationsController(ApplicationDbContext context, ICopaAPIs _copaApi)
        {
            copaApis = _copaApi;
            db = context;
        }

        #endregion

        #region Endpoints

        #region DB Endpoints
        /// <summary>
        /// GetIrregularOperations
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetIrregularOperations")]
        public IActionResult GetIrregularOperations()
        {
            var irrOps = db.IrregularOperations.ToList(); //.FirstOrDefault();
            return Ok(irrOps);
        }

        /// <summary>
        /// GetIrregularOperations
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetIrregularOperations/{Id}")]
        public IActionResult GetIrregularOperations(int Id)
        {
            var irrOps = db.IrregularOperations.Find(Id);
            if (irrOps == null)
            {
                return NotFound();
            }
            return Ok(irrOps);
        }

        /// <summary>
        /// AddFlightCrews
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPost]
        [Route("AddIrregularOperations")]
        public IActionResult AddIrregularOperations(Domain.Models.IrregularOperation irrOp)
        {
            db.IrregularOperations.Add(irrOp);
            db.SaveChanges();
            return Ok(irrOp);
        }


        /// <summary>
        /// UpdateIrregularOperations
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPut]
        [Route("UpdateIrregularOperations")]
        public IActionResult UpdateIrregularOperations(Domain.Models.IrregularOperation irrOp)
        {
            var irrOpOld = db.FlightCrews.Find(irrOp.IrregularOperationId);
            if (irrOpOld == null)
            {
                return NotFound();
            }
            //db.Update(flightCrew);
            db.Entry(irrOpOld).CurrentValues.SetValues(irrOp);
            db.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// DeleteIrregularOperations
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpDelete]
        [Route("DeleteIrregularOperations/{Id}")]
        public IActionResult DeleteIrregularOperations(int Id)
        {
            var irrOp = db.IrregularOperations.Find(Id);
            if (irrOp != null)
            {
                db.IrregularOperations.Remove(irrOp);
                db.SaveChanges();
            }
            return NoContent();
        }

        /// <summary>
        /// GetPassengers
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetPassengers")]
        public IActionResult GetPassengers()
        {
            var passangers = db.Passangers.ToList();
            return Ok(passangers);
        }

        /// <summary>
        /// GetPassengers
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetPassengers/{Id}")]
        public IActionResult GetPassengers(int Id)
        {
            var passanger = db.Passangers.Find(Id);
            if (passanger == null)
            {
                return NotFound();
            }
            return Ok(passanger);
        }

        /// <summary>
        /// AddParameters
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPost]
        [Route("AddPassengers")]
        public IActionResult AddPassengers(Passanger passanger)
        {
            db.Passangers.Add(passanger);
            db.SaveChanges();
            return Ok(passanger);
        }


        /// <summary>
        /// UpdateParameters
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPut]
        [Route("UpdatePassengers")]
        public IActionResult UpdatePassengers(Passanger passanger)
        {
            var passangerOld = db.Parameters.Find(passanger.PassangerId);
            if (passangerOld == null)
            {
                return NotFound();
            }
            //db.Update(parameter);
            db.Entry(passangerOld).CurrentValues.SetValues(passanger);
            db.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// DeleteParameters
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpDelete]
        [Route("DeletePassangers/{Id}")]
        public IActionResult DeletePassangers(int Id)
        {
            var passanger = db.Passangers.Find(Id);
            if (passanger != null)
            {
                db.Passangers.Remove(passanger);
                db.SaveChanges();
            }
            return NoContent();
        }

        /// <summary>
        /// GetIrropsLogs
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetIrropsLogs")]
        public IActionResult GetIrropsLogs()
        {
            var irrOpsLogs = db.IrregularOperationsLogs.ToList();
            return Ok(irrOpsLogs);
        }

        /// <summary>
        /// GetIrropsLogs
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetIrropsLogs/{Id}")]
        public IActionResult GetIrropsLogs(int Id)
        {
            var irrOpsLog = db.IrregularOperationsLogs.Find(Id);
            if (irrOpsLog == null)
            {
                return NotFound();
            }
            return Ok(irrOpsLog);
        }

        /// <summary>
        /// AddIrrOpsLogs
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPost]
        [Route("AddIrrOpsLogs")]
        public IActionResult AddIrrOpsLogs(IrregularOperationsLog irrOpLog)
        {
            db.IrregularOperationsLogs.Add(irrOpLog);
            db.SaveChanges();
            return Ok(irrOpLog);
        }


        /// <summary>
        /// UpdateParameters
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPut]
        [Route("UpdateIrrOpsLogs")]
        public IActionResult UpdateIrrOpsLogs(IrregularOperationsLog irrOpsLog)
        {
            var irrOpsLogOld = db.IrregularOperationsLogs.Find(irrOpsLog.IrregularOperationsLogId);
            if (irrOpsLogOld == null)
            {
                return NotFound();
            }
            //db.Update(parameter);
            db.Entry(irrOpsLogOld).CurrentValues.SetValues(irrOpsLog);
            db.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// DeleteParameters
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpDelete]
        [Route("DeleteIrrOpsLog/{Id}")]
        public IActionResult DeleteIrrOpsLog(int Id)
        {
            var irrOpsLog = db.IrregularOperationsLogs.Find(Id);
            if (irrOpsLog != null)
            {
                db.IrregularOperationsLogs.Remove(irrOpsLog);
                db.SaveChanges();
            }
            return NoContent();
        }
        #endregion

        //TODO: Validar agregar Authorize
        /// <summary>
        /// Process IrrOps
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [RequiredScopeOrAppPermission(AcceptedAppPermission = new[] { "azureFunction.Execute" })]
        [HttpPost]
        [Route("LoadIrrOps")]
        public async Task<IActionResult> LoadIrrOps()
        {
            await SaveLog("Start Proceess", true);
            var irrOps =  await GetAllIrregularOperations();
            if (irrOps == null)
            {
                var message = "No irregular operations found";
                await SaveLog(message, true);
                return BadRequest(message);
            }

            foreach (var operation in irrOps.FlightIrrops)
            {
                int operationId = 0;
                var currentIrrOps = await db.IrregularOperations.
                    Where(i => i.DepartureDate.Date == operation.DepartureDate.Date &&
                               i.FlightNumber == operation.FlightNum).Include(i => i.Passangers). //cmejiab: Adding the include passangers
                    FirstOrDefaultAsync();
                if (currentIrrOps == null)
                {
                    var irregularOpetation = new Domain.Models.IrregularOperation
                    {
                        DepartureDate = operation.DepartureDate,
                        Description = operation.IrropDescription,
                        Destination = operation.Destination,
                        FlightNumber = operation.FlightNum,
                        Origin = operation.Origin,
                    };

                    db.IrregularOperations.Add(irregularOpetation);
                    await db.SaveChangesAsync();
                    operationId = irregularOpetation.IrregularOperationId;
                }
                else
                {
                    await DeleteOldPassengers(currentIrrOps.Passangers);
                    operationId = currentIrrOps.IrregularOperationId;
                }

                var passangerList = await GetPassangerList(operation.FlightNum, operation.DepartureDate.ToString("yyyy-MM-dd"), "CM", operation.Origin, operation.Destination);
                if (passangerList != null)
                {
                    await InsertNewPassengers(operationId, passangerList);
                }
            }

            await SaveLog("End Proceess OK", true);
            return Ok("Ok");
        }

        #endregion

        #region Methods

        //TODO: Validar la necesidad de una tabla IrregularOperationsLogs con las mismas columnas que ProcessFileLog
        private async Task SaveLog(string message, bool wasSucces)
        {
            var irregularOperationsLog = new IrregularOperationsLog
            {
                Date = DateTime.Now,
                Steps = message,
                WasSuccess = wasSucces,
            };

            db.IrregularOperationsLogs.Add(irregularOperationsLog);
            await db.SaveChangesAsync();
        }

        //TODO: Agregar los summary
        private async Task<FlightIrropHeader> GetAllIrregularOperations()
        {
            var response = await copaApis.GetIrregularOperations();
            if (!response.IsSuccess)
            {
                return null;
            }

            var result = (FlightIrropHeader)response.Result;
            return result;

        }

        private async Task<FlightIrropHeader> GetAllIrregularOperationsLocal()
        {
            var result = new FlightIrropHeader
            {
                FlightIrrops = new List<FlightIrrop>
                {
                    new FlightIrrop { FlightNum = "AA101", Origin = "LAX", Destination = "JFK", DepartureDate = DateTime.Parse("2025-01-15"), IrropDescription = "None", },
                    new FlightIrrop { FlightNum = "BA202", Origin = "SFO", Destination = "LHR", DepartureDate = DateTime.Parse("2025-01-16"), IrropDescription = "Delay" },
                    new FlightIrrop { FlightNum = "DL303", Origin = "ORD", Destination = "ATL", DepartureDate = DateTime.Parse("2025-01-17"), IrropDescription = "Cancellation" },
                    new FlightIrrop { FlightNum = "UA404", Origin = "DEN", Destination = "IAH", DepartureDate = DateTime.Parse("2025-01-18"), IrropDescription = "None" },
                    new FlightIrrop { FlightNum = "AF505", Origin = "CDG", Destination = "NRT", DepartureDate = DateTime.Parse("2025-01-19"), IrropDescription = "Diversion" },
                    new FlightIrrop { FlightNum = "JL606", Origin = "HND", Destination = "LAX", DepartureDate = DateTime.Parse("2025-01-20"), IrropDescription = "Delay" },
                    new FlightIrrop { FlightNum = "QF707", Origin = "SYD", Destination = "SFO", DepartureDate = DateTime.Parse("2025-01-21"), IrropDescription = "None" },
                    new FlightIrrop { FlightNum = "EK808", Origin = "DXB", Destination = "JFK", DepartureDate = DateTime.Parse("2025-01-22"), IrropDescription = "Cancellation" },
                    new FlightIrrop { FlightNum = "LH909", Origin = "FRA", Destination = "SIN", DepartureDate = DateTime.Parse("2025-01-23"), IrropDescription = "None" },
                    new FlightIrrop { FlightNum = "CA1010", Origin = "PEK", Destination = "LHR", DepartureDate = DateTime.Parse("2025-01-24"), IrropDescription = "Diversion" }
                }
            };
            return result;

        }

        private async Task InsertNewPassengers(int operationId, List<PassengerListCom> passangerList)
        {
            foreach (var passenger in passangerList)
            {
                this.db.Passangers.Add(new Passanger
                {
                    BookingClass = passenger.BookingClass,
                    CabinClass = passenger.CabinClass,
                    FareKind = passenger.FareKind,
                    FinalPassengerDestination = passenger.FinalPassengerDestination,
                    GivenName = passenger.GivenName,
                    IrregularOperationId = operationId,
                    IsDiscountedFare = passenger.IsDiscountedFare,
                    IsFullFare = passenger.IsFullFare,
                    Seat = passenger.Seat,
                    SeatLeg = passenger.SeatLeg,
                    Surname = passenger.Surname,
                    Treatment = passenger.Treatment,
                    ConfirmationID = passenger.ConfirmationID,
                });
            }

            await db.SaveChangesAsync();
        }

        private async Task DeleteOldPassengers(ICollection<Passanger> passangers)
        {
            db.Passangers.RemoveRange(passangers);
            await db.SaveChangesAsync();
        }

        private async Task<List<PassengerListCom>> GetPassangerList(string flightNumber, string date, string company, string source, string destination)
        {
            var dateFlight = Convert.ToDateTime(date);
            var response = await copaApis.GetPassengerList(flightNumber, dateFlight, company, source, destination, "All");
            if (!response.IsSuccess)
            {
                return null;
            }

            var result = (PassengerListHeaderCom)response.Result;
            return result.PassengerList;
        }

        #endregion
    }
}
