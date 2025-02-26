using System.ComponentModel.DataAnnotations.Schema;

namespace FYPIBDPatientApp.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        //[ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        public string Action {  get; set; }

        public DateTime Timestamp { get; set; }
    }
}
