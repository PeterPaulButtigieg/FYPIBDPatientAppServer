﻿using System.ComponentModel.DataAnnotations.Schema;

namespace FYPIBDPatientApp.Models
{
    public class Prescription
    {
        public int Id { get; set; }
  
        public string PatientId { get; set; }
        //[ForeignKey(nameof(PatientId))]
        public ApplicationUser Patient { get; set; }
        
        public string Medication { get; set; }

        public int Interval { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Frequency { get; set; }
    }
}
