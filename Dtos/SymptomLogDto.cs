namespace FYPIBDPatientApp.Dtos
{
    public class SymptomLogDto
    {
        public DateTime Date { get; set; }

        public string SymptomType { get; set; }

        public int Severity { get; set; }

        public string Notes { get; set; }
    }
}
