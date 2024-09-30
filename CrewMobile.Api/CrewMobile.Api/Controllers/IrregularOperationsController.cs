using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CrewMobileApi.Apis.Interfaces;
using CrewMobile.Api.Models;
using CrewMobile.Domain.Models;
using CrewMobileApi.Apis;
using CrewMobile.Common.Models;


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

        //TODO: Validar agregar Authorize, http method y Route
        /// <summary>
        /// Process IrrOps
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Post()
        {
            await SaveLog("Start Proceess", true);
            var irrOps = await GetIrregularOperations();
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
                    Where(i => i.DepartureDate == operation.DepartureDate &&
                               i.FlightNumber == operation.FlightNum).
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

                var passangerList = await GetPassangerList(operation.FlightNum, operation.DepartureDate, "CM", operation.Origin, operation.Destination);
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
        private async Task<FlightIrropHeader> GetIrregularOperations()
        {
            var response = await copaApis.GetIrregularOperations();
            if (!response.IsSuccess)
            {
                return null;
            }

            var result = (FlightIrropHeader)response.Result;
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
