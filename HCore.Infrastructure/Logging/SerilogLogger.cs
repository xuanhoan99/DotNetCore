using Serilog;

namespace HCore.Infrastructure.Logging
{
    public class SerilogLogger
    {
        private readonly Serilog.ILogger _logger;

        public SerilogLogger()
        {
            _logger = Log.Logger;
        }

        public void LogInfo(string message) => _logger.Information(message);
        public void LogWarning(string message) => _logger.Warning(message);
        public void LogError(string message) => _logger.Error(message);
    }
}
