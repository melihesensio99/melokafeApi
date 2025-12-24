namespace KafeApi.Application.Services.Abstract
{
    public interface ILogService<T>
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
    }
}
