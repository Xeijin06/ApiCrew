using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CrewMobile.Common.Models
{
    public class AirportInfo
    {
        [JsonPropertyName("iatacode")]
        [Display(Name = "IATA Code")]
        [Required(ErrorMessage = "You must enter an {0}")]
        [StringLength(3, ErrorMessage = "The field {0} must contain {1} characters", MinimumLength = 3)]
        public string IataCode { get; set; }

        [JsonPropertyName("countrycode")]
        [Display(Name = "Country Code")]
        [Required(ErrorMessage = "You must enter an {0}")]
        [StringLength(2, ErrorMessage = "The field {0} must contain {1} characters", MinimumLength = 2)]
        public string CountryCode { get; set; }

        [JsonPropertyName("CityOfAirport")]
        [Display(Name = "City Of Airport")]
        [StringLength(50, ErrorMessage = "The field {0} must contain maximum {1} characters")]
        public string CityOfAirport { get; set; }

        [JsonPropertyName("AirportName")]
        [Display(Name = "Airport Name")]
        [Required(ErrorMessage = "You must enter an {0}")]
        [StringLength(50, ErrorMessage = "The field {0} must contain maximum {1} characters")]
        public string AirportName { get; set; }

        [JsonPropertyName("countryname")]
        [Display(Name = "Country Name")]
        [StringLength(50, ErrorMessage = "The field {0} must contain maximum {1} characters")]
        public string CountryName { get; set; }
    }
}
