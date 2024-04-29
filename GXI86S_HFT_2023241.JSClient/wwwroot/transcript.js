let transactions = [];
let connection = null;
let tranIdToUpdate = 1;
let genderToUpdate = "";
getdata();
setupSignalR();

function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:34372/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();
    
    connection.on("TransactionCreated", (user, message) => {
        getdata();
    });

    connection.on("TransactionDeleted", (user, message) => {
        getdata();
    });

    connection.on("TransactionUpdated", (user, message) => {
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
    fetch('http://localhost:34372/transaction')
        .then(x => x.json())
        .then(y => {
            transactions = y;
            console.log(transactions);
            display();
            showupdate(tranIdToUpdate);
        });
}

function display() {
    document.getElementById('resultarea').innerHTML = "";
    transactions.forEach(t => {
        document.getElementById('resultarea').innerHTML +=
        " <tr><td>"
        + t.id + "</td><td>"
        + t.accountId + "</td><td>"
        + t.date + "</td><td>"
        + t.amount + "</td><td>"
        + t.description + "</td><td>"
        + `<button type="button" onclick="remove(${t.id})">Delete</button>`
        + `<button type="button" onclick="showupdate(${t.id})">Update</button>`
            + "</td></tr >";
    });
}

function remove(id) {
    fetch('http://localhost:34372/transaction/' + id, {
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
    document.getElementById('accountidupdate').value = transactions.find(x => x['id'] == id)['accountId'];
    document.getElementById('amountupdate').value = transactions.find(x => x['id'] == id)['amount'];
    document.getElementById('descriptionupdate').value = transactions.find(x => x['id'] == id)['description'];
    
    let creationdate = transactions.find(x => x['id'] == id)['date'];
    document.getElementById('dateupdate').value = creationdate.split("T")[0];
    
    tranIdToUpdate = id;
}
function update() {
    let accountidupdate = document.getElementById('accountidupdate').value;
    let dateupdate = document.getElementById('dateupdate').value;
    let amountupdate = document.getElementById('amountupdate').value;
    let descriptionupdate = document.getElementById('descriptionupdate').value;
    
    fetch('http://localhost:34372/transaction', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            { id: tranIdToUpdate, accountId: accountidupdate, date: dateupdate, amount: amountupdate, description: descriptionupdate })
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata();
        })
        .catch((error) => { console.error('Errorvan:', error); });


}

function create() {
    let accountidcreate = document.getElementById('accountidcreate').value;
    let amountcreate = document.getElementById('amountcreate').value;
    let descriptioncreate = document.getElementById('descriptioncreate').value;
 
    fetch('http://localhost:34372/transaction/', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            { accountId: accountidcreate, amount: amountcreate, description: descriptioncreate })
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata();
        })
        .catch((error) => { console.error('Errorvan:', error); });


}