using System.ComponentModel.DataAnnotations;

namespace FYPIBDPatientApp.Dtos
{
    public class UpdateMedicationDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Notes { get; set; }
    }
}
