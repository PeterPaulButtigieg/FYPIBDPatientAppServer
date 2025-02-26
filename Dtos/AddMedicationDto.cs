using System.ComponentModel.DataAnnotations;

namespace FYPIBDPatientApp.Dtos
{
    public class AddMedicationDto
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Notes { get; set; }
    }
}
