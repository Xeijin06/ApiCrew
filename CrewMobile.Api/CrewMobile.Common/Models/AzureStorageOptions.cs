namespace CrewMobile.Common.Models
{
    public class AzureStorageOptions
    {
        public string ConnectionString { get; init; } = string.Empty;
        public string DefaultContainer { get; init; } = "processed";
    }
}
