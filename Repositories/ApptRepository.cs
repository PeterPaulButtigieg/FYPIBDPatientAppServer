using FYPIBDPatientApp.Data;
using FYPIBDPatientApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace FYPIBDPatientApp.Repositories
{
    public interface IApptRepository
    {
        Task<Appointment> GetAppointmentById(int id);
        Task<List<Appointment>> GetAppointmentsByPatientId(string patientId);
        Task<List<Appointment>> GetFutureAppointmentsByPatientId(string patientId);
        Task<Appointment> GetNextAppointmentByPatientId(string id);
        Task AddAppointment(Appointment appointment);
    }
    public class ApptRepository : IApptRepository
    {
        private readonly AppDbContext _context;

        public ApptRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment> GetAppointmentById(int id)
        {
            return await _context.Appointments.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByPatientId(string patientId)
        {
            return await _context.Appointments.Where(a => a.PatientId == patientId).OrderBy(a => a.Date).ToListAsync();
        }

        public async Task<List<Appointment>> GetFutureAppointmentsByPatientId(string patientId)
        {
            var today = DateTime.Today;

            return await _context.Appointments.Where(a => a.PatientId == patientId && a.Date >= today).OrderBy(a => a.Date).ToListAsync();
        }

        public async Task<Appointment> GetNextAppointmentByPatientId(string patientId)
        {
            var today = DateTime.Today;

            return await _context.Appointments.Where(a => a.PatientId == patientId && a.Date >= today).OrderBy(a => a.Date).FirstOrDefaultAsync();
        }

        public async Task AddAppointment(Appointment appointment)
        {
            _context.Appointments.Add(appointment);

            await _context.SaveChangesAsync();
        }
    }
}
