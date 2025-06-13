namespace CrewMobile.Common.Models
{
    public class ParameterResponse
    {
        public double MinimumRequiredVersion { get; set; }

        public string AvailableGroups { get; set; }

        public bool ValidateUserGroups { get; set; }

        public int AutoUpdateEveryMinutes { get; set; }
    }
}
