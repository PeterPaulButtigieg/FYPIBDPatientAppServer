using FYPIBDPatientApp.Data;
using FYPIBDPatientApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FYPIBDPatientApp.Repositories
{
    public interface ISympRepository
    {
        Task<SymptomLog>GetSymptomLogById(int id);
        Task<List<SymptomLog>>GetSymptomLogsByPatientId(string userId);
        Task<List<SymptomLog>> GetSymptomLogsByPatientOnDate(string userId, DateTime date);
        Task<List<SymptomLog>> GetSymptomLogsByPatientInRange(string userId, DateTime startInclusive, DateTime endExclusive);
        Task AddSymptomLog(SymptomLog log);
        Task RemoveSymptomLog(SymptomLog log);

    }
    public class SympRepository : ISympRepository
    {
        private readonly AppDbContext _context;

        public SympRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SymptomLog> GetSymptomLogById(int id)
        {
            return await _context.SymptomLogs.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<SymptomLog>> GetSymptomLogsByPatientId(string userId)
        {
            return await _context.SymptomLogs.Where(a => a.PatientId == userId).OrderBy(a => a.Date).ToListAsync();
        }

        public async Task<List<SymptomLog>> GetSymptomLogsByPatientOnDate(string userId, DateTime date)
        {
            return await _context.SymptomLogs.Where(a => a.PatientId == userId && a.Date == date).OrderBy(a => a.Date).ToListAsync();
        }

        public async Task<List<SymptomLog>> GetSymptomLogsByPatientInRange(string userId, DateTime startInclusive, DateTime endExclusive)
        {
            return await _context.SymptomLogs
                .Where(l => l.PatientId == userId
                         && l.Date >= startInclusive
                         && l.Date < endExclusive)
                .ToListAsync();
        }

        public async Task AddSymptomLog(SymptomLog log)
        {
            _context.SymptomLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveSymptomLog(SymptomLog log)
        {
            _context.SymptomLogs.Remove(log);
            await _context.SaveChangesAsync();
        }

    }
}
