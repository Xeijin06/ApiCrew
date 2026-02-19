using CrewMobile.Api.Models;
using CrewMobile.Common.Models;
using CrewMobileApi.Apis;
using CrewMobileApi.Apis.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CrewMobile.Api.Controllers
{
    //Todo: Validar la utilidad de este Controller
    //TODO: Revisar la necesidad del tratamiento SOAP (Modelos y Apis)

    [Route("api/[controller]")]
    [ApiController]
    public class TimeZonesController : ControllerBase
    {
        #region Attributes

        //private ICopaSoap copaSoap;
        private ICopaAPIs copaAPIs;

        private ApplicationDbContext db;

        #endregion

        #region Constructors
        
        /*public TimeZonesController(ApplicationDbContext context, ICopaSoap _copaSoap)
        {
            copaSoap = _copaSoap;
            db = context;
        }*/

        public TimeZonesController(ApplicationDbContext context, ICopaAPIs _copaApis)
        {
            copaAPIs = _copaApis;
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
            //var timeZones = copaSoap.GetListTimeZone(airports);
            var resultTimeZones = copaAPIs.GetListTimeZone(airports).Result;
            var timeZones = JsonConvert.DeserializeObject<TimeZoneResponse>(resultTimeZones.Result.ToString());
            //Soap.GetListTimeZone(airports);
            foreach (var airport in airports)
            {
                var timeZonesItems = timeZones.Result.TimeZoneInformation.Where(t => t.LocationCode == airport.AirportCode).FirstOrDefault(); //.SoapEnvEnvelope.SoapEnvBody.Ns0CopaTimeZoneInformationRs.Ns0TimeZones.Locations.Where(t => t.LocationCode == airport.AirportCode).FirstOrDefault();
                if ((timeZonesItems != null) && (timeZones != null))
                {
                    var timeZone = timeZonesItems.TimeZone.FirstOrDefault(); //.Ns1TimeZone.Offset.IndexOf(':');
                    if (timeZone != null)
                    {
                        var offSetString = timeZone.Offset.Split(":").FirstOrDefault();
                        var offSet = int.Parse(offSetString);
                        airport.GTMOffset = offSet;
                        airport.TimeZone = timeZone.IdTimeZone;
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