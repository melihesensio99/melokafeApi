using KafeApi.Application.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace KafeApi.Application.Services.Concrete
{
    public class LogService<T> : ILogService<T>
    {
        private readonly ILogger<T> _logger;

        public LogService(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string message)
        {
            _logger?.LogInformation("{Message}", message);
        }

        public void LogWarning(string message)
        {
            _logger?.LogWarning("{Message}", message);
        }

        public void LogError(string message)
        {

            _logger?.LogError("{Message}", message);

        }
    }
}
