# Registration Troubleshooting Guide

## Step 1: Check Browser Console for Error Messages
1. Open your browser (Chrome/Firefox/Edge)
2. Navigate to `http://localhost/login.html`
3. Press **F12** to open Developer Tools
4. Go to **Console** tab
5. Try to register with test data
6. Look for error messages in red

---

## Step 2: Common Registration Errors & Solutions

### ❌ Error: "Registration failed"
**Possible Causes:**
1. **API endpoint not reachable** - Web API not running
2. **Database connection failed** - SQL Server not running
3. **Model validation error** - Invalid input data

**Solution:**
- Check Network tab in Developer Tools (F12)
- Look at the POST request to `/api/auth/register`
- Check the Response tab for server error details

---

### ❌ Error: "Email already registered"
**This is correct!** It means:
- Email validation is working
- Database connection is working
- Use a different email to register

---

### ❌ Error: "You must be at least 17 years old"
**This is correct!** It means:
- Age validation is working
- Use a birthdate that makes you 17+ years old

---

### ❌ Fetch Error / Network Error
**Possible Causes:**
1. Application not running
2. Port number wrong
3. CORS not enabled

**Solution:**
```
1. Build the solution in Visual Studio
2. Press F5 to run the application
3. Wait for it to start
4. Navigate to http://localhost/login.html (NOT http://127.0.0.1)
```

---

## Step 3: Verify Application is Running

### Check 1: Landing Page
- Navigate to `http://localhost/`
- You should see the TK Driving School landing page
- If NOT, the application isn't running

### Check 2: Web API
- Navigate to `http://localhost/api/auth/register`
- You should get an error (NOT a 404 Page Not Found)
- Error like "HTTP 405 Method Not Allowed" = API is there ✅
- 404 Not Found = API endpoint missing ❌

---

## Step 4: Database Connection Test

### Check if TKDRIVINGSCHOOLDB is accessible:
```sql
-- Run this in SQL Server Management Studio
USE TKDRIVINGSCHOOLDB;
SELECT COUNT(*) FROM Student;
SELECT COUNT(*) FROM UserAccount;
```

If you get an error:
1. Open SQL Server Management Studio
2. Connect to: `(local)\SQLEXPRESS` or `.\SQLEXPRESS`
3. Check if TKDRIVINGSCHOOLDB exists
4. If it doesn't exist, the database needs to be created

---

## Step 5: Detailed Debug Steps

### 5.1 Open Browser Console
Press **F12** and go to **Console** tab

### 5.2 Enable Verbose Logging
Add this to your browser console:
```javascript
// Copy and paste in browser console:
localStorage.setItem('debug', 'true');
```

### 5.3 Try Registration Again
Fill out the form with test data and click "Create Account"

### 5.4 Check Console Output
Look for:
- `Network Error` = Application not running
- `HTTP 500` = Server error (check Application logs)
- `HTTP 400` = Validation error (check error message)
- `HTTP 200` = Success! (but says failed?) = Check response parsing

---

## Step 6: Network Tab Debugging

1. Open Developer Tools (F12)
2. Go to **Network** tab
3. Try to register
4. Find the **POST** request to `/api/auth/register`
5. Click on it and check:

**Request Tab:**
- Method: `POST` ✅
- URL: `http://localhost/api/auth/register` ✅
- Headers include: `Content-Type: application/json` ✅
- Body has your form data ✅

**Response Tab:**
- Status: Should be `200` (success) or `400` (validation error)
- If `500`: Server error - check Application Event Viewer
- Response contains error message? ✅ = Use that message

---

## Step 7: If You See HTTP 500 Error

This means the server encountered an error. Check:

### 7.1 Visual Studio Output Window
1. Open Visual Studio
2. View → Output Window
3. Look for error messages when trying to register
4. Common errors:
   - "Invalid operation. The connection is not open." = Database not accessible
   - "The entity type Student is not part of the model..." = EF configuration issue
   - Other exceptions listed there

### 7.2 Event Viewer (Windows)
1. Open Event Viewer (Windows + X → Event Viewer)
2. Look in: Windows Logs → Application
3. Look for recent errors from ASP.NET or IIS

### 7.3 IIS Logs
1. If running in IIS, check: `C:\inetpub\logs\LogFiles`
2. Look for failed requests (status code 500)

---

## Step 8: Verify Web API Configuration

The following should be true:

✅ **WebApiConfig.cs**
```csharp
- config.MapHttpAttributeRoutes(); exists
- config.EnableCors(cors); is enabled
- Default route includes {action}: "api/{controller}/{action}/{id}"
```

✅ **Global.asax.cs**
```csharp
- GlobalConfiguration.Configure(WebApiConfig.Register); is called
- Before MVC routes are registered
```

✅ **AuthController.cs**
```csharp
- [RoutePrefix("api/auth")] on controller
- [Route("register")] on Register method
- [HttpPost] attribute on method
```

---

## Step 9: Test with Postman (Advanced)

If the browser isn't working, test with Postman:

### Install Postman
- Download from https://www.postman.com/downloads/

### Create Test Request
1. **Method:** POST
2. **URL:** `http://localhost/api/auth/register`
3. **Headers:** 
   - `Content-Type: application/json`
4. **Body (raw JSON):**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "phone": "0123456789",
  "dateOfBirth": "2000-01-15",
  "address": "123 Main Street",
  "city": "Johannesburg",
  "postcode": "2000",
  "password": "Test12345"
}
```
5. Click **Send**

### Check Response
- Status 200 = Success ✅
- Status 400 = Validation error (see error message)
- Status 500 = Server error (see error details)

---

## Most Common Issues & Fixes

| Issue | Cause | Fix |
|-------|-------|-----|
| "Cannot POST /api/auth/register" | Application not running | F5 in Visual Studio to start |
| HTTP 405 Method Not Allowed | Route configured for GET | Check [HttpPost] attribute |
| HTTP 404 Not Found | Wrong route | Check RoutePrefix and Route |
| HTTP 500 Internal Server Error | Database/code error | Check Output Window in VS |
| "Email already registered" | Email exists in DB | Use different email |
| "You must be at least 17" | Age < 17 | Use valid birthdate (17+) |
| CORS Error | Cross-origin blocked | Check WebApiConfig CORS |

---

## Quick Start Checklist

- [ ] Visual Studio is open with the project
- [ ] Project is built (no compilation errors)
- [ ] F5 is pressed to run the application
- [ ] Browser navigates to `http://localhost/`
- [ ] Landing page loads successfully
- [ ] Click "Sign in" button
- [ ] Login page loads with Register tab
- [ ] Fill form with valid test data
- [ ] Email is unique (hasn't been used before)
- [ ] Date of birth makes you 17+ years old
- [ ] Click "Create Account"
- [ ] Open browser Console (F12) to check for errors
- [ ] Check Network tab to see API response

---

## Getting Help

When reporting an error, provide:
1. Screenshot of the error message
2. Screenshot of browser console (F12 → Console)
3. Screenshot of Network tab showing POST request + response
4. The exact form data you entered
5. Steps to reproduce

---

**Status:** Troubleshooting guide for registration issues  
**Last Updated:** 2026-08-16  
**Application:** ASP.NET MVC 5 with Web API 2
