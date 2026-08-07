using System;

namespace CrewMobile.Common.Models
{
    /// <summary>
    /// Parámetros para el endpoint RefreshAirports.
    /// </summary>
    public class RefreshAirportsRequest
    {
        /// <summary>Inicio del rango de fechas de vuelo.</summary>
        public DateTime DateFrom { get; set; }

        /// <summary>Fin del rango de fechas de vuelo.</summary>
        public DateTime DateTo { get; set; }

        /// <summary>Número de vuelo (opcional, según lo requiera el API).</summary>
        //public string FlightNumber { get; set; } = string.Empty;
    }
}
