document.addEventListener('DOMContentLoaded', function() {
    const loginForm = document.getElementById('loginForm');
    const loginMessage = document.getElementById('loginMessage');
    const registerForm = document.getElementById('registerForm');
    const registerMessage = document.getElementById('registerMessage');

    function showMessage(element, message, type) {
        if (!element) return;
        element.textContent = message;
        element.className = 'message ' + type;
    }

    function getUsers() {
        const users = JSON.parse(localStorage.getItem('users') || '[]');
        return Array.isArray(users) ? users : [];
    }

    if (loginForm) {
        loginForm.addEventListener('submit', function(e) {
            e.preventDefault();

            const email = document.getElementById('loginEmail').value.trim();
            const password = document.getElementById('loginPassword').value;
            const users = getUsers();
            const matchedUser = users.find(user => user.email.toLowerCase() === email.toLowerCase() && user.password === password);

            if (!matchedUser) {
                showMessage(loginMessage, 'Invalid email or password.', 'error');
                return;
            }

            localStorage.setItem('currentUser', JSON.stringify(matchedUser));
            showMessage(loginMessage, 'Login successful! Redirecting...', 'success');

            setTimeout(() => {
                window.location.href = 'dashboard/dashboard.html';
            }, 800);
        });
    }

    if (registerForm) {
        registerForm.addEventListener('submit', function(e) {
            e.preventDefault();

            const firstName = document.getElementById('firstName').value.trim();
            const lastName = document.getElementById('lastName').value.trim();
            const email = document.getElementById('email').value.trim();
            const phone = document.getElementById('phone').value.trim();
            const dateOfBirth = document.getElementById('dateOfBirth').value;
            const address = document.getElementById('address').value.trim();
            const city = document.getElementById('city').value.trim();
            const postcode = document.getElementById('postcode').value.trim();
            const password = document.getElementById('password').value;
            const confirmPassword = document.getElementById('confirmPassword').value;

            if (!validateRegistration(firstName, lastName, email, phone, dateOfBirth, address, city, postcode, password, confirmPassword, registerMessage)) {
                return;
            }

            const users = getUsers();
            const exists = users.some(user => user.email.toLowerCase() === email.toLowerCase());
            if (exists) {
                showMessage(registerMessage, 'An account with this email already exists.', 'error');
                return;
            }

            const newUser = {
                id: Date.now(),
                firstName,
                lastName,
                email,
                phone,
                dateOfBirth,
                address,
                city,
                postcode,
                password,
                createdAt: new Date().toISOString(),
                lessons: { total: 10, completed: 6, remaining: 4 }
            };

            users.push(newUser);
            localStorage.setItem('users', JSON.stringify(users));
            showMessage(registerMessage, 'Account created successfully! Redirecting to login...', 'success');

            setTimeout(() => {
                window.location.href = 'login.html';
            }, 1200);
        });
    }

    function validateRegistration(firstName, lastName, email, phone, dateOfBirth, address, city, postcode, password, confirmPassword, messageElement) {
        if (!firstName) {
            showMessage(messageElement, 'First name is required', 'error');
            return false;
        }

        if (!lastName) {
            showMessage(messageElement, 'Last name is required', 'error');
            return false;
        }

        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email)) {
            showMessage(messageElement, 'Please enter a valid email address', 'error');
            return false;
        }

        const phoneRegex = /^[\d\s\-\+\(\)]+$/;
        if (!phoneRegex.test(phone) || phone.replace(/\D/g, '').length < 10) {
            showMessage(messageElement, 'Please enter a valid phone number', 'error');
            return false;
        }

        if (!dateOfBirth) {
            showMessage(messageElement, 'Date of birth is required', 'error');
            return false;
        }

        const age = new Date().getFullYear() - new Date(dateOfBirth).getFullYear();
        if (age < 17) {
            showMessage(messageElement, 'You must be at least 17 years old to register', 'error');
            return false;
        }

        if (!address) {
            showMessage(messageElement, 'Address is required', 'error');
            return false;
        }

        if (!city) {
            showMessage(messageElement, 'City is required', 'error');
            return false;
        }

        if (!postcode) {
            showMessage(messageElement, 'Postcode is required', 'error');
            return false;
        }

        if (password.length < 6) {
            showMessage(messageElement, 'Password must be at least 6 characters long', 'error');
            return false;
        }

        if (password !== confirmPassword) {
            showMessage(messageElement, 'Passwords do not match', 'error');
            return false;
        }

        return true;
    }

    const currentUser = localStorage.getItem('currentUser');
    if (currentUser && window.location.pathname.includes('login.html')) {
        setTimeout(() => {
            window.location.href = 'dashboard/dashboard.html';
        }, 300);
    }
});
