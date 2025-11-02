namespace Application.Common.Interfaces.Logging
{
    public interface ILoggingService<T>
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message, Exception? ex = null);
    }
}
