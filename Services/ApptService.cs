using FYPIBDPatientApp.Models;
using FYPIBDPatientApp.Repositories;
using FYPIBDPatientApp.Dtos;
using FYPIBDPatientApp.Services.Interfaces;

namespace FYPIBDPatientApp.Services
{
    public interface IApptService
    {
        Task<Appointment> GetAppointment(int id);
        Task<List<Appointment>> GetAppointmentsForPatient(string userId);
        Task<List<Appointment>> GetFutureAppointmentsForPatient(string userId);
        Task<Appointment> GetNextAppointmentForPatient(string userId);
        Task RecordAppointment (AppointmentDto dto, string userId);
    }

    public class ApptService : IApptService
    {
        private readonly IApptRepository _repository;
        private readonly ILoggingService _logger;

        public ApptService(IApptRepository repository, ILoggingService logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Appointment> GetAppointment(int id)
        {
            return await _repository.GetAppointmentById(id);
        }

        public async Task<List<Appointment>> GetAppointmentsForPatient(string userId)
        {
            await _logger.LogActionAsync(userId, "GetAppointmentsForPatient");
            return await _repository.GetAppointmentsByPatientId(userId);
        }

        public async Task<List<Appointment>> GetFutureAppointmentsForPatient(string userId)
        {
            await _logger.LogActionAsync(userId, "GetFutureAppointmentsForPatient");
            return await _repository.GetFutureAppointmentsByPatientId(userId);
        }

        public async Task<Appointment> GetNextAppointmentForPatient(string userId)
        {
            await _logger.LogActionAsync(userId, "GetNextAppointmentForPatient");
            return await _repository.GetNextAppointmentByPatientId(userId);
        }

        public async Task RecordAppointment(AppointmentDto dto, string userId)
        {
            if (dto.Date < DateTime.Now)
            {
                await _logger.LogActionAsync(userId, "RecordAppointmentFailed");
                throw new ArgumentException("Appointment cannot be in the past.");
            }

            var appointment = new Appointment
            {
                PatientId = userId,
                Date = dto.Date,
                Venue = dto.Venue,
                AppointmentType = dto.AppointmentType,
                Notes = dto.Notes,
            };

            await _repository.AddAppointment(appointment);
            await _logger.LogActionAsync(userId, "RecordAppointment");
        }
    }
}