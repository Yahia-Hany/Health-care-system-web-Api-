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
        }
    }
}
