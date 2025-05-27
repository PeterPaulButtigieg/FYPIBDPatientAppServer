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
        Task<(double TodayIntake, double PastSixDaysIntake)> GetHydrationTotalsForPatient(string userId);
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
        public async Task<(double TodayIntake, double PastSixDaysIntake)> GetHydrationTotalsForPatient(string userId)
        {
            var today = DateTime.UtcNow.AddHours(2).Date;
            var weekAgo = today.AddDays(-6);

            var logs = await _repository.GetHydrationLogsByPatientInRange(
                userId,
                startInclusive: weekAgo,
                endExclusive: today.AddDays(1)
            );

            var sumsByDate = logs
                .GroupBy(l => l.Date.Date)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(l => l.WaterIntake)
                );

            sumsByDate.TryGetValue(today, out var todayTotal);
            var pastSix = sumsByDate
                .Where(kvp => kvp.Key < today)
                .Sum(kvp => kvp.Value);

            return (todayTotal, pastSix);
        }

        public async Task RecordHydrationLog(HydrationLogDto dto, string userId)
        {
            var log = new HydrationLog
            {
                PatientId = userId,
                Date = dto.Date,
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
