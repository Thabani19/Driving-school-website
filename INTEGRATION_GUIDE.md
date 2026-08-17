# Registration & Profile Integration Guide

## Overview
The Driving School website now has a fully integrated registration and profile system where users can:
1. Register a new account
2. Log in with their credentials
3. View and edit their profile
4. Access the dashboard with their information

---

## System Architecture

### 1. **Authentication (auth.html & auth.js)**

#### Registration Flow
- Users enter personal information: First Name, Last Name, Email, Phone, Date of Birth, Address, City, Postcode
- Password validation (minimum 6 characters, must match confirmation)
- Age validation (minimum 17 years old)
- Email validation and duplicate check
- User data stored in `localStorage` under `users` array
- Current logged-in user stored in `currentUser` key

#### Login Flow
- Email and password authentication
- Searches `users` array for matching credentials
- Sets `currentUser` on successful login
- Redirects to dashboard

#### File: `auth.html`
- Modern, responsive authentication UI
- Tab-based interface (Sign In / Register)
- Client-side form validation
- Visual feedback for success/error messages

#### File: `Scripts/auth.js`
- Tab switching functionality
- Form validation with age checking
- LocalStorage management
- User registration and login logic
- Automatic redirect for already-logged-in users

---

### 2. **Dashboard Integration (dashboard.html & dashboard.js)**

#### Features
- User profile display with initials and name
- Protected route (redirects to login if not authenticated)
- Dynamic content updated from user data
- Lesson statistics display
- Theme toggle (Dark/Light mode)
- Active navigation highlighting
- Logout functionality

#### File: `dashboard/dashboard.js` (Updated)
- Authentication check on page load
- User information display
- Navigation setup with proper links
- Dashboard data population from user object
- Theme preference persistence
- Active link highlighting

---

### 3. **Profile Management (profile/profile.html & profile.js)**

#### Features
- View user information
- Edit personal details (First Name, Last Name, Phone, Address, City, Postcode)
- Password change capability
- Lesson statistics display
- Protected route (redirects to login if not authenticated)
- Changes sync with user data and users array

#### File: `profile/profile.html` (Updated)
- Enhanced layout with avatar display
- Organized sections (Personal Info, Address, Statistics, Account Settings)
- Info cards showing lesson statistics
- Edit mode toggle
- Success/error message display
- Responsive design

#### File: `profile/profile.js` (Updated)
- User data display and management
- Edit mode toggle functionality
- Form validation for updates
- Password change handling
- LocalStorage synchronization
- Navigation setup

---

## Data Storage Structure

### Users Array (localStorage['users'])
```javascript
{
    id: 1234567890,
    firstName: "John",
    lastName: "Smith",
    email: "john@example.com",
    phone: "+27 123 456 7890",
    dateOfBirth: "1990-05-15",
    address: "123 Main Street",
    city: "Johannesburg",
    postcode: "2000",
    password: "hashedPassword", // Note: Should be hashed in production
    createdAt: "2026-08-16T10:30:00.000Z",
    profileImage: "JS", // Initials for avatar
    lessons: {
        total: 10,
        completed: 6,
        remaining: 4,
        booked: []
    }
}
```

### Current User (localStorage['currentUser'])
- Same structure as Users Array
- Automatically updated when profile changes
- Cleared on logout

---

## User Flow Diagram

```
Home (index.cshtml)
    ↓
[Sign In / Register Buttons] → auth.html
    ↓
Registration Success
    ↓
Login (sets currentUser in localStorage)
    ↓
Dashboard (dashboard.html)
    ├─ View Dashboard
    ├─ View Profile → profile/profile.html
    │   ├─ View Info
    │   ├─ Edit Info (editable fields)
    │   └─ Save Changes (syncs with users array)
    └─ Logout (clears currentUser)
```

---

## Security Considerations

### Current Implementation
- Client-side validation
- LocalStorage for user data
- Password stored in plain text (⚠️ **Development only**)

### Production Recommendations
1. **Backend Authentication** - Implement server-side login/registration
2. **Password Hashing** - Use bcrypt or similar for password security
3. **HTTPS Only** - Encrypt data in transit
4. **Session Tokens** - Replace localStorage with secure session tokens
5. **Input Sanitization** - Prevent XSS attacks
6. **Database** - Use proper database instead of localStorage
7. **Rate Limiting** - Protect against brute force attacks

---

## Testing the Integration

### Step 1: Registration
1. Navigate to `auth.html`
2. Click "Register" tab
3. Fill in all required fields
4. Create account
5. Verify data in browser DevTools → Application → localStorage

### Step 2: Login
1. Click "Sign In" tab
2. Use registered email and password
3. Should redirect to dashboard

### Step 3: Profile Management
1. From dashboard, click "My Profile"
2. View current information
3. Click "Edit Profile"
4. Modify fields
5. Click "Save Changes"
6. Verify changes persist after page reload

### Step 4: Logout
1. Click "Logout" in sidebar
2. Should redirect to auth.html
3. Verify `currentUser` is cleared from localStorage

---

## File Modifications Summary

| File | Status | Changes |
|------|--------|---------|
| `auth.html` | ✅ Created | New registration & login page |
| `Scripts/auth.js` | ✅ Created | Authentication logic |
| `dashboard/dashboard.js` | ✅ Updated | User integration, protected route |
| `dashboard/dashboard.html` | ✅ Unchanged | Links updated by JS |
| `profile/profile.html` | ✅ Updated | Enhanced layout, proper form fields |
| `profile/profile.js` | ✅ Updated | User data management, edit mode |

---

## Navigation Links

All navigation automatically configured:
- Dashboard → `dashboard/dashboard.html`
- My Profile → `profile/profile.html`
- Logout → Clears `currentUser` and redirects to `auth.html`

---

## Next Steps

1. ✅ Test complete registration flow
2. ✅ Verify profile updates work correctly
3. ⏳ Connect to backend API (when available)
4. ⏳ Implement proper password hashing
5. ⏳ Add email verification
6. ⏳ Add password reset functionality
7. ⏳ Implement lesson booking system
8. ⏳ Add instructor assignment logic
9. ⏳ Create payment integration
10. ⏳ Add progress tracking

---

## Troubleshooting

### Issue: User data not persisting
**Solution:** Check browser localStorage is enabled and has space available

### Issue: Login redirects to register
**Solution:** Verify email/password match exactly (case-sensitive)

### Issue: Profile changes not showing on dashboard
**Solution:** Page may need refresh or `currentUser` needs sync with `users` array

### Issue: Dark mode not persisting
**Solution:** Check localStorage 'darkMode' key is being set

---

**Version:** 1.0  
**Last Updated:** 2026-08-16  
**Status:** Development
