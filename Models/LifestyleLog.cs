using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYPIBDPatientApp.Models
{
    public class LifestyleLog
    {
        //[Key]
        public int Id { get; set; }

        public string PatientId { get; set; }
        //[ForeignKey(nameof(PatientId))]
        public ApplicationUser Patient { get; set; }

        public DateTime Date { get; set; }

        public int StressLevel { get; set; }

        public TimeSpan SleepDuration { get; set; }

        public TimeSpan Exercise {  get; set; }

        public string Notes { get; set; }
    }
}
