let CustomerTransactionInfos = [];
let CustomersWithAccountsAndTransactions = [];
let CustomerTransactionDetails = [];
let TotalSpendingLast30Days = [];
let LastIncomePerCustomer = [];
let connection = null;
getdatacrud();
setupSignalR();

function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:34372/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("TransactionCreated", (user, message) => {
        getdatacrud();
    });

    connection.on("TransactionDeleted", (user, message) => {
        getdatacrud();
    });

    connection.on("TransactionUpdated", (user, message) => {
        getdatacrud();
    });
    connection.on("AccountCreated", (user, message) => {
        getdatacrud();
    });

    connection.on("AccountDeleted", (user, message) => {
        getdatacrud();
    });

    connection.on("AccountUpdated", (user, message) => {
        getdatacrud();
    });
    connection.on("CustomerCreated", (user, message) => {
        getdatacrud();
    });

    connection.on("CustomerDeleted", (user, message) => {
        getdatacrud();
    });

    connection.on("CustomerUpdated", (user, message) => {
        getdatacrud();
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
async function getdatacrud() {
    getdataCustomerTransactionInfos();
    getdataCustomersWithAccountsAndTransactions();
    getdataCustomerTransactionDetails();
    getdataTotalSpendingLast30Days();
    getdataLastIncomePerCustomer();
}
async function getdataCustomerTransactionInfos() {
    fetch('http://localhost:34372/api/NonCrud/GetCustomerTransactionInfo')
        .then(x => x.json())
        .then(y => {
            CustomerTransactionInfos = y;
            console.log(CustomerTransactionInfos);
            displayCustomerTransactionInfos();
        });
}
async function getdataCustomersWithAccountsAndTransactions() {
    fetch('http://localhost:34372/api/NonCrud/GetCustomersWithAccountsAndTransactions')
        .then(x => x.json())
        .then(y => {
            CustomersWithAccountsAndTransactions = y;
            console.log(CustomersWithAccountsAndTransactions);
            displayCustomersWithAccountsAndTransactions();
        });
}
async function getdataCustomerTransactionDetails() {
    fetch('http://localhost:34372/api/NonCrud/GetCustomerTransactionDetails')
        .then(x => x.json())
        .then(y => {
            CustomerTransactionDetails = y;
            console.log(CustomerTransactionDetails);
            displayCustomerTransactionDetails();
        });
}
async function getdataTotalSpendingLast30Days() {
    fetch('http://localhost:34372/api/NonCrud/GetTotalSpendingLast30Days')
        .then(x => x.json())
        .then(y => {
            TotalSpendingLast30Days = y;
            console.log(TotalSpendingLast30Days);
            displayTotalSpendingLast30Days();
        });
}
async function getdataLastIncomePerCustomer() {
    fetch('http://localhost:34372/api/NonCrud/GetLastIncomePerCustomer')
        .then(x => x.json())
        .then(y => {
            LastIncomePerCustomer = y;
            console.log(LastIncomePerCustomer);
            displayLastIncomePerCustomer();
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
function displayCustomersWithAccountsAndTransactions() {
    document.getElementById('resultareaCustomersWithAccountsAndTransactions').innerHTML = "";
    CustomersWithAccountsAndTransactions.forEach(t => {
        let accountsHTML = "";
        t.accounts.forEach(account => {
            accountsHTML += account.accountNumber + " -- " + account.transactionCount + "<br>";
        });
        document.getElementById('resultareaCustomersWithAccountsAndTransactions').innerHTML +=
            " <tr><td>"
            + t.customerId + "</td><td>"
            + t.firstName + "</td><td>"
            + t.lastName + "</td><td>"
            + accountsHTML + "</td></tr >";
    });
}

function displayCustomerTransactionDetails() {
    document.getElementById('resultareaCustomerTransactionDetails').innerHTML = "";
    CustomerTransactionDetails.forEach(t => {
        let currencyType = "EUR";
        if (t.currencyType) {
            currencyType = "HUF";
        }
        let accountType = "Current";
        if (t.accountType) {
            accountType = "Savings";
        }
        document.getElementById('resultareaCustomerTransactionDetails').innerHTML +=
            " <tr><td>"
        + t.customerName + "</td><td>"
        + t.totalTransactionAmount + "</td><td>"
        + t.accountid + "</td><td>"
        + currencyType + "</td><td>"
        + accountType + "</td></tr >";
    });
}
function displayTotalSpendingLast30Days() {
    document.getElementById('resultareaTotalSpendingLast30Days').innerHTML = "";
    TotalSpendingLast30Days.forEach(t => {
        document.getElementById('resultareaTotalSpendingLast30Days').innerHTML +=
            " <tr><td>"
            + t.customerId + "</td><td>"
            + t.customerName + "</td><td>"
            + t.totalSpending + "</td></tr >";
    });
}

function displayLastIncomePerCustomer() {
    document.getElementById('resultareaLastIncomePerCustomer').innerHTML = "";
    LastIncomePerCustomer.forEach(t => {
        document.getElementById('resultareaLastIncomePerCustomer').innerHTML +=
            " <tr><td>"
        + t.customerName + "</td><td>"
        + t.lastIncomeAmount + "</td><td>"
        + t.currencyType + "</td></tr >";
    });
}