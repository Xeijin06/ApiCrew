using CrewMobile.Api.Models;
using CrewMobileApi.Apis.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrewMobile.Api.Controllers
{
    /// <summary>
    /// Controller to provide information to App
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class FlighStatusMocksController : ControllerBase
    {
        #region Attributes

        private ICopaAPIs copaApis;

        private ApplicationDbContext db;

        #endregion

        #region Constructors
        public FlighStatusMocksController(ApplicationDbContext context, ICopaAPIs _copaApi)
        {
            copaApis = _copaApi;
            db = context;
        }
        #endregion

        #region DB Endpoints
        /// <summary>
        /// GetAllFlighStatusMocks
        /// </summary>
        /// <returns>Json</returns>
        ////[Authorize]
        [HttpGet]
        [Route("GetAllFlighStatusMocks")]
        public IActionResult GetAllFlighStatusMocks()
        {
            var flighStatusMocks = db.FlighStatusMocks.ToList();
            return Ok(flighStatusMocks);
        }

        /// <summary>
        /// GetFlighStatusMocks
        /// </summary>
        /// <returns>Json</returns>
        ////[Authorize]
        [HttpGet]
        [Route("GetFlighStatusMockById/{Id}")]
        public IActionResult GetFlighStatusMockById(int Id)
        {
            var flighStatusMock = db.FlighStatusMocks.Find(Id);
            if (flighStatusMock == null)
            {
                return NotFound();
            }
            return Ok(flighStatusMock);
        }

        /// <summary>
        /// AddFlighStatusMock
        /// </summary>
        /// <returns>Json</returns>
        ////[Authorize]
        [HttpPost]
        [Route("AddFlighStatusMock")]
        public IActionResult AddFlighStatusMock(Domain.Models.FlighStatusMock flighStatusMock)
        {
            db.FlighStatusMocks.Add(flighStatusMock);
            db.SaveChanges();
            return Ok(flighStatusMock);
        }

        /// <summary>
        /// UpdateFlighStatusMock
        /// </summary>
        /// <returns>Json</returns>
        ////[Authorize]
        [HttpPut]
        [Route("UpdateFlighStatusMock")]
        public IActionResult UpdateFlighStatusMock(Domain.Models.FlighStatusMock flighStatusMock)
        {
            var flighStatusMockOld = db.FlighStatusMocks.Find(flighStatusMock.FlighStatusMockId);
            if (flighStatusMockOld == null)
            {
                return NotFound();
            }
            db.Entry(flighStatusMockOld).CurrentValues.SetValues(flighStatusMock);
            db.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// DeleteFlighStatusMock
        /// </summary>
        /// <returns>Json</returns>
        ////[Authorize]
        [HttpDelete]
        [Route("DeleteFlighStatusMock/{Id}")]
        public IActionResult DeleteFlighStatusMock(int Id)
        {
            var flighStatusMock = db.FlighStatusMocks.Find(Id);
            if (flighStatusMock != null)
            {
                db.FlighStatusMocks.Remove(flighStatusMock);
                db.SaveChanges();
            }
            return NoContent();
        }
        #endregion
    }
}
