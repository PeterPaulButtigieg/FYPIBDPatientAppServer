using FYPIBDPatientApp.Dtos;
using FYPIBDPatientApp.Models;
using FYPIBDPatientApp.Repositories;
using FYPIBDPatientApp.Services.Interfaces;
using NuGet.Protocol.Core.Types;
using System.Drawing;


namespace FYPIBDPatientApp.Services
{
    public interface IDietService
    {
        Task<DietaryLog> GetDietaryLog(int id);
        Task<List<DietaryLog>> GetDietaryLogsForPatient(string userId);
        Task<List<DietaryLog>> GetDietaryLogsForPatientOnDate(string userId, DateTime date);
        Task RecordDietaryLog(DietaryLogDto dto, string userId);
        Task DeleteDietaryLog(int id);
    }
    public class DietService : IDietService
    {
        private readonly IDietRepository _repository;
        private readonly ILoggingService _logger;

        public DietService(IDietRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<DietaryLog> GetDietaryLog(int id)
        {
            return await _repository.GetDietaryLogById(id);
        }

        public async Task<List<DietaryLog>> GetDietaryLogsForPatient(string userId)
        {
            return await _repository.GetDietaryLogsByPatientId(userId);
        }

        public async Task<List<DietaryLog>> GetDietaryLogsForPatientOnDate(string userId, DateTime date)
        {
            return await _repository.GetDietaryLogsByPatientIdOnDate(userId, date);
        }

        public async Task RecordDietaryLog(DietaryLogDto dto, string userId)
        {
            var log = new DietaryLog
            {
                PatientId = userId,
                Date = dto.Date,
                FoodItem = dto.FoodItem,
                CookingMethod = dto.CookingMethod,
                Notes = dto.Notes,
            };

            await _repository.AddDietaryLog(log);
        }

        public async Task DeleteDietaryLog(int id)
        {
            var log = await _repository.GetDietaryLogById(id);

            if (log == null)
            {
                return;
            }

            await _repository.RemoveDietaryLog(log);
        }
    }

   
}
