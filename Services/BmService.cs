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

        public async Task RecordBowelMovementLog(BowelMovementLogDto dto, string userId)
        {
            var bowelMovementLog = new BowelMovementLog
            {
                PatientId = userId,
                Date = DateTime.Now,
                StoolType = dto.StoolType,
                Blood = dto.Blood,
                Urgency = dto.Urgency,
                Notes = dto.Notes,
            };

            await _repository.AddBowelMovementLog(bowelMovementLog);
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
