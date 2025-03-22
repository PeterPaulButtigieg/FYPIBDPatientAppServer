using FYPIBDPatientApp.Data;
using FYPIBDPatientApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FYPIBDPatientApp.Repositories
{
    public interface IDietRepository
    {
        Task<DietaryLog> GetDietaryLogById(int id);
        Task<List<DietaryLog>> GetDietaryLogsByPatientId(string userId);
        Task<List<DietaryLog>> GetDietaryLogsByPatientIdOnDate(string userId, DateTime date);
        Task AddDietaryLog(DietaryLog log);
        Task RemoveDietaryLog(DietaryLog log);
    }

    public class DietRepository : IDietRepository
    {
        private readonly AppDbContext _context;

        public DietRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DietaryLog> GetDietaryLogById(int id)
        {
            return await _context.DietaryLogs.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<DietaryLog>> GetDietaryLogsByPatientId(string userId)
        {
            return await _context.DietaryLogs.Where(a => a.PatientId == userId).OrderBy(a => a.Date).ToListAsync();
        }

        public async Task<List<DietaryLog>> GetDietaryLogsByPatientIdOnDate(string userId, DateTime date)
        {
            return await _context.DietaryLogs.Where(a => a.PatientId == userId && a.Date == date).OrderBy(a => a.Date).ToListAsync();
        }

        public async Task AddDietaryLog(DietaryLog log)
        {
            _context.DietaryLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveDietaryLog(DietaryLog log)
        {
            _context.DietaryLogs.Remove(log);
            await _context.SaveChangesAsync();
        }
    }
}
