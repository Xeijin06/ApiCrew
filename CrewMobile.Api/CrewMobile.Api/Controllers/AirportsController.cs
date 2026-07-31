using CrewMobile.Api.Models;
using CrewMobile.Common.Models;
using CrewMobileApi.Apis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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

        #region Api Endpoints
        /// <summary>
        /// Consulta el API de catálogo de aeropuertos (GetApiAirportsInformation).
        /// </summary>
        /// <returns>Json</returns>
        [Authorize]
        [HttpGet]
        [Route("GetApiAirportsInformation")]
        public async Task<IActionResult> GetApiAirportsInformation()
        {
            var result = await copaApis.GetApiAirportsInformation();
            if (!result.IsSuccess)
            {
                return BadRequest(result.Result?.ToString() ?? result.Message);
            }

            return Ok(result.Result);
        }

        /// <summary>
        /// Revisa el API de listado de vuelos y el de TimeZone para registrar
        /// aeropuertos nuevos (enriquecidos con el catálogo de aeropuertos) y
        /// refrescar su TimeZone/GTMOffset.
        /// </summary>
        /// <param name="departureFrom">Inicio del rango de fechas de vuelo.</param>
        /// <param name="departureTo">Fin del rango de fechas de vuelo.</param>
        /// <param name="flightNumber">Número de vuelo (opcional, según lo requiera el API).</param>
        /// <returns>Json</returns>
        [Authorize]
        [HttpPost]
        [Route("RefreshAirports")]
        public async Task<IActionResult> RefreshAirports(
            DateTime departureFrom,
            DateTime departureTo,
            string flightNumber = "")
        {
            // 1. Listado de vuelos
            var flightsResult = await copaApis.GetApiFlightInformation(departureFrom, departureTo, flightNumber);
            if (!flightsResult.IsSuccess)
            {
                return BadRequest(flightsResult.Result?.ToString() ?? flightsResult.Message);
            }

            var flightHeader = flightsResult.Result as FlightDetailHeaderCom;
            if (flightHeader?.Flights == null || flightHeader.Flights.Count == 0)
            {
                return Ok(new { Added = 0, Updated = 0, Message = "No flights returned by the API." });
            }

            var codesFromFlights = flightHeader.Flights
                .SelectMany(f => new[] { f.OriginAirport, f.DestinationAirport })
                .Where(code => !string.IsNullOrWhiteSpace(code))
                .Select(code => code.Trim().ToUpperInvariant())
                .Distinct()
                .ToList();

            // 2. Catálogo maestro de aeropuertos (datos reales)
            var airportsInfoResult = await copaApis.GetApiAirportsInformation();
            if (!airportsInfoResult.IsSuccess)
            {
                return BadRequest(airportsInfoResult.Result?.ToString() ?? airportsInfoResult.Message);
            }

            var catalog = (airportsInfoResult.Result as AirportsResponse)?.Airports ?? new List<AirportInfo>();
            var catalogByCode = catalog
                .Where(a => !string.IsNullOrWhiteSpace(a.IataCode))
                .GroupBy(a => a.IataCode.Trim().ToUpperInvariant())
                .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

            // 3. Determinar nuevos (una sola consulta a Azure SQL)
            var existingCodes = await db.Airports.Select(a => a.AirportCode).ToListAsync();
            var existingSet = new HashSet<string>(existingCodes, StringComparer.OrdinalIgnoreCase);

            var newAirports = codesFromFlights
                .Where(code => !existingSet.Contains(code))
                .Select(code =>
                {
                    catalogByCode.TryGetValue(code, out var info);
                    return new Domain.Models.Airport
                    {
                        AirportCode = code,
                        AirportName = info?.CityOfAirport ?? code,
                        AirportAbbreviation = info?.CityOfAirport is { } city
                            ? GetAbbreviation(city)
                            : code,
                        CountryCode = info?.CountryCode ?? "NA",
                        TimeZone = string.Empty,
                        GTMOffset = 0
                    };
                })
                .ToList();

            // 4. Estrategia de ejecución + transacción (requerido con EnableRetryOnFailure en Azure SQL)
            var strategy = db.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await db.Database.BeginTransactionAsync();

                if (newAirports.Count > 0)
                {
                    await db.Airports.AddRangeAsync(newAirports);
                    await db.SaveChangesAsync();
                }

                var allAirports = await db.Airports.ToListAsync();
                var tzResult = await copaApis.GetListTimeZone(allAirports);
                var tzResponse = JsonConvert.DeserializeObject<TimeZoneResponse>(tzResult.Result.ToString());

                if (tzResponse?.Result?.TimeZoneInformation != null)
                {
                    foreach (var airport in allAirports)
                    {
                        var info = tzResponse.Result.TimeZoneInformation
                            .FirstOrDefault(t => t.LocationCode == airport.AirportCode);
                        var timeZone = info?.TimeZone.FirstOrDefault();
                        if (timeZone == null)
                        {
                            continue;
                        }

                        var offsetString = timeZone.Offset.Split(':').FirstOrDefault();
                        if (int.TryParse(offsetString, out var offset))
                        {
                            airport.GTMOffset = offset;
                            airport.TimeZone = timeZone.IdTimeZone;
                            db.Entry(airport).State = EntityState.Modified;
                        }
                    }
                }

                await db.SaveChangesAsync();
                await transaction.CommitAsync();
            });

            return Ok(new
            {
                Added = newAirports.Count,
                NewAirportCodes = newAirports.Select(a => a.AirportCode),
                Message = "Airports synchronized successfully."
            });
        }
        #endregion

        #region Methods
        /// <summary>
        /// Genera una abreviatura a partir del nombre de la ciudad del aeropuerto.
        /// Reglas: normaliza los guiones a espacios, corta en la primera stopword
        /// (conectores como de/la/del o sufijos geográficos como Island) y abrevia
        /// todas las palabras menos la última a su inicial seguida de punto y espacio.
        /// Ej: "Buenos Aires" => "B. Aires", "San Pedro Sula" => "S. P. Sula",
        /// "San Andres Island" => "S. Andres", "Port-Au-Prince" => "P. Prince",
        /// "Comayagua - Tegucigalpa" => "C. Tegucigalpa", "Santa Cruz de la Sierra" => "S. Cruz".
        /// </summary>
        /// <param name="cityOfAirport">Nombre de la ciudad del aeropuerto.</param>
        /// <returns>Abreviatura generada.</returns>
        private static string GetAbbreviation(string cityOfAirport)
        {
            if (string.IsNullOrWhiteSpace(cityOfAirport))
            {
                return string.Empty;
            }

            // Normalizar: tratar guiones como separadores de palabra
            cityOfAirport = cityOfAirport.Replace('-', ' ');

            // Palabras que cortan el nombre significativo (conectores + sufijos geográficos)
            var stopWords = new HashSet<string>(
                new[]
                {
                    "de", "del", "la", "las", "los", "el", "y", "e", "da", "do", "dos",
                    "au", "aux", "island", "islands", "isla", "islas", "city", "int", "international"
                },
                StringComparer.OrdinalIgnoreCase);

            var words = cityOfAirport
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            // Cortar en la primera stopword
            var meaningful = new List<string>();
            foreach (var word in words)
            {
                if (stopWords.Contains(word))
                {
                    break;
                }
                meaningful.Add(word);
            }

            if (meaningful.Count == 0)
            {
                return cityOfAirport;
            }

            if (meaningful.Count == 1)
            {
                return meaningful[0];
            }

            // Abreviar todas menos la última
            var abbreviated = meaningful
                .Take(meaningful.Count - 1)
                .Select(w => $"{char.ToUpperInvariant(w[0])}.");

            return string.Join(" ", abbreviated) + " " + meaningful[^1];
        }
        #endregion
    }
}
