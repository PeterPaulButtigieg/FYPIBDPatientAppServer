namespace FYPIBDPatientApp.Models
{
    public class Medication
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; } 

        public string Notes { get; set; }

        public List<Prescription> PatientMedication { get; set;} = new List<Prescription>();
    }
}
