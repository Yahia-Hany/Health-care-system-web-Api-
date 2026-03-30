using Health_care_system__web_Api_.Entities;
using Microsoft.EntityFrameworkCore;

namespace Health_care_system__web_Api_.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) :base (options) 
        { 
        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>()
                .HasKey(AP => new { AP.PatientId, AP.DoctorId });
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { Id = 5, Name = "Dr. Ahmed", Specialization = "Cardiology" },
                new Doctor { Id = 6, Name = "Dr. Ali", Specialization = "Dentist" }
                );
            modelBuilder.Entity<Patient>().HasData(
                new Patient { Id = 4, Name = "Omar", BirthDate = new DateTime(2000, 1, 1) },
                new Patient { Id = 5, Name = "mahmoud", BirthDate = new DateTime(1998, 5, 10) }
                );
            modelBuilder.Entity<Appointment>().HasData(
                new Appointment
                {
                    DoctorId = 5,
                    PatientId = 5,
                    AppointmentDate = new DateTime(2026, 1, 1)
                },
                new Appointment
                {
                    DoctorId = 6,
                    PatientId = 4,
                    AppointmentDate = new DateTime(2026, 3, 1)
                }
                );
        }
  
    }
}
