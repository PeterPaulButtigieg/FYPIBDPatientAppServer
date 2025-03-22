using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Composition;
using FYPIBDPatientApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;

namespace FYPIBDPatientApp.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<BowelMovementLog> BowelMovementLogs { get; set; }
        public DbSet<Concent> Concents { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<DietaryLog> DietaryLogs { get; set; }
        public DbSet<HydrationLog> HydrationLogs { get; set; }
        public DbSet<LifestyleLog> LifestyleLogs { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<SymptomLog> SymptomLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = "F0C7924B-3C4E-4C5F-B8F5-1EE0D65D422F",
                    Name = "PATIENT",
                    ConcurrencyStamp = "1",
                    NormalizedName = "PATIENT"
                },

                new IdentityRole()
                {
                    Id = "2E1B51E9-1B52-424D-B4DA-07AFA32FD9DD",
                    Name = "HCPRO",
                    ConcurrencyStamp = "2",
                    NormalizedName = "HCPRO"
                }
            );

            //Seeding a HealthCareProfessional(Admin)
            ApplicationUser user = new ApplicationUser()
            {
                Id = "134c1566-3f64-4ab4-b1e7-2ffe11f43e32",
                UserName = "drsmith@gmail.com",
                NormalizedUserName = "DRSMITH@GMAIL.COM",
                Email = "drsmith@gmail.com",
                NormalizedEmail = "DRSMITH@GMAIL.COM",
                FirstName = "John",
                LastName = "Smith",
                Gender = "M",
                DateOfBirth = new DateTime(1988, 2, 26),
                LockoutEnabled = false,
                EmailConfirmed = true,
                MobileNumber = "23778888",
            };

            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "password");
            builder.Entity<ApplicationUser>().HasData(user);

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "2E1B51E9-1B52-424D-B4DA-07AFA32FD9DD",
                    UserId = "134c1566-3f64-4ab4-b1e7-2ffe11f43e32"
                }
            );

            //Appointments
            builder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(u => u.PatientAppointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Cascade); 

            /*builder.Entity<Appointment>()
            .HasOne(a => a.HealthcareProfessional)
            .WithMany(u => u.ProfessionalAppointments)
            .HasForeignKey(a => a.HealthcareProfessionalId)
            .OnDelete(DeleteBehavior.Restrict);*/

            //Diagnoses
            builder.Entity<Diagnosis>()
            .HasOne(d => d.Patient)
            .WithMany(u => u.PatientDiagnoses)
            .HasForeignKey(d => d.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

            /*builder.Entity<Diagnosis>()
            .HasOne(d => d.HealthcareProfessional)
            .WithMany(u => u.ProfessionalDiagnoses)
            .HasForeignKey(d => d.HealthcareProfessionalId)
            .OnDelete(DeleteBehavior.Restrict);*/

            //Prescriptions
            builder.Entity<Prescription>()
            .HasOne(d => d.Patient)
            .WithMany(u => u.RecievedPrescription)
            .HasForeignKey(d => d.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

            /*builder.Entity<Prescription>()
            .HasOne(d => d.HealthcareProfessional)
            .WithMany(u => u.IssuedPrescription)
            .HasForeignKey(d => d.HealthcareProfessionalId)
            .OnDelete(DeleteBehavior.Restrict);*/
        }
    }
}
