using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYPIBDPatientApp.Models
{
    public class Appointment
    {
        //[Key]
        public int Id { get; set; }

        public string PatientId { get; set; }
        //[ForeignKey(nameof(PatientId))]
        public ApplicationUser Patient { get; set; }

        public string HealthcareProfessionalId { get; set; }
        //[ForeignKey(nameof(HealthcareProfessionalId))]
        public ApplicationUser HealthcareProfessional { get; set; }

        public DateTime Date {  get; set; }

        public string Venue { get; set; }

        public string AppointmentType { get; set; }

        public string Notes { get; set; }
    }
}
