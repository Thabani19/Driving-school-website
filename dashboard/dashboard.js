// ========================================
// TK Driving School Dashboard
// dashboard.js
// Integrated with User Registration & Profile
// ========================================

document.addEventListener('DOMContentLoaded', function() {
    // Check if user is logged in
    const currentUser = localStorage.getItem('currentUser');
    
    if (!currentUser) {
        // Redirect to login if not authenticated
        window.location.href = '../login.html';
        return;
    }

    const user = JSON.parse(currentUser);

    // Update profile section
    updateUserProfile(user);

    // Setup navigation links
    setupNavigation();

    // Update dashboard with user data
    updateDashboardData(user);

    // Current Date
    const today = new Date();
    const options = {
        weekday: "long",
        year: "numeric",
        month: "long",
        day: "numeric"
    };
    console.log("Today's Date:", today.toLocaleDateString("en-ZA", options));

    function updateUserProfile(user) {
        // Update avatar
        const avatars = document.querySelectorAll('.avatar');
        const initials = (user.firstName.charAt(0) + user.lastName.charAt(0)).toUpperCase();
        avatars.forEach(avatar => {
            avatar.textContent = initials;
        });

        // Update user name in profile section
        const profileTexts = document.querySelectorAll('.profile-text span');
        profileTexts.forEach(el => {
            if (el.textContent && (el.textContent.includes('John') || el.textContent.includes('Smith'))) {
                el.textContent = `${user.firstName} ${user.lastName}`;
            }
        });

        // Update welcome message
        const h1 = document.querySelector('.main-content h1');
        if (h1 && h1.textContent.includes('Welcome')) {
            h1.textContent = `Welcome back, ${user.firstName}!`;
        }

        // Update student information section
        const userFirstName = document.getElementById('userFirstName');
        const userLastName = document.getElementById('userLastName');
        const userEmail = document.getElementById('userEmail');
        const userPhone = document.getElementById('userPhone');
        const userDOB = document.getElementById('userDOB');
        const userAddress = document.getElementById('userAddress');
        const userCity = document.getElementById('userCity');
        const userPostcode = document.getElementById('userPostcode');

        if (userFirstName) userFirstName.textContent = user.firstName || '--';
        if (userLastName) userLastName.textContent = user.lastName || '--';
        if (userEmail) userEmail.textContent = user.email || '--';
        if (userPhone) userPhone.textContent = user.phone || '--';
        if (userDOB && user.dateOfBirth) {
            const dob = new Date(user.dateOfBirth);
            userDOB.textContent = dob.toLocaleDateString('en-ZA', { year: 'numeric', month: 'long', day: 'numeric' });
        } else if (userDOB) {
            userDOB.textContent = '--';
        }
        if (userAddress) userAddress.textContent = user.address || '--';
        if (userCity) userCity.textContent = user.city || '--';
        if (userPostcode) userPostcode.textContent = user.postcode || '--';
    }

    function setupNavigation() {
        // Update all navigation links
        const navLinks = document.querySelectorAll('.sidebar a');
        navLinks.forEach(link => {
            const text = link.textContent.trim();
            switch(text) {
                case 'Dashboard':
                    link.href = 'dashboard.html';
                    break;
                case 'My Profile':
                    link.href = '../profile/profile.html';
                    break;
                case 'Logout':
                    link.href = '#';
                    link.addEventListener('click', function(e) {
                        e.preventDefault();
                        if (confirm('Are you sure you want to logout?')) {
                            localStorage.removeItem('currentUser');
                            window.location.href = '../login.html';
                        }
                    });
                    break;
            }
        });
    }

    function updateDashboardData(user) {
        // Update lesson statistics from user data
        const cardNumbers = document.querySelectorAll(".card h2");
        const lessons = user.lessons || { 
            total: 10, 
            completed: 6, 
            remaining: 4 
        };
        
        if (cardNumbers.length >= 4) {
            cardNumbers[0].textContent = lessons.total;
            cardNumbers[1].textContent = lessons.completed;
            cardNumbers[2].textContent = lessons.remaining || (lessons.total - lessons.completed);
            cardNumbers[3].textContent = "R" + (lessons.total * 350).toLocaleString(); // R350 per lesson
        }
    }

    // Theme Toggle
    const themeToggle = document.getElementById('themeToggle');
    if (themeToggle) {
        themeToggle.addEventListener('click', function() {
            document.body.classList.toggle('dark-mode');
            const isDark = document.body.classList.contains('dark-mode');
            localStorage.setItem('darkMode', isDark);
            
            const icon = themeToggle.querySelector('.theme-icon');
            const label = themeToggle.querySelector('.theme-label');
            
            if (isDark) {
                icon.textContent = '☀️';
                label.textContent = 'Light';
            } else {
                icon.textContent = '🌙';
                label.textContent = 'Dark';
            }
        });

        // Load saved theme preference
        if (localStorage.getItem('darkMode') === 'true') {
            document.body.classList.add('dark-mode');
            themeToggle.querySelector('.theme-icon').textContent = '☀️';
            themeToggle.querySelector('.theme-label').textContent = 'Light';
        }
    }

    // Make nav items highlight on click
    const sidebarLinks = document.querySelectorAll('.sidebar a');
    sidebarLinks.forEach(link => {
        link.addEventListener('click', function() {
            if (!this.href.includes('#')) {
                sidebarLinks.forEach(l => l.classList.remove('active'));
                this.classList.add('active');
            }
        });
    });
});


// ========================================
// Welcome Message
// ========================================

const profileName = document.querySelector(".profile span");

if(profileName){

    profileName.textContent = dashboardData.studentName;

}


// ========================================
// Greeting
// ========================================

const hour = new Date().getHours();

let greeting = "";

if(hour < 12){

    greeting = "Good Morning!";

}
else if(hour < 17){

    greeting = "Good Afternoon!";

}
else{

    greeting = "Good Evening!";

}

const heading = document.querySelector(".header h1");

if(heading){

    heading.textContent = greeting + ", " + dashboardData.studentName;

}


// ========================================
// Quick Action Buttons
// ========================================

const buttons = document.querySelectorAll(".btn");

buttons.forEach(button=>{

    button.addEventListener("click",function(e){

        e.preventDefault();

        alert(button.textContent + " page will be available soon.");

    });

});

// ========================================
// Theme Toggle
// ========================================

const themeToggle = document.getElementById("themeToggle");
const themeIcon = document.querySelector(".theme-icon");
const themeLabel = document.querySelector(".theme-label");

const applyTheme = (theme) => {
    const isDark = theme === "dark";
    document.body.classList.toggle("dark-mode", isDark);

    if (themeIcon) {
        themeIcon.textContent = isDark ? "☀️" : "🌙";
    }

    if (themeLabel) {
        themeLabel.textContent = isDark ? "Light" : "Dark";
    }

    localStorage.setItem("tk-driving-theme", theme);
};

const savedTheme = localStorage.getItem("tk-driving-theme") || "light";
applyTheme(savedTheme);

if (themeToggle) {
    themeToggle.addEventListener("click", () => {
        const isDark = document.body.classList.contains("dark-mode");
        applyTheme(isDark ? "light" : "dark");
    });
}


// ========================================
// Sidebar Navigation
// ========================================

const menuItems = document.querySelectorAll(".sidebar a");

menuItems.forEach(item=>{

    item.addEventListener("click",function(){

        menuItems.forEach(link=>{

            link.classList.remove("active");

        });

        this.classList.add("active");

    });

});


// ========================================
// Notifications
// ========================================

const notifications = document.querySelectorAll(".notification");

notifications.forEach((note,index)=>{

    note.style.opacity = "0";

    setTimeout(()=>{

        note.style.transition = "0.6s";

        note.style.opacity = "1";

    },300 * index);

});


// ========================================
// Highlight Upcoming Lesson
// ========================================

const lessonCard = document.querySelector(".lesson-card");

if(lessonCard){

    lessonCard.addEventListener("mouseover",()=>{

        lessonCard.style.boxShadow="0 0 15px rgba(179,0,0,.4)";

    });

    lessonCard.addEventListener("mouseout",()=>{

        lessonCard.style.boxShadow="none";

    });

}


// ========================================
// Booking Table Hover Effect
// ========================================

const rows = document.querySelectorAll("table tr");

rows.forEach(row=>{

    row.addEventListener("mouseover",()=>{

        row.style.background="#f4f4f4";

    });

    row.addEventListener("mouseout",()=>{

        row.style.background="";

    });

});


// ========================================
// Fake Notification Counter
// ========================================

let notificationCount = notifications.length;

console.log("Notifications:", notificationCount);


// ========================================
// Auto Refresh Clock
// ========================================

function updateClock(){

    const now = new Date();

    const time = now.toLocaleTimeString();

    console.log(time);

}

setInterval(updateClock,1000);


// ========================================
// Console Message
// ========================================

console.log("TK Driving School Dashboard Loaded Successfully");