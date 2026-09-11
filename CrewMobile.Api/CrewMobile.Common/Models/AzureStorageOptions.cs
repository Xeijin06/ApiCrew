namespace CrewMobile.Common.Models
{
    public class AzureStorageOptions
    {
        public string ConnectionString { get; init; } = string.Empty;
        public string DefaultContainer { get; init; } = "processed";
        public int DayDeleteFile { get; set; } = 15;
        public int DayDeleteFileLog { get; set; } = 30;
        public bool EnableAutoCleanup { get; set; } = true;
        public string TimeZoneId { get; set; } = "REPLACE";
    }
}
