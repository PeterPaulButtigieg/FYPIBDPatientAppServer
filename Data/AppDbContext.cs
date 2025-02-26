using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Composition;
using FYPIBDPatientApp.Models;

namespace FYPIBDPatientApp.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Concent> Concents { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<DietaryLog> DietaryLogs { get; set; }
        public DbSet<HydrationLog> HydrationLogs { get; set; }
        public DbSet<LabOrder> LabOrders { get; set; }
        public DbSet<LabResult> LabResults { get; set; }
        public DbSet<LifestyleLog> LifestyleLogs { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Prescription> PatientMedications { get; set; }
        public DbSet<SymptomLog> SymptomLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Appointments
            builder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(u => u.PatientAppointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<Appointment>()
            .HasOne(a => a.HealthcareProfessional)
            .WithMany(u => u.ProfessionalAppointments)
            .HasForeignKey(a => a.HealthcareProfessionalId)
            .OnDelete(DeleteBehavior.Restrict);

            //Diagnoses
            builder.Entity<Diagnosis>()
            .HasOne(d => d.Patient)
            .WithMany(u => u.PatientDiagnoses)
            .HasForeignKey(d => d.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Diagnosis>()
            .HasOne(d => d.HealthcareProfessional)
            .WithMany(u => u.ProfessionalDiagnoses)
            .HasForeignKey(d => d.HealthcareProfessionalId)
            .OnDelete(DeleteBehavior.Restrict);

            //Prescriptions
            builder.Entity<Prescription>()
            .HasOne(d => d.Patient)
            .WithMany(u => u.RecievedPrescription)
            .HasForeignKey(d => d.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Prescription>()
            .HasOne(d => d.HealthcareProfessional)
            .WithMany(u => u.IssuedPrescription)
            .HasForeignKey(d => d.HealthcareProfessionalId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
