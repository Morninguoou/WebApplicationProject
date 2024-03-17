const container = document.getElementById('container');
const registerBtn = document.getElementById('register');
const loginBtn = document.getElementById('login');

registerBtn.addEventListener('click', () => {
    container.classList.add("active");
});

loginBtn.addEventListener('click', () => {
    container.classList.remove("active");
});

function validateForm() {
    var password = document.getElementById("password").value;
    var confirm_password = document.getElementById("confirm_password").value;
  
    if (password != confirm_password) {
      alert("Confirm password ไม่ตรงกับ password");
      return false;
    }
    return true;
}

document.addEventListener("DOMContentLoaded", function() {
    var loginButton = document.querySelector("#loginForm button[type=submit]");
    
    loginButton.addEventListener("click", function(event) {
        event.preventDefault();
    });
});