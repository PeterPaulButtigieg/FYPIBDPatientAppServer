using FYPIBDPatientApp.Models;
using FYPIBDPatientApp.Dtos;
using FYPIBDPatientApp.Repositories;
using FYPIBDPatientApp.Services.Interfaces;

namespace FYPIBDPatientApp.Services
{
    public interface IPsService 
    {
        Task<Prescription> GetPrescription(int id);
        Task<List<Prescription>> GetPrescriptionsForPatient(string userId);
        Task<List<Prescription>> GetPrescriptionsForPatientOnDate(string userId, DateTime date);
        Task<List<Prescription>> GetCurrentPrescriptionsForPatient(string userId);
        Task NewPrescription(PrescriptionDto dto, string userId);
        Task ModifyPrescription(PrescriptionDto dto, string userId);
        Task DeletePrescription(int id);
    }

    public class PsService : IPsService
    {
        private readonly IPsRepository _repository;
        private readonly ILoggingService _logger;

        public PsService(IPsRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Prescription> GetPrescription(int id)
        {
            return await _repository.GetPrescriptionById(id);
        }

        public async Task<List<Prescription>> GetPrescriptionsForPatient(string userId)
        {
            return await _repository.GetPrescriptionsByPatientId(userId);
        }

        public async Task<List<Prescription>> GetPrescriptionsForPatientOnDate(string userId, DateTime date)
        {
            var activePrescriptions = await _repository.GetActivePrescriptionsByPatientId(userId);

            var duePrescriptions = activePrescriptions.Where(p =>
            {
                var daysSinceStart = (date - p.StartDate.Date).Days;

                if (p.Interval <= 0)
                    return false;

                return daysSinceStart % p.Interval == 0;

            }).ToList();

            return duePrescriptions;
        }

        public async Task<List<Prescription>> GetCurrentPrescriptionsForPatient(string userId)
        {
            return await _repository.GetActivePrescriptionsByPatientId(userId);
        }

        public async Task NewPrescription(PrescriptionDto dto, string userId)
        {
            var prescription = new Prescription
            {
                PatientId  = userId,
                Medication = dto.Medication,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Interval = dto.Interval,
                Frequency = dto.Notes,
            };

            await _repository.AddPrescription(prescription);
        }

        public async Task ModifyPrescription(PrescriptionDto dto, string userId)
        {
            var existingPrescription = await _repository.GetPrescriptionById(dto.Id);

            if (existingPrescription == null)
            {
                throw new Exception("Prescription not found.");
            }

            if (existingPrescription.PatientId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to modify this prescription.");
            }

            existingPrescription.Medication = dto.Medication;
            existingPrescription.Interval = dto.Interval;
            existingPrescription.Frequency = dto.Notes;
            existingPrescription.StartDate = dto.StartDate;
            existingPrescription.EndDate = dto.EndDate;

            await _repository.UpdatePrescription(existingPrescription);
        }

        public async Task DeletePrescription(int id)
        {
            var prescription = await _repository.GetPrescriptionById(id);

            if (prescription == null)
            {
                return;
            }

            await _repository.RemovePrescription(prescription);
        }



    }
}
