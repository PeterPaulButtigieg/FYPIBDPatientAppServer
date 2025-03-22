using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYPIBDPatientApp.Models
{
    public class SymptomLog
    {
        //[Key]
        public int Id { get; set; }

        public string PatientId { get; set; }
        //[ForeignKey(nameof(PatientId))]
        public ApplicationUser Patient { get; set; }

        public DateTime Date {  get; set; }

        public string SymptomType { get; set; }

        public int Severity { get; set; }

        public string Notes { get; set; }
    }
}
