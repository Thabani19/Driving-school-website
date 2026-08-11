using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace TKDrivingSchool.Models
{

        // ============================================================
        // 1. USER
        // ============================================================
        public class User
        {
            [Key]
            public int UserID { get; set; }

            [Required]
            [EmailAddress]
            [StringLength(256)]
            [Index(IsUnique = true)]
            public string Email { get; set; }

            [Required]
            public string PasswordHash { get; set; }

            [Required]
            public string Role { get; set; }

            public bool IsActive { get; set; } = true;

            public DateTime CreatedAt { get; set; } = DateTime.Now;

            public DateTime? LastLoginDate { get; set; }

            // Navigation properties
            public virtual Student Student { get; set; }
            public virtual Instructor Instructor { get; set; }
            public virtual Administrator Administrator { get; set; }
            public virtual ICollection<Notification> Notifications { get; set; }
            public virtual ICollection<BookingChange> BookingChanges { get; set; }
        }

        // ============================================================
        // 2. STUDENT
        // ============================================================
        public class Student
        {
            [Key]
            [ForeignKey("User")]
            public int UserID { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Date of Birth")]
            public DateTime? DateOfBirth { get; set; }

            [Phone]
            public string Phone { get; set; }

            public string Address { get; set; }

            [Display(Name = "License Type")]
            public string LicenceType { get; set; }

            [Display(Name = "Registration Date")]
            public DateTime RegistrationDate { get; set; } = DateTime.Now;

            public int? TotalLessonsCompleted { get; set; } = 0;

            public int? TotalLessonsBooked { get; set; } = 0;

            // Navigation properties
            public virtual User User { get; set; }
            public virtual ICollection<Booking> Bookings { get; set; }
            public virtual ICollection<CourseEnrolment> CourseEnrolments { get; set; }
            public virtual ICollection<Review> Reviews { get; set; }
            public virtual ICollection<PaymentMethod> PaymentMethods { get; set; }
        }

        // ============================================================
        // 3. INSTRUCTOR
        // ============================================================
        public class Instructor
        {
            [Key]
            [ForeignKey("User")]
            public int UserID { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Phone]
            public string Phone { get; set; }

            [Display(Name = "License Type")]
            public string LicenceType { get; set; }

            [Display(Name = "Availability")]
            public string AvailabilityStatus { get; set; }

            public decimal? HourlyRate { get; set; }

            public int? YearsOfExperience { get; set; }

            [Display(Name = "Hire Date")]
            public DateTime? HireDate { get; set; }

            // Navigation properties
            public virtual User User { get; set; }
            public virtual ICollection<Schedule> Schedules { get; set; }
            public virtual ICollection<Review> Reviews { get; set; }
            public virtual ICollection<InstructorUnavailability> Unavailabilities { get; set; }
        }

        // ============================================================
        // 4. ADMINISTRATOR
        // ============================================================
        public class Administrator
        {
            [Key]
            [ForeignKey("User")]
            public int UserID { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            public string AdminLevel { get; set; }

            // Navigation property
            public virtual User User { get; set; }
        }

        // ============================================================
        // 5. COURSE
        // ============================================================
        public class Course
        {
            [Key]
            public int CourseID { get; set; }

            [Required]
            [Display(Name = "Course Name")]
            public string CourseName { get; set; }

            public string Description { get; set; }

            [Display(Name = "Number of Lessons")]
            public int NumberOfLessons { get; set; }

            [DataType(DataType.Currency)]
            public decimal Price { get; set; }

            [Display(Name = "Duration (weeks)")]
            public int Duration { get; set; }

            public bool IsActive { get; set; } = true;

            // Navigation properties
            public virtual ICollection<CourseEnrolment> CourseEnrolments { get; set; }
        }

        // ============================================================
        // 6. COURSE ENROLMENT
        // ============================================================
        public class CourseEnrolment
        {
            [Key]
            public int EnrolmentID { get; set; }

            [ForeignKey("Student")]
            public int StudentID { get; set; }

            [ForeignKey("Course")]
            public int CourseID { get; set; }

            [Display(Name = "Enrolment Date")]
            public DateTime EnrolmentDate { get; set; } = DateTime.Now;

            public string Status { get; set; }

            [Display(Name = "Completion Date")]
            public DateTime? CompletionDate { get; set; }

            public decimal? ProgressPercentage { get; set; }

            // Navigation properties
            public virtual Student Student { get; set; }
            public virtual Course Course { get; set; }
        }

        // ============================================================
        // 7. LESSON TYPE
        // ============================================================
        public class LessonType
        {
            [Key]
            public int LessonTypeID { get; set; }

            [Required]
            public string Name { get; set; }

            public string Description { get; set; }

            [Display(Name = "Duration (minutes)")]
            public int Duration { get; set; }

            [DataType(DataType.Currency)]
            public decimal Price { get; set; }

            public bool IsActive { get; set; } = true;

            // Navigation properties
            public virtual ICollection<Booking> Bookings { get; set; }
        }

        // ============================================================
        // 8. VEHICLE
        // ============================================================
        public class Vehicle
        {
            [Key]
            public int VehicleID { get; set; }

            [Required]
            [Display(Name = "Registration Number")]
            [StringLength(50)]
            [Index(IsUnique = true)]
            public string RegistrationNumber { get; set; }

            public string Make { get; set; }

            public string Model { get; set; }

            public int Year { get; set; }

            [Display(Name = "Vehicle Type")]
            public string VehicleType { get; set; }

            [Display(Name = "Availability")]
            public string AvailabilityStatus { get; set; } = "Available";

            public bool IsActive { get; set; } = true;

            public int? Mileage { get; set; }

            [Display(Name = "Last Service Date")]
            public DateTime? LastServiceDate { get; set; }

            [Display(Name = "Next Service Due")]
            public DateTime? NextServiceDue { get; set; }

            public string Color { get; set; }

            // Navigation properties
            public virtual ICollection<Schedule> Schedules { get; set; }
            public virtual ICollection<Review> Reviews { get; set; }
            public virtual ICollection<VehicleMaintenance> Maintenances { get; set; }
        }

        // ============================================================
        // 9. SCHEDULE
        // ============================================================
        public class Schedule
        {
            [Key]
            public int ScheduleID { get; set; }

            [ForeignKey("Instructor")]
            public int InstructorID { get; set; }

            [ForeignKey("Vehicle")]
            public int VehicleID { get; set; }

            [DataType(DataType.Date)]
            public DateTime Date { get; set; }

            [DataType(DataType.Time)]
            public TimeSpan StartTime { get; set; }

            [DataType(DataType.Time)]
            public TimeSpan EndTime { get; set; }

            public bool IsAvailable { get; set; } = true;

            public string Status { get; set; }

            // Navigation properties
            public virtual Instructor Instructor { get; set; }
            public virtual Vehicle Vehicle { get; set; }
            public virtual ICollection<Booking> Bookings { get; set; }
        }

        // ============================================================
        // 10. BOOKING
        // ============================================================
        public class Booking
        {
            [Key]
            public int BookingID { get; set; }

            [ForeignKey("Student")]
            public int StudentID { get; set; }

            [ForeignKey("Schedule")]
            public int ScheduleID { get; set; }

            [ForeignKey("LessonType")]
            public int LessonTypeID { get; set; }

            [Display(Name = "Booking Date")]
            public DateTime BookingDate { get; set; } = DateTime.Now;

            public string Status { get; set; }

            [Display(Name = "Payment Status")]
            public string PaymentStatus { get; set; }

            [Display(Name = "Booking Notes")]
            public string BookingNotes { get; set; }

            public DateTime CreatedAt { get; set; } = DateTime.Now;

            public DateTime? ConfirmedAt { get; set; }

            public DateTime? CancelledAt { get; set; }

            [Display(Name = "Cancellation Reason")]
            public string CancellationReason { get; set; }

            // Navigation properties
            public virtual Student Student { get; set; }
            public virtual Schedule Schedule { get; set; }
            public virtual LessonType LessonType { get; set; }
            public virtual Payment Payment { get; set; }
            public virtual Lesson Lesson { get; set; }
            public virtual ICollection<BookingChange> BookingChanges { get; set; }
            public virtual ICollection<Review> Reviews { get; set; }
        }

        // ============================================================
        // 11. LESSON
        // ============================================================
        public class Lesson
        {
            [Key]
            public int BookingID { get; set; }  // ← This is both PK and FK

            [Display(Name = "Lesson Date")]
            public DateTime LessonDate { get; set; }

            [Display(Name = "Attendance Status")]
            public string AttendanceStatus { get; set; }

            [Display(Name = "Lesson Status")]
            public string LessonStatus { get; set; }

            [Display(Name = "Instructor Notes")]
            public string InstructorNotes { get; set; }

            [Display(Name = "Progress Notes")]
            public string ProgressNotes { get; set; }

            [Display(Name = "Completed At")]
            public DateTime? CompletedAt { get; set; }

            public int? StudentRating { get; set; }

            // Navigation property
            public virtual Booking Booking { get; set; }
        }

        // ============================================================
        // 12. PAYMENT
        // ============================================================
        public class Payment
        {
            [Key]
            public int BookingID { get; set; }  // ← This is both PK and FK

            [DataType(DataType.Currency)]
            public decimal Amount { get; set; }

            [Display(Name = "Payment Method")]
            public string PaymentMethod { get; set; }

            [Display(Name = "Payment Date")]
            public DateTime PaymentDate { get; set; } = DateTime.Now;

            [Display(Name = "Payment Status")]
            public string PaymentStatus { get; set; }

            [Display(Name = "Transaction Reference")]
            public string TransactionReference { get; set; }

            public string PaymentGateway { get; set; }

            public DateTime? RefundDate { get; set; }

            [Display(Name = "Refund Reason")]
            public string RefundReason { get; set; }

            // Navigation property
            public virtual Booking Booking { get; set; }
        }

        // ============================================================
        // 13. PAYMENT METHOD
        // ============================================================
        public class PaymentMethod
        {
            [Key]
            public int PaymentMethodID { get; set; }

            [ForeignKey("Student")]
            public int StudentID { get; set; }

            [Display(Name = "Method Type")]
            public string MethodType { get; set; }

            public string Provider { get; set; }

            [Display(Name = "Account Number (masked)")]
            public string AccountNumberMasked { get; set; }

            public bool IsDefault { get; set; } = false;

            public bool IsActive { get; set; } = true;

            public DateTime CreatedAt { get; set; } = DateTime.Now;

            // Navigation property
            public virtual Student Student { get; set; }
        }

        // ============================================================
        // 14. REVIEW
        // ============================================================
        public class Review
        {
            [Key]
            public int ReviewID { get; set; }

            [ForeignKey("Student")]
            public int StudentID { get; set; }

            [ForeignKey("Booking")]
            public int BookingID { get; set; }

            [ForeignKey("Instructor")]
            public int? InstructorID { get; set; }

            [ForeignKey("Vehicle")]
            public int? VehicleID { get; set; }

            [Range(1, 5)]
            public int Rating { get; set; }

            [Display(Name = "Review Comment")]
            public string Comment { get; set; }

            [Display(Name = "Review Date")]
            public DateTime ReviewDate { get; set; } = DateTime.Now;

            [Display(Name = "Is Approved")]
            public bool IsApproved { get; set; } = false;

            public string ReviewType { get; set; }

            // Navigation properties
            public virtual Student Student { get; set; }
            public virtual Booking Booking { get; set; }
            public virtual Instructor Instructor { get; set; }
            public virtual Vehicle Vehicle { get; set; }
        }

        // ============================================================
        // 15. NOTIFICATION
        // ============================================================
        public class Notification
        {
            [Key]
            public int NotificationID { get; set; }

            [ForeignKey("User")]
            public int UserID { get; set; }

            [Required]
            public string Title { get; set; }

            [Required]
            public string Message { get; set; }

            [Display(Name = "Notification Type")]
            public string NotificationType { get; set; }

            [Display(Name = "Is Read")]
            public bool IsRead { get; set; } = false;

            [Display(Name = "Created At")]
            public DateTime CreatedAt { get; set; } = DateTime.Now;

            public DateTime? ReadAt { get; set; }

            public string Link { get; set; }

            // Navigation property
            public virtual User User { get; set; }
        }

        // ============================================================
        // 16. BOOKING CHANGE
        // ============================================================
        public class BookingChange
        {
            [Key]
            public int ChangeID { get; set; }

            [ForeignKey("Booking")]
            public int BookingID { get; set; }

            [ForeignKey("ChangedByUser")]
            public int ChangedByUserID { get; set; }

            public string ChangeType { get; set; }

            [Display(Name = "Old Schedule")]
            public int OldScheduleID { get; set; }

            [Display(Name = "New Schedule")]
            public int? NewScheduleID { get; set; }

            public string Reason { get; set; }

            [Display(Name = "Change Date")]
            public DateTime ChangeDate { get; set; } = DateTime.Now;

            // Navigation properties
            public virtual Booking Booking { get; set; }
            public virtual User ChangedByUser { get; set; }
        }

        // ============================================================
        // 17. INSTRUCTOR UNAVAILABILITY
        // ============================================================
        public class InstructorUnavailability
        {
            [Key]
            public int UnavailabilityID { get; set; }

            [ForeignKey("Instructor")]
            public int InstructorID { get; set; }

            [Display(Name = "Start Date")]
            public DateTime StartDate { get; set; }

            [Display(Name = "End Date")]
            public DateTime EndDate { get; set; }

            public string Reason { get; set; }

            [Display(Name = "Is Approved")]
            public bool IsApproved { get; set; } = false;

            // Navigation property
            public virtual Instructor Instructor { get; set; }
        }

        // ============================================================
        // 18. VEHICLE MAINTENANCE
        // ============================================================
        public class VehicleMaintenance
        {
            [Key]
            public int MaintenanceID { get; set; }

            [ForeignKey("Vehicle")]
            public int VehicleID { get; set; }

            [Display(Name = "Maintenance Date")]
            public DateTime MaintenanceDate { get; set; }

            public string Type { get; set; }

            public string Description { get; set; }

            [DataType(DataType.Currency)]
            public decimal Cost { get; set; }

            [Display(Name = "Next Service Date")]
            public DateTime? NextServiceDate { get; set; }

            [Display(Name = "Is Completed")]
            public bool IsCompleted { get; set; } = false;

            // Navigation property
            public virtual Vehicle Vehicle { get; set; }
        }

        // ============================================================
        // 19. AUDIT LOG
        // ============================================================
        public class AuditLog
        {
            [Key]
            public int AuditID { get; set; }

            [ForeignKey("User")]
            public int? UserID { get; set; }

            public string Action { get; set; }

            public string Entity { get; set; }

            public int? EntityID { get; set; }

            public string OldValues { get; set; }
            public string NewValues { get; set; }

            [Display(Name = "IP Address")]
            public string IPAddress { get; set; }

            public DateTime Timestamp { get; set; } = DateTime.Now;

            // Navigation property
            public virtual User User { get; set; }
        }

        // ============================================================
        // 20. REPORT
        // ============================================================
        public class Report
        {
            [Key]
            public int ReportID { get; set; }

            [Required]
            public string ReportName { get; set; }

            public string ReportType { get; set; }

            public string Parameters { get; set; }

            public string ResultData { get; set; }

            [Display(Name = "Generated By")]
            public int? GeneratedByUserID { get; set; }

            [Display(Name = "Generated At")]
            public DateTime GeneratedAt { get; set; } = DateTime.Now;

            [Display(Name = "File Path")]
            public string FilePath { get; set; }

            // Navigation property
            [ForeignKey("GeneratedByUserID")]
            public virtual User GeneratedBy { get; set; }
        }

        // ============================================================
        // DATABASE CONTEXT
        // ============================================================
        public class TKDrivingSchoolContext : DbContext
        {
            public TKDrivingSchoolContext()
                : base("name=TKDrivingSchoolContext")
            {
                this.Configuration.LazyLoadingEnabled = true;
                this.Configuration.AutoDetectChangesEnabled = true;
            }

            public DbSet<User> Users { get; set; }
            public DbSet<Student> Students { get; set; }
            public DbSet<Instructor> Instructors { get; set; }
            public DbSet<Administrator> Administrators { get; set; }
            public DbSet<Course> Courses { get; set; }
            public DbSet<CourseEnrolment> CourseEnrolments { get; set; }
            public DbSet<LessonType> LessonTypes { get; set; }
            public DbSet<Vehicle> Vehicles { get; set; }
            public DbSet<Schedule> Schedules { get; set; }
            public DbSet<Booking> Bookings { get; set; }
            public DbSet<Lesson> Lessons { get; set; }
            public DbSet<Payment> Payments { get; set; }
            public DbSet<PaymentMethod> PaymentMethods { get; set; }
            public DbSet<Review> Reviews { get; set; }
            public DbSet<Notification> Notifications { get; set; }
            public DbSet<BookingChange> BookingChanges { get; set; }
            public DbSet<InstructorUnavailability> InstructorUnavailabilities { get; set; }
            public DbSet<VehicleMaintenance> VehicleMaintenances { get; set; }
            public DbSet<AuditLog> AuditLogs { get; set; }
            public DbSet<Report> Reports { get; set; }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                // ============================================================
                // ONE-TO-ONE RELATIONSHIPS (SHARED PRIMARY KEY PATTERN)
                // ============================================================

                // User ↔ Student (UserID is both PK and FK)
                modelBuilder.Entity<User>()
                    .HasOptional(u => u.Student)
                    .WithRequired(s => s.User)
                    .WillCascadeOnDelete(true);

                // User ↔ Instructor (UserID is both PK and FK)
                modelBuilder.Entity<User>()
                    .HasOptional(u => u.Instructor)
                    .WithRequired(i => i.User)
                    .WillCascadeOnDelete(true);

                // User ↔ Administrator (UserID is both PK and FK)
                modelBuilder.Entity<User>()
                    .HasOptional(u => u.Administrator)
                    .WithRequired(a => a.User)
                    .WillCascadeOnDelete(true);

                // Booking ↔ Lesson (BookingID is both PK and FK)
                modelBuilder.Entity<Booking>()
                    .HasOptional(b => b.Lesson)
                    .WithRequired(l => l.Booking)
                    .WillCascadeOnDelete(false);

                // Booking ↔ Payment (BookingID is both PK and FK)
                modelBuilder.Entity<Booking>()
                    .HasOptional(b => b.Payment)
                    .WithRequired(p => p.Booking)
                    .WillCascadeOnDelete(false);

                // ============================================================
                // TABLE NAMES
                // ============================================================
                modelBuilder.Entity<User>().ToTable("Users");
                modelBuilder.Entity<Student>().ToTable("Students");
                modelBuilder.Entity<Instructor>().ToTable("Instructors");
                modelBuilder.Entity<Administrator>().ToTable("Administrators");

                // ============================================================
                // ONE-TO-MANY RELATIONSHIPS
                // ============================================================

                modelBuilder.Entity<Booking>()
                    .HasRequired(b => b.Student)
                    .WithMany(s => s.Bookings)
                    .HasForeignKey(b => b.StudentID)
                    .WillCascadeOnDelete(false);

                modelBuilder.Entity<Booking>()
                    .HasRequired(b => b.Schedule)
                    .WithMany(s => s.Bookings)
                    .HasForeignKey(b => b.ScheduleID)
                    .WillCascadeOnDelete(false);

                modelBuilder.Entity<Booking>()
                    .HasRequired(b => b.LessonType)
                    .WithMany(l => l.Bookings)
                    .HasForeignKey(b => b.LessonTypeID)
                    .WillCascadeOnDelete(false);

                modelBuilder.Entity<Review>()
                    .HasRequired(r => r.Student)
                    .WithMany(s => s.Reviews)
                    .HasForeignKey(r => r.StudentID)
                    .WillCascadeOnDelete(false);

                modelBuilder.Entity<Review>()
                    .HasRequired(r => r.Booking)
                    .WithMany(b => b.Reviews)
                    .HasForeignKey(r => r.BookingID)
                    .WillCascadeOnDelete(false);

                modelBuilder.Entity<Review>()
                    .HasOptional(r => r.Instructor)
                    .WithMany(i => i.Reviews)
                    .HasForeignKey(r => r.InstructorID)
                    .WillCascadeOnDelete(false);

                modelBuilder.Entity<Review>()
                    .HasOptional(r => r.Vehicle)
                    .WithMany(v => v.Reviews)
                    .HasForeignKey(r => r.VehicleID)
                    .WillCascadeOnDelete(false);

                modelBuilder.Entity<Schedule>()
                    .HasRequired(s => s.Instructor)
                    .WithMany(i => i.Schedules)
                    .HasForeignKey(s => s.InstructorID)
                    .WillCascadeOnDelete(false);

                modelBuilder.Entity<Schedule>()
                    .HasRequired(s => s.Vehicle)
                    .WithMany(v => v.Schedules)
                    .HasForeignKey(s => s.VehicleID)
                    .WillCascadeOnDelete(false);

                modelBuilder.Entity<BookingChange>()
                    .HasRequired(bc => bc.Booking)
                    .WithMany(b => b.BookingChanges)
                    .HasForeignKey(bc => bc.BookingID)
                    .WillCascadeOnDelete(false);

                modelBuilder.Entity<BookingChange>()
                    .HasRequired(bc => bc.ChangedByUser)
                    .WithMany(u => u.BookingChanges)
                    .HasForeignKey(bc => bc.ChangedByUserID)
                    .WillCascadeOnDelete(false);

                modelBuilder.Entity<CourseEnrolment>()
                    .HasRequired(ce => ce.Student)
                    .WithMany(s => s.CourseEnrolments)
                    .HasForeignKey(ce => ce.StudentID)
                    .WillCascadeOnDelete(false);

                modelBuilder.Entity<CourseEnrolment>()
                    .HasRequired(ce => ce.Course)
                    .WithMany(c => c.CourseEnrolments)
                    .HasForeignKey(ce => ce.CourseID)
                    .WillCascadeOnDelete(false);

                modelBuilder.Entity<Notification>()
                    .HasRequired(n => n.User)
                    .WithMany(u => u.Notifications)
                    .HasForeignKey(n => n.UserID)
                    .WillCascadeOnDelete(false);

                modelBuilder.Entity<InstructorUnavailability>()
                    .HasRequired(iu => iu.Instructor)
                    .WithMany(i => i.Unavailabilities)
                    .HasForeignKey(iu => iu.InstructorID)
                    .WillCascadeOnDelete(false);

                modelBuilder.Entity<VehicleMaintenance>()
                    .HasRequired(vm => vm.Vehicle)
                    .WithMany(v => v.Maintenances)
                    .HasForeignKey(vm => vm.VehicleID)
                    .WillCascadeOnDelete(false);

                modelBuilder.Entity<PaymentMethod>()
                    .HasRequired(pm => pm.Student)
                    .WithMany(s => s.PaymentMethods)
                    .HasForeignKey(pm => pm.StudentID)
                    .WillCascadeOnDelete(false);

                // ============================================================
                // DEFAULT VALUES (Auto-generated timestamps)
                // ============================================================
                modelBuilder.Entity<User>()
                    .Property(u => u.CreatedAt)
                    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

                modelBuilder.Entity<Student>()
                    .Property(s => s.RegistrationDate)
                    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

                modelBuilder.Entity<Booking>()
                    .Property(b => b.CreatedAt)
                    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

                modelBuilder.Entity<Payment>()
                    .Property(p => p.PaymentDate)
                    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

                modelBuilder.Entity<Notification>()
                    .Property(n => n.CreatedAt)
                    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

                modelBuilder.Entity<AuditLog>()
                    .Property(a => a.Timestamp)
                    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

                modelBuilder.Entity<Review>()
                    .Property(r => r.ReviewDate)
                    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

                modelBuilder.Entity<BookingChange>()
                    .Property(bc => bc.ChangeDate)
                    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            }
        }
    
}


