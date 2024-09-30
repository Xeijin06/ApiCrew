using System.ComponentModel.DataAnnotations;

namespace CrewMobile.Common.Models
{
    public class ParameterCom
    {
        [Display(Name = "Minimum Required Version")]
        public double MinimumRequiredVersion { get; set; }

        [Display(Name = "Mocked Employees")]
        public bool MockedEmployees { get; set; }

        [Display(Name = "Mock Services")]
        public bool MockServices { get; set; }

        [Display(Name = "Always Mock Services")]
        public bool AlwaysMockServices { get; set; }

        [Display(Name = "Fail one service fail all services")]
        public bool FailOneServiceFailAllServices { get; set; }

        [Display(Name = "Validate User Groups")]
        public bool ValidateUserGroups { get; set; }

        [Display(Name = "Auto Update Every Minutes")]
        public int AutoUpdateEveryMinutes { get; set; }

        [Display(Name = "Hours To Show Legs")]
        public int HoursToShowLegs { get; set; }

        [Display(Name = "Minutes to keep current flight")]
        public int MinuteToKeepFlight { get; set; }

        [Display(Name = "Show Full Fare")]
        public bool ShowFullFare { get; set; }
    }
}