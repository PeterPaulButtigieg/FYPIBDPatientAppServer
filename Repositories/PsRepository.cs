using FYPIBDPatientApp.Data;
using FYPIBDPatientApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FYPIBDPatientApp.Repositories
{
    public interface IPsRepository 
    {
        Task<Prescription> GetPrescriptionById(int id);
        Task<List<Prescription>> GetPrescriptionsByPatientId(string userId);
        Task<List<Prescription>> GetActivePrescriptionsByPatientId(string userId);
        Task AddPrescription(Prescription prescription);
        Task UpdatePrescription(Prescription prescription);
        Task RemovePrescription(Prescription prescription);
    }
    public class PsRepository : IPsRepository
    {
        private readonly AppDbContext _context;

        public PsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Prescription> GetPrescriptionById(int id)
        {
            return await _context.Prescriptions.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Prescription>> GetPrescriptionsByPatientId(string userId)
        {
            return await _context.Prescriptions.Where(a => a.PatientId == userId).OrderBy(a => a.StartDate).ToListAsync();
        }

        public async Task<List<Prescription>> GetActivePrescriptionsByPatientId(string userId)
        {
            var today = DateTime.Today;

            return await _context.Prescriptions.Where(a => a.PatientId == userId && a.EndDate >= today && a.StartDate <= today).OrderBy(a => a.StartDate).ToListAsync();
        }

        public async Task AddPrescription(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePrescription(Prescription prescription)
        {
            _context.Prescriptions.Update(prescription);
            await _context.SaveChangesAsync();
        }

        public async Task RemovePrescription(Prescription prescription)
        {
            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();
        }


    }
}
