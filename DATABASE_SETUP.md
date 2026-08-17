# Database Setup Guide

## Overview
The driving school website now connects to a SQL Server database to persist user registration and profile data.

## What Was Added

### 1. **Database Model** (`Models/User.cs`)
- User entity with all registration fields
- Properties for lesson tracking
- Timestamps for creation/update
- Email uniqueness constraint

### 2. **Database Context** (`Data/ApplicationDbContext.cs`)
- Entity Framework DbContext
- Configured for SQL Server
- Email uniqueness validation

### 3. **Web API Controller** (`Controllers/AuthController.cs`)
- **POST `/api/auth/register`** - Register new user
- **POST `/api/auth/login`** - Authenticate user
- **GET `/api/auth/profile/{id}`** - Get user profile
- **PUT `/api/auth/profile/{id}`** - Update user profile
- Password hashing with SHA256
- All validation on backend

### 4. **Database Connection** (`Web.config`)
```xml
<connectionStrings>
  <add name="DrivingSchoolConnection" 
       connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=DrivingSchoolDb;Integrated Security=true;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### 5. **Web API Configuration** (`App_Start/WebApiConfig.cs`)
- Configured attribute-based routing
- Default API route template

### 6. **Application Startup** (`Global.asax.cs`)
- Registers Web API routes
- Initializes MVC and Filters

---

## Setup Steps

### Step 1: Verify SQL Server
Ensure SQL Server Express is installed and running:
```powershell
# Check if SQL Server is running
Get-Service -Name "MSSQL$SQLEXPRESS" | Start-Service
```

### Step 2: Create Database (Using Package Manager Console)
```
Enable-Migrations
Add-Migration InitialCreate
Update-Database
```

Or run this SQL script manually:
```sql
CREATE DATABASE DrivingSchoolDb;

USE DrivingSchoolDb;

CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Phone NVARCHAR(20) NOT NULL,
    DateOfBirth DATETIME NOT NULL,
    Address NVARCHAR(500) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    Postcode NVARCHAR(20) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    UpdatedAt DATETIME NULL,
    TotalLessons INT DEFAULT 0,
    CompletedLessons INT DEFAULT 0,
    RemainingLessons INT DEFAULT 0,
    TotalPaid DECIMAL(10,2) DEFAULT 0
);
```

### Step 3: Build and Run
1. Open the project in Visual Studio
2. Build the solution (`Ctrl + Shift + B`)
3. Run the application (`F5`)
4. Test registration at `http://localhost/auth.html`

---

## API Endpoints

### Register User
```
POST /api/auth/register
Content-Type: application/json

{
    "firstName": "John",
    "lastName": "Smith",
    "email": "john@example.com",
    "phone": "+27 123 456 7890",
    "dateOfBirth": "1990-05-15",
    "address": "123 Main Street",
    "city": "Johannesburg",
    "postcode": "2000",
    "password": "SecurePass123"
}
```

**Response (201 Created):**
```json
{
    "success": true,
    "message": "Registration successful",
    "userId": 1,
    "user": { ... }
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
    "phone": "+27 987 654 3210",
    "address": "456 Oak Avenue",
    "city": "Cape Town",
    "postcode": "8000"
}
```

---

## Data Flow

```
User Registration (auth.html)
        ↓
    auth.js (validate)
        ↓
POST /api/auth/register
        ↓
    AuthController (validate, hash password)
        ↓
    ApplicationDbContext (save to DB)
        ↓
    Response with user data
        ↓
    localStorage (store session)
        ↓
    Redirect to dashboard
```

---

## Security Features

✅ **Backend Validation**
- All inputs validated server-side
- Cannot be bypassed

✅ **Password Security**
- SHA256 hashing
- Hash stored in database, password never stored

✅ **Email Uniqueness**
- Database constraint prevents duplicates
- Validated both client and server-side

✅ **Database Isolation**
- Integrated Windows Authentication
- No credentials exposed

---

## Troubleshooting

### Issue: "Connection string name 'DrivingSchoolConnection' not found"
**Solution:** Verify connection string is in Web.config under `<connectionStrings>`

### Issue: "SQL Server not running"
**Solution:** 
```powershell
Get-Service -Name "MSSQL$SQLEXPRESS" | Start-Service
```

### Issue: "404 on /api/auth/register"
**Solution:** 
1. Verify WebApiConfig is registered in Global.asax.cs
2. Check AuthController is in Controllers folder
3. Rebuild solution

### Issue: "User already registered but email shows available"
**Solution:** Database constraint working - try with different email

---

## Files Modified/Created

| File | Type | Purpose |
|------|------|---------|
| `Models/User.cs` | ✅ Created | User entity model |
| `Data/ApplicationDbContext.cs` | ✅ Created | Entity Framework context |
| `Controllers/AuthController.cs` | ✅ Created | API endpoints |
| `App_Start/WebApiConfig.cs` | ✅ Created | Web API routing |
| `Global.asax.cs` | ✅ Created | Application startup |
| `Scripts/auth.js` | ✅ Updated | API calls instead of localStorage |
| `profile/profile.js` | ✅ Updated | API calls for profile |
| `Web.config` | ✅ Updated | Connection string added |

---

## Next Steps

1. ✅ Create database
2. ✅ Run application
3. ⏳ Test registration flow
4. ⏳ Test login
5. ⏳ Test profile update
6. ⏳ Deploy to production

---

**Status:** Ready for Testing  
**Last Updated:** 2026-08-16  
**Database:** SQL Server Express (Local)
