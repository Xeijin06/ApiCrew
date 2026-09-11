using CrewMobile.Api.Models;
using CrewMobile.Api.Services.Interface;
using System.Collections.Concurrent;
using System.Globalization;

namespace CrewMobile.Api.Services
{
    public class LogStorageAccountService : ILogStorageAccountService
    {
        private readonly ConcurrentQueue<LogEntry> _logs;
        private readonly int _maxLogEntries;
        private readonly object _lock = new object();

        public LogStorageAccountService(int maxLogEntries = 10000)
        {
            _maxLogEntries = maxLogEntries;
            _logs = new ConcurrentQueue<LogEntry>();
        }

        public void Log(Models.LogLevel level, string message)
        {
            var logEntry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Level = level,
                Message = message
            };

            _logs.Enqueue(logEntry);
            lock (_lock)
            {
                while (_logs.Count > _maxLogEntries)
                {
                    _logs.TryDequeue(out _);
                }
            }
        }


        public List<LogEntry> GetAllLogs()
        {
            return _logs.ToList();
        }

        public void ClearLogs()
        {
            lock (_lock)
            {
                while (_logs.TryDequeue(out _)) { }
            }
        }

        public async Task<string> ExportToTextFormatAsync()
        {
            var logs = GetAllLogs().OrderBy(log => log.Timestamp);
            var textLines = new List<string>();

            foreach (var log in logs)
            {
                var formattedDate = log.Timestamp.ToString("d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                var line = $"{formattedDate} - {log.Message}";
                textLines.Add(line);
            }

            return await Task.FromResult(string.Join("\n", textLines));
        }
    }
}
