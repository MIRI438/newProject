const uri = "http://localhost:5190/api/users/login";

document.getElementById('getManagerButton').addEventListener('click', async function() {
    
    alert("Admin,123");
});

document.getElementById('login-form').addEventListener('submit', function (event) {
    event.preventDefault();

    let user = {
        name: document.getElementById('username').value,
        id: document.getElementById('userid').value
    };
    console.log("Sending user data:", JSON.stringify(user));

    fetch(uri, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error("Login failed. Status: " + response.status);
        }
        return response.json();
    })
    .then(data => {
        if (data.token) {
            localStorage.setItem("id",user.id);
            console.log(user.name,user.id);

            if ( user.id == 123)
                localStorage.setItem("link", true);
            else
                localStorage.setItem("link", false);


            console.log("Received token:", data.token); 
            localStorage.setItem('authToken', data.token);
            console.log("Login successful, token saved.");
            alert("Login successful!");
            location.href ="../index.html"

        } else {
            throw new Error("Login failed: No token returned.");
        }
    })
    .catch(error => {
        console.error("Error during login:", error);
        alert("Error during login: " + error.message);
    });
});

