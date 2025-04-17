using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
namespace MedicalCenter.Model
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Doctor>()
                .ToTable("Doctors");
            builder.Entity<Patient>()
            .ToTable("Patients");

            builder.Entity<Service>()
            .HasOne(s => s.Specialization)
            .WithMany(sp => sp.Services)
            .HasForeignKey(s => s.SpecializationId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DoctorSpecialization>()
                     .HasOne(ds => ds.Specialization)
                     .WithMany(s => s.DoctorSpecializations)
                     .HasForeignKey(ds => ds.SpecializationId)
                     .OnDelete(DeleteBehavior.Cascade);
        }


        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentStatus> AppointmentStatus { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorSpecialization> DoctorSpecialization { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<DoctorQualification> DoctorQualifications { get; set; }
        public DbSet<HospitalAffiliation> HospitalAffiliation { get; set; }
        public DbSet<MedicalCenterDoctorAvailability> MedicalCenterDoctorAvailability { get; set; }
        public DbSet<MedicalCenters> MedicalCenter { get; set; }
        public DbSet<PatientReview> PatientReviews { get; set; }
    }
}
