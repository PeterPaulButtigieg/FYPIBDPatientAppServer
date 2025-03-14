using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYPIBDPatientApp.Models
{
    public class BowelMovementLog
    {
        //[Key]
        public int Id { get; set; }

        public string PatientId { get; set; }
        //[ForeignKey(nameof(PatientId))]
        public ApplicationUser Patient { get; set; }

        public DateTime Date { get; set; }

        public string StoolType { get; set; }

        public bool Blood { get; set; }

        public string Urgency { get; set; }

        public string Notes { get; set; }
    }
}
