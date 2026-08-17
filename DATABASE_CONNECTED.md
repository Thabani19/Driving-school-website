# Database Connection Guide - TKDRIVINGSCHOOLDB

## Overview
Your registration and profile system is now connected to the existing **TKDRIVINGSCHOOLDB** database.

## Database Tables Used

### 1. **Student Table**
Stores student/user personal information:
- StudentID (Primary Key)
- FirstName, LastName
- Email (Unique)
- Phone, Gender
- DateOfBirth, Address
- RegistrationDate

### 2. **UserAccount Table**
Stores authentication credentials:
- UserID (Primary Key)
- Username (Email)
- Password (SHA256 Hashed)
- Role (Student, Instructor, Admin)
- StudentID (Foreign Key to Student)
- InstructorID (Foreign Key to Instructor)

## How Registration Works

```
User Fills Registration Form
        ↓
Validation (Client + Server)
        ↓
POST /api/auth/register
        ↓
1. Create Student record (FirstName, LastName, Email, Phone, DOB, Address)
2. Create UserAccount record (linked to Student, Password hashed)
        ↓
Save to TKDRIVINGSCHOOLDB
        ↓
Return success with StudentID
        ↓
Store in localStorage (session)
        ↓
Redirect to Dashboard
```

## How Login Works

```
User Enters Email & Password
        ↓
POST /api/auth/login
        ↓
Query Student table by Email
        ↓
Find linked UserAccount
        ↓
Verify hashed password
        ↓
Return Student data if match
        ↓
Store in localStorage (session)
        ↓
Redirect to Dashboard
```

## How Profile Updates Work

```
User Edits Profile
        ↓
PUT /api/auth/profile/{StudentID}
        ↓
Update Student record (Name, Phone, Address, Gender)
        ↓
Update UserAccount password if changed
        ↓
Save to TKDRIVINGSCHOOLDB
        ↓
Return updated Student data
```

## Connection String
```xml
<add name="DrivingSchoolConnection" 
     connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=TKDRIVINGSCHOOLDB;Integrated Security=true;" 
     providerName="System.Data.SqlClient" />
```

**Key Points:**
- ✅ Uses existing TKDRIVINGSCHOOLDB
- ✅ Local SQL Server Express
- ✅ Windows Authentication (no password needed)

## Database Schema Integration

### Entity Framework Mappings

**User Class → Student Table**
```
User.Id           → StudentID
User.FirstName    → FirstName
User.LastName     → LastName
User.Email        → Email
User.Phone        → Phone
User.DateOfBirth  → DateOfBirth
User.Address      → Address
User.Gender       → Gender
User.CreatedAt    → RegistrationDate
```

**UserAccount Class → UserAccount Table**
```
UserAccount.Id        → UserID
UserAccount.Username  → Username (stored as Email)
UserAccount.Password  → Password (SHA256 Hashed)
UserAccount.Role      → Role (set to "Student")
UserAccount.StudentId → StudentID (Foreign Key)
```

## Security Implementation

### Password Storage
✅ Never stored in Student table
✅ Hashed with SHA256 in UserAccount table
✅ Verified on login using hashing

### Data Validation
✅ Server-side validation on all inputs
✅ Email uniqueness enforced
✅ Age verification (minimum 17)
✅ Password minimum length (6 characters)

### Database Constraints
✅ Email UNIQUE in Student table
✅ Foreign Key: UserAccount.StudentID → Student.StudentID
✅ Integrated Security (no stored credentials)

## API Endpoints

### Register Student
```
POST /api/auth/register
Content-Type: application/json

{
    "firstName": "John",
    "lastName": "Smith",
    "email": "john@example.com",
    "phone": "0123456789",
    "dateOfBirth": "1990-05-15",
    "address": "123 Main Street",
    "city": "Johannesburg",
    "postcode": "2000",
    "password": "SecurePass123"
}
```

**Response (Success):**
```json
{
    "success": true,
    "message": "Registration successful",
    "userId": 1,
    "user": {
        "id": 1,
        "firstName": "John",
        "lastName": "Smith",
        "email": "john@example.com",
        "phone": "0123456789",
        "dateOfBirth": "1990-05-15",
        "address": "123 Main Street",
        "city": "Johannesburg",
        "postcode": "2000"
    }
}
```

### Login
```
POST /api/auth/login
Content-Type: application/json

{
    "email": "john@example.com",
    "password": "SecurePass123"
}
```

**Response (Success):**
```json
{
    "success": true,
    "message": "Login successful",
    "user": {
        "id": 1,
        "firstName": "John",
        "lastName": "Smith",
        "email": "john@example.com",
        ...
    }
}
```

### Get Profile
```
GET /api/auth/profile/1
```

### Update Profile
```
PUT /api/auth/profile/1
Content-Type: application/json

{
    "firstName": "Jonathan",
    "lastName": "Smith",
    "phone": "0987654321",
    "address": "456 New Street",
    "gender": "Male"
}
```

## Next Steps

### Setup
1. ✅ Database schema already created (SQL_CODE101)
2. ✅ Connection string configured
3. ✅ Models mapped to existing tables
4. Build and run the project

### Testing
1. Navigate to `http://localhost/auth.html`
2. Register a new student
   - Data will be saved to Student table
   - Password will be hashed and saved to UserAccount table
3. Login with registered email
   - Queries Student and UserAccount tables
   - Verifies password hash
4. View and update profile
   - Updates Student table
   - Password changes update UserAccount

### Verification
Check database:
```sql
-- View registered students
SELECT * FROM Student;

-- View user accounts
SELECT UserID, Username, Role, StudentId FROM UserAccount;

-- View student with their account
SELECT s.StudentID, s.FirstName, s.LastName, s.Email, ua.Role
FROM Student s
LEFT JOIN UserAccount ua ON s.StudentID = ua.StudentID;
```

## Important Notes

### Fields Not Used (Yet)
The following tables exist but are not used by registration/profile:
- Instructor
- Vehicle
- Course
- Enrollment
- Lesson
- Attendance
- Result
- Payment
- Announcement

These can be integrated for lesson booking, instructor assignment, payments, etc.

### Future Integration
This registration system provides the foundation for:
- Lesson enrollment (via Enrollment table)
- Instructor assignment (via Lesson table)
- Payment tracking (via Payment table)
- Result recording (via Result table)
- Attendance monitoring (via Attendance table)

## Files Modified

| File | Changes |
|------|---------|
| `Web.config` | Updated connection string to TKDRIVINGSCHOOLDB |
| `Models/User.cs` | Mapped to Student table, added UserAccount model |
| `Data/ApplicationDbContext.cs` | Configured for existing database schema |
| `Controllers/AuthController.cs` | Uses Student & UserAccount tables |

## Troubleshooting

### Issue: "Invalid object name 'Student'"
**Solution:** Ensure SQL Server is running and database is created
```sql
SELECT * FROM TKDRIVINGSCHOOLDB.INFORMATION_SCHEMA.TABLES;
```

### Issue: "Foreign key constraint failed"
**Solution:** StudentID must exist in Student table before creating UserAccount
(Handled automatically in registration)

### Issue: "Login returns Unauthorized"
**Solution:** 
1. Verify student was registered (check Student table)
2. Verify UserAccount was created (check UserAccount table)
3. Check password hash is stored (not plain text)

---

**Status:** Connected to TKDRIVINGSCHOOLDB  
**Last Updated:** 2026-08-16  
**Database:** SQL Server Express (Local)
