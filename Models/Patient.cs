using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace FYPIBDPatientApp.Models
{
    public class HealthcareProfessional
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        public string title { get; set; }

        public string specialisation {  get; set; }
    }
}
