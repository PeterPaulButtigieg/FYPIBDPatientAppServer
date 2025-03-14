using FYPIBDPatientApp.Data;
using FYPIBDPatientApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FYPIBDPatientApp.Repositories
{
    public interface IBmRepository
    {
        Task<BowelMovementLog> GetBowelMovementLogById(int id);
        Task<List<BowelMovementLog>> GetBowelMovementLogsByPatientId(string userId);
        Task AddBowelMovementLog(BowelMovementLog log);
        Task RemoveBowelMovementLog(BowelMovementLog log);
    }
    public class BmRepository : IBmRepository
    {
        private readonly AppDbContext _context;

        public BmRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BowelMovementLog> GetBowelMovementLogById(int id)
        {
            return await _context.BowelMovementLogs.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<BowelMovementLog>> GetBowelMovementLogsByPatientId(string userId)
        {
            return await _context.BowelMovementLogs.Where(a => a.PatientId == userId).OrderBy(a => a.Date).ToListAsync();
        }

        public async Task AddBowelMovementLog(BowelMovementLog log)
        {
            _context.BowelMovementLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveBowelMovementLog(BowelMovementLog log)
        {
            _context.BowelMovementLogs.Remove(log);
            await _context.SaveChangesAsync();
        }

    }
}
