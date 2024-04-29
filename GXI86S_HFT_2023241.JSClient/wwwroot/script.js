let customers = [];
let connection = null;
let custumerIdToUpdate = 1;
let genderToUpdate ="";
getdata();
setupSignalR();

function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:34372/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("CustomerCreated", (user, message) => {
        getdata();
    });

    connection.on("CustomerDeleted", (user, message) => {
        getdata();
    });

    connection.on("CustomerUpdated", (user, message) => {
        getdata();
    });

    connection.onclose(async () => {
        await start();
    });
    start();
}

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

async function getdata() {
    fetch('http://localhost:34372/customer')
        .then(x => x.json())
        .then(y => {
            customers = y;
            //console.log(customers);
            display();
            showupdate(custumerIdToUpdate);
        });
}

function display() {
    document.getElementById('resultarea').innerHTML = "";
    customers.forEach(t => {
        document.getElementById('resultarea').innerHTML +=
            " <tr><td>" + t.id + "</td><td>"
            + t.firstName + "</td><td>"
            + t.lastName + "</td><td>"
            + t.email + "</td><td>"
            + t.gender + "</td><td>"
            + t.birthDate + "</td><td>"
            + t.phone + "</td><td>"
            + `<button type="button" onclick="remove(${t.id})">Delete</button>`
        + `<button type="button" onclick="showupdate(${t.id})">Update</button>`
            + "</td></tr >";
    });
}

function remove(id) {
    fetch('http://localhost:34372/Customer/' + id, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json', },
        body: null
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata();
        })
        .catch((error) => { console.error('Error:', error); });

}

function showupdate(id) {
    /*document.getElementById('updateformdiv').style.display = 'flex';*/
    document.getElementById('firstnameupdate').value = customers.find(x => x['id'] == id)['firstName'];
    document.getElementById('lastnameupdate').value = customers.find(x => x['id'] == id)['lastName'];
    let birthpart = customers.find(x => x['id'] == id)['birthDate'];
    document.getElementById('birthupdate').value = birthpart.split("T")[0];
    document.getElementById('emailupdate').value = customers.find(x => x['id'] == id)['email'];
    document.getElementById('phoneupdate').value = customers.find(x => x['id'] == id)['phone'];
    let gender = customers.find(x => x['id'] == id)['gender'];
    if (gender =="Male") {
        document.getElementById("male").checked = true;
    } else {
        document.getElementById("female").checked = true;
    }

    custumerIdToUpdate = id;
}
function update() {
    let firstname = document.getElementById('firstnameupdate').value;
    let lastname = document.getElementById('lastnameupdate').value;
    let birth = document.getElementById('birthupdate').value;
    let email = document.getElementById('emailupdate').value;
    let phone = document.getElementById('phoneupdate').value;
    let gender;
    if (document.getElementById("male").checked == true) {
        gender = "Male"; 
    } else {
        gender = "Female";
    }

    fetch('http://localhost:34372/Customer', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            { id: custumerIdToUpdate, gender: gender, firstName: firstname, lastName: lastname, birthDate: birth, email: email, phone: phone  })
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata();
        })
        .catch((error) => { console.error('Errorvan:', error); });


}

function create() {
    let firstname = document.getElementById('firstname').value;
    let lastname = document.getElementById('lastname').value;
    let birth = document.getElementById('birth').value;

    fetch('http://localhost:34372/Customer', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            { firstName: firstname, lastName: lastname, birthDate: birth })
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata();
        })
        .catch((error) => { console.error('Errorvan:', error); });


}