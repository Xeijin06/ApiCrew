using CrewMobile.Api.Models;

namespace CrewMobile.Api.Services.Interface
{
    public interface ILogStorageAccountService
    {
        void Log(Models.LogLevel level, string message);

        List<LogEntry> GetAllLogs();
        void ClearLogs();
        Task<string> ExportToTextFormatAsync();
    }
}
