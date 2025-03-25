using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYPIBDPatientApp.Models
{
    public class Diagnosis
    {
        //[Key]
        public int Id { get; set; }

        public string PatientId { get; set; }
        //[ForeignKey(nameof(PatientId))]
        public ApplicationUser Patient { get; set; }

        public int DiagnosisCode { get; set; }

        public DateTime Date { get; set; }

        public string Notes { get; set; }
    }
}
