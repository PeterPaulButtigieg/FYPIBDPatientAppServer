using FYPIBDPatientApp.Models;
using FYPIBDPatientApp.Dtos;
using FYPIBDPatientApp.Repositories;
using FYPIBDPatientApp.Services.Interfaces;

namespace FYPIBDPatientApp.Services
{
    public interface IHydService
    {
        Task<HydrationLog>GetHydrationLog(int id);
        Task<List<HydrationLog>>GetHydrationLogsForPatient(string userId);
        Task<List<HydrationLog>>GetHydrationLogForPatienOnDate(string userId, DateTime date);
        Task RecordHydrationLog(HydrationLogDto dto, string userId);
        Task DeleteHydrationLog(int id);
    }
    public class HydService : IHydService
    {
        private readonly IHydRepository _repository;
        private readonly ILoggingService _logger;

        public HydService(IHydRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<HydrationLog>GetHydrationLog(int id)
        {
            return await _repository.GetHydrationLogById(id);
        }

        public async Task<List<HydrationLog>>GetHydrationLogsForPatient(string userId)
        {
            return await _repository.GetHydrationLogsByPatientId(userId);
        }

        public async Task<List<HydrationLog>>GetHydrationLogForPatienOnDate(string userId, DateTime date)
        {
            return await _repository.GetGetHydrationLogsForPatientOnDate(userId, date);
        }

        public async Task RecordHydrationLog(HydrationLogDto dto, string userId)
        {
            var log = new HydrationLog
            {
                PatientId = userId,
                Date = DateTime.Now,
                WaterIntake = dto.WaterIntake,
                Notes = dto.Notes,
            };

            await _repository.AddHydrationLog(log);
        }

        public async Task DeleteHydrationLog(int id)
        {
            var log = await _repository.GetHydrationLogById(id);

            if (log == null)
            {
                return;
            }

            await _repository.RemoveHydrationLog(log);
        }

    }
}
