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
