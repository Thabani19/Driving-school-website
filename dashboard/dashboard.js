// ========================================
// TK Driving School Dashboard
// dashboard.js
// ========================================

// Current Date
const today = new Date();

const options = {
    weekday: "long",
    year: "numeric",
    month: "long",
    day: "numeric"
};

console.log("Today's Date:", today.toLocaleDateString("en-ZA", options));


// ========================================
// Dashboard Data
// ========================================

const dashboardData = {
    studentName: "John Smith",
    totalLessons: 10,
    completedLessons: 6,
    remainingLessons: 4,
    totalPaid: 3500
};


// ========================================
// Update Dashboard Cards
// ========================================

const cardNumbers = document.querySelectorAll(".card h2");

if (cardNumbers.length >= 4) {

    cardNumbers[0].textContent = dashboardData.totalLessons;

    cardNumbers[1].textContent = dashboardData.completedLessons;

    cardNumbers[2].textContent = dashboardData.remainingLessons;

    cardNumbers[3].textContent = "R" + dashboardData.totalPaid.toLocaleString();

}


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