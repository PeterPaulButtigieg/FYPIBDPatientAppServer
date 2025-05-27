using FYPIBDPatientApp.Dtos;
using FYPIBDPatientApp.Models;
using FYPIBDPatientApp.Repositories;
using FYPIBDPatientApp.Services.Interfaces;

namespace FYPIBDPatientApp.Services
{
    public interface ISympService
    {
        Task<SymptomLog> GetSymptomLog(int id);
        Task<List<SymptomLog>> GetSymptomLogsForPatient(string userId);
        Task<List<(int value, string label)>> GetSymptomRecapForPatient(string userId);
        Task RecordSymptomLog(SymptomLogDto dto, string userId);
        Task DeleteSymptomLog(int id);
    }
    public class SympService : ISympService
    {
        private readonly ISympRepository _repository;
        private readonly ILoggingService _logger;

        public SympService(ISympRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<SymptomLog> GetSymptomLog(int id)
        {
            return await _repository.GetSymptomLogById(id);
        }

        public async Task<List<SymptomLog>> GetSymptomLogsForPatient(string userId)
        {
            return await _repository.GetSymptomLogsByPatientId(userId);
        }

        public async Task<List<(int value, string label)>> GetSymptomRecapForPatient(string userId)
        {
            var today = DateTime.UtcNow.AddHours(2).Date;
            var weekAgo = today.AddDays(-6);

            var logs = await _repository.GetSymptomLogsByPatientInRange(
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


        public async Task RecordSymptomLog(SymptomLogDto dto, string userId)
        {
            var log = new SymptomLog
            {
                PatientId = userId,
                Date = dto.Date,
                SymptomType = dto.SymptomType,
                Severity = dto.Severity,
                Notes = dto.Notes,
            };

            await _repository.AddSymptomLog(log);
        }

        public async Task DeleteSymptomLog(int id)
        {
            var log = await _repository.GetSymptomLogById(id);

            if (log == null)
            {
                return;
            }

            await _repository.RemoveSymptomLog(log);
        }
    }
}
