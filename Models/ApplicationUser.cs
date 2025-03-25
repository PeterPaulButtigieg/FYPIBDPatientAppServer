using Microsoft.AspNetCore.Identity;

namespace FYPIBDPatientApp.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender {  get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MobileNumber { get; set; }

        //Daily Logs
        public List<BowelMovementLog> BowelMovementLog { get; set; } = new List<BowelMovementLog>();
        public List<DietaryLog> DietaryLog { get; set; } = new List<DietaryLog>();
        public List<HydrationLog> HydrationLog { get; set; } = new List<HydrationLog>();
        public List<LifestyleLog> LifestyleLog { get; set; } = new List<LifestyleLog>();
        public List<SymptomLog> SymptomLog { get; set; } = new List<SymptomLog>();

        //Other stuff
        public List<Appointment> PatientAppointments { get; set; } = new List<Appointment>();
        public List<Prescription> RecievedPrescription { get; set;} = new List<Prescription>();
        public List<Diagnosis> PatientDiagnoses { get; set; } = new List<Diagnosis>();

        //Legal
        public List<AuditLog> AuditLog { get; set; } = new List<AuditLog>();
        public Concent Concent { get; set; }
        public bool isDeleted { get; set; } 

        //HealthCare Professionals, they can be sick too. Anyways Identity doesnt like separate user collections.
        public string? JobTitle { get; set; }
        public string? Specialisation {  get; set; }
    }
}
