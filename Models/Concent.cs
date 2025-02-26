using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYPIBDPatientApp.Models
{
    public class Concent
    {
        //[Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        //[ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        public bool Granted { get; set; }

        public DateTime DateGranted { get; set; }

        public DateTime DateRevoked { get; set; }
    }
}
