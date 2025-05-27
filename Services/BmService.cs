using FYPIBDPatientApp.Models;
using FYPIBDPatientApp.Repositories;
using FYPIBDPatientApp.Dtos;
using FYPIBDPatientApp.Services.Interfaces;

namespace FYPIBDPatientApp.Services
{
    public interface IBmService
    {
        Task<BowelMovementLog> GetBowelMovementLog(int id);
        Task<List<BowelMovementLog>> GetBowelMovementLogsForPatient(string userId);
        Task<List<(int value, string label)>> GetBowelMovementRecapForPatient(string userId);
        Task RecordBowelMovementLog(BowelMovementLogDto dto, string userId);
        Task DeleteBowelMovementLog(int id);
    }
    public class BmService : IBmService
    {
        private readonly IBmRepository _repository;
        private readonly ILoggingService _logger;

        public BmService(IBmRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<BowelMovementLog> GetBowelMovementLog(int id)
        {
            return await _repository.GetBowelMovementLogById(id);
        }

        public async Task<List<BowelMovementLog>> GetBowelMovementLogsForPatient(string userId)
        {
            return await _repository.GetBowelMovementLogsByPatientId(userId);
        }

        public async Task<List<(int value, string label)>> GetBowelMovementRecapForPatient(string userId)
        {
            var today = DateTime.UtcNow.AddHours(2).Date;
            var weekAgo = today.AddDays(-6);

            var logs = await _repository.GetBowelMovementLogsByPatientInRange(
                userId,
                startInclusive: weekAgo,
                endExclusive: today.AddDays(1)
            );

            var countsByDate = logs
                .GroupBy(l => l.Date.Date)
                .ToDictionary(g => g.Key, g => g.Count());

            var recap = Enumerable
                .Range(0, 7)
                .Select(offset =>
                {
                    var date = weekAgo.AddDays(offset);
                    countsByDate.TryGetValue(date, out var cnt);
                    return (value: cnt,
                             label: date.ToString("ddd"));
                })
                .ToList();

            return recap;
        }

        public async Task RecordBowelMovementLog(BowelMovementLogDto dto, string userId)
        {
            var log = new BowelMovementLog
            {
                PatientId = userId,
                Date = DateTime.Now,
                StoolType = dto.StoolType,
                Blood = dto.Blood,
                Urgency = dto.Urgency,
                Notes = dto.Notes,
            };

            await _repository.AddBowelMovementLog(log);
        }

        public async Task DeleteBowelMovementLog(int id)
        {
            var log = await _repository.GetBowelMovementLogById(id);

            if (log == null)
            {
                return;
            }

            await _repository.RemoveBowelMovementLog(log);
        }

    }
}
