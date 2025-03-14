using FYPIBDPatientApp.Models;

namespace FYPIBDPatientApp.Dtos
{
    public class AppointmentDto
    {
        public DateTime Date { get; set; }

        public string Venue { get; set; }

        public string AppointmentType { get; set; }

        public string Notes { get; set; }
    }
}
