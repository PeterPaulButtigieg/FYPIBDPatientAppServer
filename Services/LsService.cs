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
        Task<List<(double value, string label)>> GetSleepRecapForPatient(string userId);
        Task<List<(double value, string label)>> GetExerciseRecapForPatient(string userId);
        Task<List<(double value, string label)>> GetMoodRecapForPatient(string userId);
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

        public async Task<List<(double value, string label)>> GetSleepRecapForPatient(string userId)
        {
            var todayUtc = DateTime.UtcNow.Date;
            var weekAgoUtc = todayUtc.AddDays(-6);

            var logs = await _repository.GetLifestyleLogsByPatientInRange(
                userId,
                startInclusive: weekAgoUtc,
                endExclusive: todayUtc.AddDays(1)
            );


            var sumsByDate = logs
                .GroupBy(l => l.Date.Date)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(l => l.SleepDuration.TotalHours)
                );

            var recap =  Enumerable
                .Range(0, 7)
                .Select(offset => {
                    var date = weekAgoUtc.AddDays(offset);
                    sumsByDate.TryGetValue(date, out var hours);
                    return (value: Math.Round(hours, 2),
                             label: date.ToString("ddd"));
                })
                .ToList();

            return recap;
        }

        public async Task<List<(double value, string label)>> GetExerciseRecapForPatient(string userId)
        {
            var todayUtc = DateTime.UtcNow.Date;
            var weekAgoUtc = todayUtc.AddDays(-6);

            var logs = await _repository.GetLifestyleLogsByPatientInRange(
                userId,
                startInclusive: weekAgoUtc,
                endExclusive: todayUtc.AddDays(1)
            );


            var sumsByDate = logs
                .GroupBy(l => l.Date.Date)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(l => l.Exercise.TotalHours)
                );

            var recap = Enumerable
                .Range(0, 7)
                .Select(offset => {
                    var date = weekAgoUtc.AddDays(offset);
                    sumsByDate.TryGetValue(date, out var hours);
                    return (value: Math.Round(hours, 2),
                             label: date.ToString("ddd"));
                })
                .ToList();

            return recap;
        }

        public async Task<List<(double value, string label)>> GetMoodRecapForPatient(string userId)
        {
            var todayUtc = DateTime.UtcNow.Date;
            var weekAgoUtc = todayUtc.AddDays(-6);

            var logs = await _repository.GetLifestyleLogsByPatientInRange(
                userId,
                startInclusive: weekAgoUtc,
                endExclusive: todayUtc.AddDays(1)
            );


            var avgByDate = logs
                .GroupBy(l => l.Date.Date)
                .ToDictionary(
                    g => g.Key,
                    g => g.Any()
                        ? g.Average(l => l.StressLevel)
                        : 0.0
                );

            var recap = Enumerable
                .Range(0, 7)
                .Select(offset => {
                    var date = weekAgoUtc.AddDays(offset);
                    avgByDate.TryGetValue(date, out var avg);
                    return (value: Math.Round(avg, 2),
                             label: date.ToString("ddd"));
                })
                .ToList();

            return recap;
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

            var log = new LifestyleLog
            {
                PatientId = userId,
                Date = DateTime.UtcNow,
                SleepDuration = sleepDuration,
                Exercise = exercise,
                StressLevel = dto.StressLevel,
                Notes = dto.Notes ?? string.Empty
            };

            await _repository.AddLifestyleLog(log);
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
