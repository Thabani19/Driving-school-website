// TK Driving School - Student Profile
// Loads user data from localStorage registration
document.addEventListener('DOMContentLoaded', function() {
    const currentUserData = localStorage.getItem("currentUser");
    if (!currentUserData) {
        alert("Please sign in first");
        window.location.href = "../login.html";
        return;
    }
    
    let user = JSON.parse(currentUserData);
    
    // Get elements
    const firstName = document.getElementById("firstName");
    const lastName = document.getElementById("lastName");
    const email = document.getElementById("email");
    const phone = document.getElementById("phone");
    const dateOfBirth = document.getElementById("dateOfBirth");
    const address = document.getElementById("address");
    const city = document.getElementById("city");
    const postcode = document.getElementById("postcode");
    
    const profileName = document.getElementById("profileName");
    const headerName = document.getElementById("headerName");
    
    const editBtn = document.getElementById("editBtn");
    const cancelBtn = document.getElementById("cancelBtn");
    const signOutBtn = document.getElementById("signOutBtn");
    const logoutLink = document.getElementById("logout");
    
    // Display user data from localStorage
    function displayUserData() {
        if (firstName) firstName.value = user.firstName || "";
        if (lastName) lastName.value = user.lastName || "";
        if (email) email.value = user.email || "";
        if (phone) phone.value = user.phone || "";
        if (dateOfBirth) dateOfBirth.value = user.dateOfBirth || "";
        if (address) address.value = user.address || "";
        if (city) city.value = user.city || "";
        if (postcode) postcode.value = user.postcode || "";
        
        const fullName = (user.firstName || "User") + " " + (user.lastName || "");
        if (profileName) profileName.textContent = fullName + " - Profile";
        if (headerName) headerName.textContent = fullName;
    }
    
    // Display user data on page load
    displayUserData();
    
    // Sign Out button
    if (signOutBtn) {
        signOutBtn.addEventListener("click", function() {
            if (confirm("Are you sure you want to sign out?")) {
                localStorage.removeItem("currentUser");
                window.location.href = "../login.html";
            }
        });
    }
    
    // Logout from sidebar
    if (logoutLink) {
        logoutLink.addEventListener("click", function(e) {
            e.preventDefault();
            if (confirm("Are you sure you want to logout?")) {
                localStorage.removeItem("currentUser");
                window.location.href = "../login.html";
            }
        });
    }
});
