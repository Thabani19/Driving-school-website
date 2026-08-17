using System.Data.Entity;
using DrivingSchoolLandingPage.Models;

namespace DrivingSchoolLandingPage.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("name=DrivingSchoolConnection")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        // Existing tables
        public DbSet<User> Students { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Map User to Student table
            modelBuilder.Entity<User>()
                .ToTable("Student");

            // Map UserAccount to UserAccount table
            modelBuilder.Entity<UserAccount>()
                .ToTable("UserAccount");

            // Email uniqueness for Student table
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasColumnName("Email");

            base.OnModelCreating(modelBuilder);
        }
    }
}
