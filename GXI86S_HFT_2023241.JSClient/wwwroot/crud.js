let CustomerTransactionInfos = [];
let connection = null;
getdataCustomerTransactionInfos();
setupSignalR();

function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:34372/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("TransactionCreated", (user, message) => {
        getdataCustomerTransactionInfos();
    });

    connection.on("TransactionDeleted", (user, message) => {
        getdataCustomerTransactionInfos();
    });

    connection.on("TransactionUpdated", (user, message) => {
        getdataCustomerTransactionInfos();
    });
    connection.on("AccountCreated", (user, message) => {
        getdataCustomerTransactionInfos();
    });

    connection.on("AccountDeleted", (user, message) => {
        getdataCustomerTransactionInfos();
    });

    connection.on("AccountUpdated", (user, message) => {
        getdataCustomerTransactionInfos();
    }); connection.on("CustomerCreated", (user, message) => {
        getdataCustomerTransactionInfos();
    });

    connection.on("CustomerDeleted", (user, message) => {
        getdataCustomerTransactionInfos();
    });

    connection.on("CustomerUpdated", (user, message) => {
        getdataCustomerTransactionInfos();
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

async function getdataCustomerTransactionInfos() {
    fetch('http://localhost:34372/api/NonCrud/GetCustomerTransactionInfo')
        .then(x => x.json())
        .then(y => {
            CustomerTransactionInfos = y;
            console.log(CustomerTransactionInfos);
            displayCustomerTransactionInfos();
        });
}

function displayCustomerTransactionInfos() {
    document.getElementById('resultarea').innerHTML = "";
    CustomerTransactionInfos.forEach(t => {
        document.getElementById('resultarea').innerHTML +=
            " <tr><td>"
        + t.customerId + "</td><td>"
        + t.firstName + "</td><td>"
        + t.lastName + "</td><td>"
        + t.numberOfTransactions + "</td></tr >";
    });
}
