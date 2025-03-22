using FYPIBDPatientApp.Data;
using FYPIBDPatientApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FYPIBDPatientApp.Repositories
{
    public interface ISympRepository
    {
        Task<SymptomLog>GetSymptomLogById(int id);
        Task<List<SymptomLog>>GetSymptomLogsByPatientId(string userId);
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
