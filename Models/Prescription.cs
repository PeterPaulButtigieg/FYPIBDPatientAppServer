using System.ComponentModel.DataAnnotations.Schema;

namespace FYPIBDPatientApp.Models
{
    public class Prescription
    {
        public int Id { get; set; }

        public string PatientId { get; set; }
        //[ForeignKey(nameof(PatientId))]
        public ApplicationUser Patient { get; set; }

        public string HealthcareProfessionalId { get; set; }
        //[ForeignKey(nameof(HealthcareProfessionalId))]
        public ApplicationUser HealthcareProfessional { get; set; }

        public int MedicationId { get; set; }
        //[ForeignKey(nameof(MedicationId))]
        public Medication Medication { get; set; }

        public string Dosage { get; set; }

        public string Frequency { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Notes { get; set; }
    }
}
