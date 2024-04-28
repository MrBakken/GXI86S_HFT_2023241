fetch('http://localhost:34372/customer')
    .then(x => x.json())
    .then(y => {
        customers = y;
        console.log(customers);
        display();
    });

function display() {
    customers.forEach(t => {
        document.getElementById('resultarea').innerHTML +=
            "<tr><td>" + t.id + "</td><td>"
            + t.firstName + "</td><td>"
            + t.lastName + "</td><td>"
            + t.email + "</td><td>"
            + t.gender + "</td><td>"
            + t.birthDate + "</td><td>"
            + t.phone + "</td></tr>";
    });
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
            .then(data => {console.log('Success:', data);})
            .catch((error) => { console.error('Errorvan:', error); });

    
}