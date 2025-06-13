using CrewMobile.Api.Models;
using CrewMobileApi.Apis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrewMobile.Api.Controllers
{
    /// <summary>
    /// Controller to provide information to App
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class AirportsController : ControllerBase
    {
        #region Attributes

        private ICopaAPIs copaApis;

        private ApplicationDbContext db;

        #endregion

        #region Constructors
        public AirportsController(ApplicationDbContext context, ICopaAPIs _copaApi)
        {
            copaApis = _copaApi;
            db = context;
        }
        #endregion

        #region DB Endpoints
        /// <summary>
        /// GetAllAirports
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetAllAirports")]
        public IActionResult GetAllAirports()
        {
            var airports = db.Airports.ToList(); 
            return Ok(airports);
        }

        /// <summary>
        /// GetAirports
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetAirportById/{Id}")]
        public IActionResult GetAirportById(int Id)
        {
            var airport = db.Airports.Find(Id);
            if (airport == null)
            {
                return NotFound();
            }
            return Ok(airport);
        }

        /// <summary>
        /// AddAirport
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPost]
        [Route("AddAirport")]
        public IActionResult AddAirport(Domain.Models.Airport airport)
        {
            db.Airports.Add(airport);
            db.SaveChanges();
            return Ok(airport);
        }

        /// <summary>
        /// UpdateAirport
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPut]
        [Route("UpdateAirport")]
        public IActionResult UpdateAirport(Domain.Models.Airport airport)
        {
            var airportOld = db.Airports.Find(airport.AirportId);
            if (airportOld == null)
            {
                return NotFound();
            }
            db.Entry(airportOld).CurrentValues.SetValues(airport);
            db.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// DeleteAirport
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpDelete]
        [Route("DeleteAirport/{Id}")]
        public IActionResult DeleteAirport(int Id)
        {
            var airport = db.Airports.Find(Id);
            if (airport != null)
            {
                db.Airports.Remove(airport);
                db.SaveChanges();
            }
            return NoContent();
        }
        #endregion
    }
}
