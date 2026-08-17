using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using DrivingSchoolLandingPage.Data;
using DrivingSchoolLandingPage.Models;
using System.Security.Cryptography;
using System.Text;

namespace DrivingSchoolLandingPage.Controllers
{
    [RoutePrefix("api/auth")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuthController : ApiController
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost]
        [Route("register")]
        public IHttpActionResult Register([FromBody] dynamic userData)
        {
            try
            {
                string firstName = (userData?.firstName ?? string.Empty).ToString()?.Trim();
                string lastName = (userData?.lastName ?? string.Empty).ToString()?.Trim();
                string email = (userData?.email ?? string.Empty).ToString()?.Trim();
                string phone = (userData?.phone ?? string.Empty).ToString()?.Trim();
                string dateOfBirthStr = (userData?.dateOfBirth ?? string.Empty).ToString()?.Trim();
                string address = (userData?.address ?? string.Empty).ToString()?.Trim();
                string city = (userData?.city ?? string.Empty).ToString()?.Trim();
                string postcode = (userData?.postcode ?? string.Empty).ToString()?.Trim();
                string password = (userData?.password ?? string.Empty).ToString();
                string confirmPassword = (userData?.confirmPassword ?? string.Empty).ToString();

                if (string.IsNullOrWhiteSpace(firstName))
                    return BadRequest(new { message = "First name is required" });

                if (string.IsNullOrWhiteSpace(lastName))
                    return BadRequest(new { message = "Last name is required" });

                if (string.IsNullOrWhiteSpace(email))
                    return BadRequest(new { message = "Email is required" });

                email = email.ToLowerInvariant();

                if (string.IsNullOrWhiteSpace(phone))
                    return BadRequest(new { message = "Phone number is required" });

                if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                    return BadRequest(new { message = "Password must be at least 6 characters" });

                if (!string.IsNullOrWhiteSpace(confirmPassword) && password != confirmPassword)
                    return BadRequest(new { message = "Passwords do not match" });

                if (_db.Students.Any(u => u.Email.ToLower() == email))
                    return BadRequest(new { message = "Email already registered" });

                if (!DateTime.TryParse(dateOfBirthStr, out DateTime dob))
                    return BadRequest(new { message = "Invalid date of birth" });

                var age = DateTime.Now.Year - dob.Year;
                if (dob > DateTime.Now.AddYears(-age))
                    age--;

                if (age < 17)
                    return BadRequest(new { message = "You must be at least 17 years old" });

                if (string.IsNullOrWhiteSpace(address))
                    return BadRequest(new { message = "Address is required" });

                var student = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Phone = phone,
                    DateOfBirth = dob,
                    Address = address,
                    Gender = "Not Specified",
                    CreatedAt = DateTime.Now
                };

                _db.Students.Add(student);
                _db.SaveChanges();

                var existingAccount = _db.UserAccounts.FirstOrDefault(u => u.Username.ToLower() == email);
                if (existingAccount != null)
                {
                    _db.Students.Remove(student);
                    _db.SaveChanges();
                    return BadRequest(new { message = "Email already registered" });
                }

                var userAccount = new UserAccount
                {
                    Username = email,
                    Password = HashPassword(password),
                    Role = "Student",
                    StudentId = student.Id
                };

                _db.UserAccounts.Add(userAccount);
                _db.SaveChanges();

                return Ok(new
                {
                    success = true,
                    message = "Registration successful",
                    userId = student.Id,
                    user = new
                    {
                        id = student.Id,
                        firstName = student.FirstName,
                        lastName = student.LastName,
                        email = student.Email,
                        phone = student.Phone,
                        dateOfBirth = student.DateOfBirth.ToString("yyyy-MM-dd"),
                        address = student.Address,
                        city = city,
                        postcode = postcode
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Registration Error: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Stack Trace: " + ex.StackTrace);
                return InternalServerError(new { message = "Registration failed: " + ex.Message });
            }
        }

        /// <summary>
        /// User login
        /// </summary>
        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] dynamic credentials)
        {
            try
            {
                string email = credentials?.email;
                string password = credentials?.password;

                System.Diagnostics.Debug.WriteLine($"Login attempt for email: {email}");

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                    return BadRequest("Email and password are required");

                // Find student by email
                var student = _db.Students.FirstOrDefault(u => u.Email == email);

                if (student == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Student not found for email: {email}");
                    return BadRequest("Invalid email or password");
                }

                System.Diagnostics.Debug.WriteLine($"Student found: {student.FirstName} {student.LastName} (ID: {student.Id})");

                // Find user account
                var userAccount = _db.UserAccounts.FirstOrDefault(u => u.StudentId == student.Id);

                if (userAccount == null)
                {
                    System.Diagnostics.Debug.WriteLine($"UserAccount not found for StudentId: {student.Id}");
                    return BadRequest("Invalid email or password");
                }

                System.Diagnostics.Debug.WriteLine($"UserAccount found: {userAccount.Username}");

                // Verify password
                bool passwordValid = VerifyPassword(password, userAccount.Password);
                System.Diagnostics.Debug.WriteLine($"Password verification result: {passwordValid}");

                if (!passwordValid)
                {
                    System.Diagnostics.Debug.WriteLine("Password verification failed");
                    return BadRequest("Invalid email or password");
                }

                return Ok(new
                {
                    success = true,
                    message = "Login successful",
                    user = new
                    {
                        id = student.Id,
                        firstName = student.FirstName,
                        lastName = student.LastName,
                        email = student.Email,
                        phone = student.Phone,
                        dateOfBirth = student.DateOfBirth.ToString("yyyy-MM-dd"),
                        address = student.Address,
                        gender = student.Gender,
                        registrationDate = student.CreatedAt.ToString("yyyy-MM-dd"),
                        totalLessons = 0,
                        completedLessons = 0,
                        remainingLessons = 0,
                        totalPaid = 0
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Login Error: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Stack Trace: " + ex.StackTrace);
                return InternalServerError(new Exception("Login failed: " + ex.Message));
            }
        }

        /// <summary>
        /// Get user profile
        /// </summary>
        [HttpGet]
        [Route("profile/{id}")]
        public IHttpActionResult GetProfile(int id)
        {
            try
            {
                var student = _db.Students.FirstOrDefault(u => u.Id == id);

                if (student == null)
                    return NotFound();

                return Ok(new
                {
                    success = true,
                    user = new
                    {
                        id = student.Id,
                        firstName = student.FirstName,
                        lastName = student.LastName,
                        email = student.Email,
                        phone = student.Phone,
                        dateOfBirth = student.DateOfBirth.ToString("yyyy-MM-dd"),
                        address = student.Address,
                        gender = student.Gender,
                        registrationDate = student.CreatedAt.ToString("yyyy-MM-dd"),
                        totalLessons = 0,
                        completedLessons = 0,
                        remainingLessons = 0,
                        totalPaid = 0
                    }
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        [HttpPut]
        [Route("profile/{id}")]
        public IHttpActionResult UpdateProfile(int id, [FromBody] dynamic updatedData)
        {
            try
            {
                var student = _db.Students.FirstOrDefault(u => u.Id == id);

                if (student == null)
                    return NotFound();

                // Update allowed fields
                if (!string.IsNullOrWhiteSpace(updatedData?.firstName))
                    student.FirstName = updatedData.firstName;

                if (!string.IsNullOrWhiteSpace(updatedData?.lastName))
                    student.LastName = updatedData.lastName;

                if (!string.IsNullOrWhiteSpace(updatedData?.phone))
                    student.Phone = updatedData.phone;

                if (!string.IsNullOrWhiteSpace(updatedData?.address))
                    student.Address = updatedData.address;

                if (!string.IsNullOrWhiteSpace(updatedData?.gender))
                    student.Gender = updatedData.gender;

                _db.SaveChanges();

                // Update password if provided
                if (!string.IsNullOrWhiteSpace(updatedData?.password))
                {
                    if (updatedData.password.Length < 6)
                        return BadRequest("Password must be at least 6 characters");

                    var userAccount = _db.UserAccounts.FirstOrDefault(u => u.StudentId == id);
                    if (userAccount != null)
                    {
                        userAccount.Password = HashPassword(updatedData.password);
                        _db.SaveChanges();
                    }
                }

                return Ok(new
                {
                    success = true,
                    message = "Profile updated successfully",
                    user = new
                    {
                        id = student.Id,
                        firstName = student.FirstName,
                        lastName = student.LastName,
                        email = student.Email,
                        phone = student.Phone,
                        dateOfBirth = student.DateOfBirth.ToString("yyyy-MM-dd"),
                        address = student.Address,
                        gender = student.Gender,
                        registrationDate = student.CreatedAt.ToString("yyyy-MM-dd")
                    }
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Hash password using SHA256
        /// </summary>
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        /// <summary>
        /// Verify password
        /// </summary>
        private bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hash;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
