let accounts = [];
let connection = null;
let accountIdToUpdate = 1;
let genderToUpdate = "";
getdata();
setupSignalR();

function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:34372/account/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("AccountCreated", (user, message) => {
        getdata();
    });

    connection.on("AccountDeleted", (user, message) => {
        getdata();
    });

    connection.on("AccountUpdated", (user, message) => {
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
    fetch('http://localhost:34372/account')
        .then(x => x.json())
        .then(y => {
            accounts = y;
            console.log(accounts);
            display();
            showupdate(accountIdToUpdate);
        });
}

function display() {
    document.getElementById('resultarea').innerHTML = "";
    accounts.forEach(t => {
        document.getElementById('resultarea').innerHTML +=
            " <tr><td>"
        + t.accountNumber_ID + "</td><td>"
        + t.customerId + "</td><td>"
        + t.currencyType + "</td><td>"
        + t.balance + "</td><td>"
        + t.creationDate + "</td><td>"
        + t.accountType + "</td><td>"
        + `<button type="button" onclick="remove(${t.accountNumber_ID})">Delete</button>`
        + `<button type="button" onclick="showupdate(${t.accountNumber_ID})">Update</button>`
            + "</td></tr >";
    });
}

function remove(id) {
    fetch('http://localhost:34372/account/' + id, {
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
    document.getElementById('customeridupdate').value = accounts.find(x => x['accountNumber_ID'] == id)['customerId'];

    let acct = accounts.find(x => x['accountNumber_ID'] == id)['accountType'];
    if (acct == "Savings") {
        document.getElementById("savingsupdate").checked = true;
    } else {
        document.getElementById("currentupdate").checked = true;
    }
    document.getElementById('balaceupdate').value = accounts.find(x => x['accountNumber_ID'] == id)['balance'];

    let creationdate = accounts.find(x => x['accountNumber_ID'] == id)['creationDate'];
    document.getElementById('creationdateupdate').value = creationdate.split("T")[0];

    let currencytype = accounts.find(x => x['accountNumber_ID'] == id)['currencyType'];
    if (currencytype == "EUR") {
        document.getElementById("eurupdate").checked = true;
    } else {
        document.getElementById("hufupdate").checked = true;
    }

    accountIdToUpdate = id;
}
function update() {
    let customeridupdate = document.getElementById('customeridupdate').value;

    let accounttype;
    if (document.getElementById("savingsupdate").checked == true) {
        accounttype = "Savings";
    } else {
        accounttype = "Current";
    }

    let balaceupdate = document.getElementById('balaceupdate').value;
    let creationdateupdate = document.getElementById('creationdateupdate').value;

    let currencytype;
    if (document.getElementById("eurupdate").checked == true) {
        currencytype = "EUR";
    } else {
        currencytype = "HUF";
    }
    

    fetch('http://localhost:34372/account', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            {accountNumber_ID : accountIdToUpdate, customerId: customeridupdate, accountType: accounttype, balance: balaceupdate, creationDate: creationdateupdate, currencyType: currencytype })
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata();
        })
        .catch((error) => { console.error('Errorvan:', error); });


}

function create() {
    let customerIdcreate = document.getElementById('customerIdcreate').value;
    let currencyTypecreate = "";
    if (document.getElementById('eur').checked == true) {
        currencyTypecreate = "EUR";
    }
    else {
        currencyTypecreate = "HUF";
    }

    let accountTypecreate = "";
    if (document.getElementById('savings').checked == true) {
        accountTypecreate = "Savings";
    }
    else {
        accountTypecreate = "Current";
    }

    fetch('http://localhost:34372/account/', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            { customerId: customerIdcreate, accountType: accountTypecreate, currencyType: currencyTypecreate})
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata();
        })
        .catch((error) => { console.error('Errorvan:', error); });


}