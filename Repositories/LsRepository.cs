using FYPIBDPatientApp.Data;
using FYPIBDPatientApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FYPIBDPatientApp.Repositories
{
    public interface ILsRepository
    {
        Task<LifestyleLog> GetLifestyleLogById(int id);
        Task<List<LifestyleLog>> GetLifestyleLogsByPatientId(string userId);
        Task AddLifestyleLog(LifestyleLog log);
        Task RemoveLifestyleLog(LifestyleLog log);
    }
    public class LsRepository : ILsRepository
    {
        private readonly AppDbContext _context;

        public LsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LifestyleLog> GetLifestyleLogById(int id)
        {
            return await _context.LifestyleLogs.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<LifestyleLog>> GetLifestyleLogsByPatientId(string userId)
        {
            return await _context.LifestyleLogs.Where(a => a.PatientId == userId).OrderBy(a => a.Date).ToListAsync();
        }

        public async Task AddLifestyleLog(LifestyleLog log)
        {
            _context.LifestyleLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveLifestyleLog(LifestyleLog log)
        {
            _context.LifestyleLogs.Remove(log);
            await _context.SaveChangesAsync();
        }

    }
}
