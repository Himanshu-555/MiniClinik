using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mini_Clinic_Appointment_System.Helper;

namespace Mini_Clinic_Appointment_System.Data
{
    public class MiniClinikContext(DbContextOptions<MiniClinikContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<ApplicationUser>().
            //    HasOne(u=>u.)
            //    .WithOne(p => p.applicationUser)
            //    .HasForeignKey<Patient>(p => p.UserId);
        }
    }
}
