namespace FYPIBDPatientApp.Dtos
{
    public class PrescriptionDto
    {
        public int Id { get; set; }

        public string Medication { get; set; }

        public int Interval { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Notes { get; set; }
    }
}
