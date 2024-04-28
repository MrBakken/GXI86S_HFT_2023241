let customers = [];
let connection = null;
getdata();
setupSignalR();

function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:34372/customer/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("CustomerCreated", (user, message) => {
        getdata();
    });

    connection.on("CustomerDeleted", (user, message) => {
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
            + "</td></tr >";
    });
}

function remove(id) {
    fetch('http://localhost:34372/Customer/' + id, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json', },
        body: null })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata();
        })
        .catch((error) => { console.error('Error:', error); });

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