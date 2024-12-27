using CrewMobile.Api.Models;
using CrewMobileApi.Apis.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrewMobile.Api.Controllers
{
    //Todo: Validar la utilidad de este Controller

    [Route("api/[controller]")]
    [ApiController]
    public class TimeZonesController : ControllerBase
    {
        #region Attributes

        private ICopaSoap copaSoap;

        private ApplicationDbContext db;

        #endregion

        #region Constructors
        
        public TimeZonesController(ApplicationDbContext context, ICopaSoap _copaSoap)
        {
            copaSoap = _copaSoap;
            db = context;
        }

        #endregion

        #region Endpoints

        //TODO: Validar agregar Authorize y Route
        //TODO: Revisar el Summary
        /// <summary>
        /// Cargar las TimeZones de Aeropuertos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var airports = await db.Airports.ToListAsync();
            var timeZones = copaSoap.GetListTimeZone(airports);
            foreach (var airport in airports)
            {
                var timeZone = timeZones.SoapEnvEnvelope.SoapEnvBody.Ns0CopaTimeZoneInformationRs.Ns0TimeZones.Locations.Where(t => t.LocationCode == airport.AirportCode).FirstOrDefault();
                if (timeZone != null)
                {
                    var position = timeZone.Ns1TimeZone.Offset.IndexOf(':');
                    if (position != -1)
                    {
                        var offSetString = timeZone.Ns1TimeZone.Offset.Substring(0, position);
                        var offSet = int.Parse(offSetString);
                        airport.GTMOffset = offSet;
                        airport.TimeZone = timeZone.Ns1TimeZone.Ns1TimeZoneId;
                        db.Entry(airport).State = EntityState.Modified;
                    }

                }
            }

            await db.SaveChangesAsync();
            return Ok("Ok");
        }

        #endregion

        #region Methods

        #endregion
    }
}