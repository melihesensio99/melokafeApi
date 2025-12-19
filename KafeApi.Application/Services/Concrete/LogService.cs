using KafeApi.Application.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace KafeApi.Application.Services.Concrete
{
    public class LogService : ILogService
    {
        private readonly ILogger<LogService> _logger;

        public LogService(ILogger<LogService> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string message)
        {
            _logger?.LogInformation("==> [INFO]: {Message}", message);
        }

        public void LogWarning(string message)
        {
            _logger?.LogWarning("==> [WARNING]: {Message}", message);
        }

        public void LogError(string message, Exception ex = null)
        {
            if (ex != null)
            {
                _logger?.LogError(ex, "==> [ERROR]: {Message}", message);
            }
            else
            {
                _logger?.LogError("==> [ERROR]: {Message}", message);
            }
        }
    }
}
