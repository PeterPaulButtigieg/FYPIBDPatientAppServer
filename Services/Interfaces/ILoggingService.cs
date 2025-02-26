namespace FYPIBDPatientApp.Services.Interfaces
{
    public interface ILoggingService
    {
        Task LogActionAsync(string userId, string action);
    }
}
