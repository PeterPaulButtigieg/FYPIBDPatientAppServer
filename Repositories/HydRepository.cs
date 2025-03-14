using FYPIBDPatientApp.Data;
using FYPIBDPatientApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FYPIBDPatientApp.Repositories
{
    public interface IHydRepository
    {
        Task<HydrationLog> GetHydrationLogById(int id);
        Task<List<HydrationLog>> GetHydrationLogsByPatientId(string userId);
        Task<List<HydrationLog>> GetGetHydrationLogsForPatientOnDate(string userId, DateTime date);
        Task AddHydrationLog(HydrationLog log);
        Task RemoveHydrationLog(HydrationLog log);
    }
    public class HydRepository : IHydRepository
    {
        private readonly AppDbContext _context;

        public HydRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<HydrationLog> GetHydrationLogById(int id)
        {
            return await _context.HydrationLogs.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<HydrationLog>> GetHydrationLogsByPatientId(string userId)
        {
            return await _context.HydrationLogs.Where(a => a.PatientId == userId).OrderBy(a => a.Date).ToListAsync();
        }

        public async Task<List<HydrationLog>> GetGetHydrationLogsForPatientOnDate(string userId, DateTime date)
        {
            return await _context.HydrationLogs.Where(a => a.PatientId == userId && a.Date == date).OrderBy(a => a.Date).ToListAsync();
        }

        public async Task AddHydrationLog(HydrationLog log)
        {
            _context.HydrationLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveHydrationLog(HydrationLog log)
        {
            _context.HydrationLogs.Remove(log);
            await _context.SaveChangesAsync();
        }
    }
}
