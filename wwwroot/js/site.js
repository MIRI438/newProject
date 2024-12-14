const apiBaseUrl = "http://localhost:5190/api/BrandBags";

document.addEventListener("DOMContentLoaded", () => {
    if (localStorage.getItem("link") == "true") {
        let link = document.createElement("a");
        link.href = "./userList.html";
        link.innerHTML = "users";
        console.log(sessionStorage.getItem("link"));
        document.body.appendChild(link);
    }
    loadBags();
});

async function loadBags() {
    const userId = localStorage.getItem('id');
    const token = localStorage.getItem('authToken');

    if (!userId || !token) {
        alert('User not logged in');
        window.location.href = 'login.html'; 
        return;
    }

    try {
        const response = await fetch(`${apiBaseUrl}?userId=${userId}`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            console.log("Response Status:", response.status); 
            throw new Error(`Failed to fetch bags. Status: ${response.status}`);
        }

        const bags = await response.json();
        console.log("Fetched bags:", bags);

        const tableBody = document.querySelector("#bagsTable tbody");
        tableBody.innerHTML = ''; 

        bags.forEach(bag => {
            const row = document.createElement("tr");
            row.innerHTML = `
                <td>${bag.id}</td>
                <td>${bag.nameBrand}</td>
                <td>${bag.isDesigning}</td>
                <td>${bag.isVegan}</td>
                <td>
                    <button onclick="editBag(${bag.id})">Edit</button>
                    <button onclick="deleteBag(${bag.id})">Delete</button>
                </td>
            `;
            tableBody.appendChild(row);
        });

    } catch (error) {
        console.error("Error fetching bags:", error);
        alert(`Error fetching bags: ${error.message}`);
    }
}


async function addBag() {
    const NameBrand = prompt("Enter the name of the bag:");
    const id = 1;
    const IsDesigning = Boolean(prompt("IsDesigning?"));
    const IsVegan = Boolean(prompt("IsVegan?"));
    const UserId = localStorage.getItem("id");

    if (!NameBrand) return;

    try {
        const response = await fetch(apiBaseUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ id, NameBrand, IsDesigning, IsVegan, UserId })
        });

        if (!response.ok) {
            throw new Error("Failed to add bag");
        }

        loadBags(); 
    } catch (error) {
        alert("Error adding bag: " + error.message);
    }
}


async function editBag(id) {
    const Name = prompt("Enter the new name of the bag:");
    const desin = Boolean(prompt("IsDesigning?"));
    const Vegan = Boolean(prompt("IsVegan?"));
    const token = localStorage.getItem('authToken')

    let i = 365;
    if (!Name) return;

    let bag = { Id:i++ , NameBrand: Name, IsDesigning: desin, IsVegan: Vegan, UserId: id };

    try {
        const response = await fetch(`${apiBaseUrl}/${id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(bag)
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


async function deleteBag(id) {
    if (!confirm("Are you sure you want to delete this bag?")) return;

    try {
        const response = await fetch(`${apiBaseUrl}/${id}`, {
            method: "DELETE"
        });

        if (!response.ok) {
            throw new Error("Failed to delete bag");
        }

        loadBags(); 
    } catch (error) {
        alert("Error deleting bag: " + error.message);
    }
}

document.querySelector("#addBagBtn").addEventListener("click", addBag);
