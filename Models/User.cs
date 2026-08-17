using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrivingSchoolLandingPage.Models
{
    [Table("Student")]
    public class User
    {
        [Key]
        [Column("StudentID")]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        [Column("DateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(15)]
        public string Phone { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [Column("RegistrationDate")]
        public DateTime CreatedAt { get; set; }

        // Additional fields not in original Student table (for authentication)
        [NotMapped]
        public string Password { get; set; }

        [NotMapped]
        public string City { get; set; }

        [NotMapped]
        public string Postcode { get; set; }

        // Lesson tracking
        [NotMapped]
        public int TotalLessons { get; set; } = 0;

        [NotMapped]
        public int CompletedLessons { get; set; } = 0;

        [NotMapped]
        public int RemainingLessons { get; set; } = 0;

        [NotMapped]
        public decimal TotalPaid { get; set; } = 0;

        public User()
        {
            CreatedAt = DateTime.Now;
        }
    }

    [Table("UserAccount")]
    public class UserAccount
    {
        [Key]
        [Column("UserID")]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; }

        [Column("StudentID")]
        public int? StudentId { get; set; }

        [Column("InstructorID")]
        public int? InstructorId { get; set; }
    }
}
