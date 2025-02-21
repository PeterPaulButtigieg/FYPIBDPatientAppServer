namespace FYPIBDPatientApp.Models
{
    public class AuditLog
    {
        public string Id { get; set; }

        public string ApplicationUserId { get; set; }

        public string Action {  get; set; }

        public string TableName { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
