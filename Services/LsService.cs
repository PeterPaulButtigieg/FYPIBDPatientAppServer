using FYPIBDPatientApp.Dtos;
using FYPIBDPatientApp.Models;
using FYPIBDPatientApp.Repositories;
using FYPIBDPatientApp.Services.Interfaces;
using System.Xml;


namespace FYPIBDPatientApp.Services
{
    public interface ILsService
    {
        Task<LifestyleLog> GetLifestyleLog(int id);
        Task<List<LifestyleLog>> GetLifestyleLogsForPatient(string userId);
        Task RecordLifestyleLog(LifestyleLogDto dto, string userId);
        Task DeleteLifestyleLog(int id);
    }

    public class LsService : ILsService
    {
        private readonly ILsRepository _repository;
        private readonly ILoggingService _logger;

        public LsService(ILsRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<LifestyleLog> GetLifestyleLog(int id)
        {
            return await _repository.GetLifestyleLogById(id);
        }

        public async Task<List<LifestyleLog>> GetLifestyleLogsForPatient(string userId)
        {
            return await _repository.GetLifestyleLogsByPatientId(userId);
        }
        public async Task RecordLifestyleLog(LifestyleLogDto dto, string userId)
        {
            TimeSpan sleepDuration;
            TimeSpan exercise;
            try
            {
                sleepDuration = XmlConvert.ToTimeSpan(dto.SleepDuration);
                exercise = XmlConvert.ToTimeSpan(dto.Exercise);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid sleep duration format. Expected ISO 8601 format.", ex);
            }

            if (sleepDuration < TimeSpan.Zero)
                throw new ArgumentException("Sleep duration must be non-negative.");

            if (exercise < TimeSpan.Zero)
                throw new ArgumentException("Exercise duration must be non-negative.");

            if (dto.StressLevel < 0)
                throw new ArgumentException("Stress level must be non-negative.");

            var log = new LifestyleLog
            {
                PatientId = userId,
                Date = DateTime.Now,
                SleepDuration = sleepDuration,
                Exercise = exercise,
                StressLevel = dto.StressLevel
            };
        }

        public async Task DeleteLifestyleLog(int id)
        {
            var log = await _repository.GetLifestyleLogById(id);

            if (log == null)
            {
                return;
            }

            await _repository.RemoveLifestyleLog(log);
        }


    }
}
