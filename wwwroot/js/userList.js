const uri = "http://localhost:5190/api/Users";
let users = [];
const token = localStorage.getItem('authToken');

document.querySelector("#addUserBtn").addEventListener("click", addItem);
let returnList = document.querySelector("#returnListBag");


returnList.addEventListener("click", () => {
    window.location.href = "./index.html";  
});

document.addEventListener("DOMContentLoaded", () => {
    getItems();

})
async function getItems() {
    
    try {
        const response = await fetch(uri, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,  
                'Content-Type': 'application/json'
            },
            
        });

        if (!response.ok) {
            throw new Error('Failed to fetch brand bags');
        }
        debugger
        const user = await response.json();
        console.log(user);
        const tableBody = document.querySelector("#userTable tbody");
        tableBody.innerHTML = ''; 

        user.forEach(user => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td>${user.id}</td>
                <td>${user.name}</td>
                <td>${user.isNew}</td>
                <td>
                    <button onclick="editUser(${user.id})">Edit</button>
                    <button onclick="deleteUser(${user.id})">Delete</button>
                </td>
            `;
            tableBody.appendChild(row);
        });
    } 
    catch (error) {
        console.error(error);
        alert("Error fetching bags: " + error.message);
    }
}

async function addItem() {

    const Name = prompt("Enter the name of the user:");
    const Id = parseInt(prompt("Enter the password of the user:"));
    const IsNew = Boolean(prompt("IsNew?"));



    try{
        const response = await fetch(uri,{
            method: "POST",
            headers: {
                'Authorization': `Bearer ${token}`,  
                "Content-Type": "application/json" 
            },
            body: JSON.stringify({Id,Name,IsNew})
        });

        debugger
        if(!response.ok){
            throw new Error("Faild to add user")
        }
        getItems();

    }
    catch(error){
        console.log(error);
        alert("Error adding user");
    }
}

async function editUser(id) {
    const Name = prompt("Enter the new name of the user:");

    if (!Name) return;

    let user = { Id: id, Name: Name, IsNew: false };

    try {
        const response = await fetch(`${uri}/${id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(user)
        });
        debugger
        if (!response.ok) {
            const errorText = await response.text(); 
            throw new Error(`Failed to update user: ${errorText}`);
        }

        getItems(); 
    } catch (error) {
        alert("Error updating user: " + error.message); 
    }
}

async function deleteUser(id) {
    if (!confirm("Are you sure you want to delete this user?")) return;

    try {
        const response = await fetch(`${uri}/${id}`, {
            method: "DELETE",
            headers: {
                'Authorization': `Bearer ${token}`,
                "Content-Type": "application/json"  
            }
        });

        if (!response.ok) {
            const errorText = await response.text();  
            console.error("Error deleting user:", errorText);  
            throw new Error("Failed to delete user");
        }

        getItems(); 
    } catch (error) {
        alert("Error deleting user: " + error.message);  
    }
}



