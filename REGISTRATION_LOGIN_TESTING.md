# Registration & Login Testing Guide

## ✅ Complete Setup Checklist

### Step 1: Verify Visual Studio Configuration
- [ ] Open the project in Visual Studio
- [ ] Right-click Project → Properties
- [ ] Verify Target Framework is **.NET Framework 4.7.2**
- [ ] Check that no build errors exist (Build → Build Solution)

### Step 2: Start the Application
- [ ] Press **F5** to run the application in Debug mode
- [ ] Wait for browser to open automatically
- [ ] You should see the **TK Driving School landing page**
- [ ] If not, navigate to `http://localhost/`

### Step 3: Test Registration

#### 3.1 Navigate to Login Page
- Click **"Sign in"** button or navigate to `http://localhost/login.html`

#### 3.2 Open Browser Console (CRITICAL!)
- Press **F12** to open Developer Tools
- Click **Console** tab
- You'll see debug messages here (🔵 🟢 ❌)

#### 3.3 Fill Registration Form
Use this test data (you can change the email each time):
```
First Name:    John
Last Name:     Doe
Email:         john123@example.com (UNIQUE - change number each time)
Phone:         0123456789
Date of Birth: 2000-01-15 (must be 17+ years old)
Address:       123 Main Street
City:          Johannesburg
Postcode:      2000
Password:      Test@12345 (min 6 chars)
Confirm:       Test@12345
```

#### 3.4 Submit Registration
- Click **"Create Account"** button
- **Watch the Console** for messages:
  - 🔵 Messages = API is being called ✅
  - ✅ Messages = Registration succeeded ✅
  - ❌ Messages = Error occurred ❌

#### 3.5 Expected Success Flow
1. Console shows: `🔵 API Response Status: 200`
2. Message appears: **"Account created successfully! Redirecting to dashboard..."**
3. Page automatically redirects to **Dashboard** (dashboard/dashboard.html)
4. Dashboard displays your name and user info

---

## Step 4: Test Login After Registration

### 4.1 Navigate Back to Login
- You can manually go to `http://localhost/login.html` or
- Click the browser back button (but usually auto-redirects to dashboard)

### 4.2 Test Login with Same Credentials
- Click **"Sign In"** tab
- Enter:
  - Email: `john123@example.com` (same as registered)
  - Password: `Test@12345` (same as registered)
- Press **"Sign In"** button

### 4.3 Check Console Again
- Look for:
  - 🔵 `Login attempt for email: john123@example.com`
  - ✅ Login Success Response (if successful)
  - ❌ Error messages (if failed)

### 4.4 Expected Success Flow
1. Console shows: `🔵 Login API Response Status: 200`
2. Message appears: **"Login successful! Redirecting..."**
3. Page redirects to Dashboard
4. Dashboard shows your name

---

## ❌ Troubleshooting

### Issue: "Registration failed" (no console messages)
**Cause:** Application not running  
**Solution:**
1. Close the browser
2. In Visual Studio, press **F5** again
3. Wait for application to start
4. Try registration again

---

### Issue: Console shows `🔵 API Response Status: 500`
**Cause:** Server error (database issue)  
**Solution:**
1. Check Visual Studio Output Window (View → Output)
2. Look for error messages like:
   - "The entity type Student is not part of the model" → EF configuration issue
   - "Invalid operation. The connection is not open" → Database not accessible
3. Verify SQL Server is running and database exists

**To verify database:**
1. Open SQL Server Management Studio
2. Connect to: `(local)\SQLEXPRESS`
3. Look for `TKDRIVINGSCHOOLDB` in the database list
4. Expand it and check if `Student` and `UserAccount` tables exist

---

### Issue: "Invalid email or password" on Login
**But registration succeeded!**  
**Possible Causes:**
1. **UserAccount not created during registration** - Check database
2. **Password hashing issue** - Password wasn't stored correctly
3. **StudentId link broken** - Foreign key issue

**Solution:**
1. Open SQL Server Management Studio
2. Run these queries:
```sql
-- Check if student was created
SELECT TOP 10 StudentID, FirstName, LastName, Email 
FROM Student 
ORDER BY StudentID DESC;

-- Check if UserAccount was created
SELECT TOP 10 UserID, Username, Role, StudentID 
FROM UserAccount 
ORDER BY UserID DESC;

-- Check if they're linked
SELECT s.StudentID, s.FirstName, s.Email, ua.Username, ua.Role
FROM Student s
LEFT JOIN UserAccount ua ON s.StudentID = ua.StudentID
ORDER BY s.StudentID DESC;
```

If Student exists but UserAccount doesn't → UserAccount creation failed  
If both exist → Password verification might be broken

---

### Issue: "You must be at least 17 years old"
**This is correct!** Age validation is working.  
**Solution:** Use a birthdate that makes you 17+ years old  
Example: If today is 2026-08-16, use `2009-08-15` or earlier

---

### Issue: "Email already registered"
**This is correct!** Email uniqueness is enforced.  
**Solution:** Use a different email each time (add numbers: test1@, test2@, etc.)

---

## 🔍 Advanced Debugging

### Check Console Messages (Copy-Paste These to Console)
Open DevTools Console and paste:
```javascript
// View current stored user
console.log('Stored User:', JSON.parse(localStorage.getItem('currentUser')));

// Clear stored user (if stuck in bad state)
localStorage.removeItem('currentUser');
console.log('Cleared user session');

// Check API base URL
console.log('API Base:', '/api/auth');
```

### Monitor Network Requests
1. Open DevTools (F12)
2. Click **Network** tab
3. Try to register
4. Find the POST request to `/api/auth/register`
5. Click it and check:
   - **Request Headers:** Content-Type should be `application/json`
   - **Request Body:** Your form data
   - **Response Status:** Should be `200` (success) or `400` (validation error)
   - **Response Body:** Error message if anything fails

---

## ✅ Success Indicators

### Registration Working ✅
- [ ] Console shows `🔵 API Response Status: 200`
- [ ] Console shows `✅ API Success Response:`
- [ ] User redirects to Dashboard automatically
- [ ] Dashboard shows user's name and info
- [ ] Student record appears in database

### Login Working ✅
- [ ] Can login with registered email/password
- [ ] Console shows `✅ Login Success Response:`
- [ ] Redirects to Dashboard
- [ ] Dashboard displays correct user data

### Database Connected ✅
- [ ] SQL Server is running
- [ ] TKDRIVINGSCHOOLDB exists
- [ ] Student table has records after registration
- [ ] UserAccount table has records after registration

---

## 📞 Getting More Help

If you're still stuck, open the Visual Studio Output Window and share:
1. Any error messages displayed
2. Registration attempt with test data
3. Console messages from DevTools (F12 → Console)
4. Network tab showing the API response

---

**Status:** Registration & Login fully configured  
**Database:** TKDRIVINGSCHOOLDB with Student & UserAccount tables  
**Authentication:** SHA256 password hashing  
**Session:** localStorage-based user session
