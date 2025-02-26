using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYPIBDPatientApp.Models
{
    public class LabResult
    {
        //[Key]
        public int Id { get; set; }

        public int LabOrderId { get; set; }
        //[ForeignKey(nameof(LabOrderId))]
        public LabOrder LabOrder { get; set; }

        public string ResultDetails { get; set; }

        public DateTime Date { get; set; }

        public string Notes { get; set; }
    }
}
