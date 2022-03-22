function redirect(url) {
    window.location.replace(url);
}

function addToCart(toyId) {
    debugger;
    const chosenAmount = document.getElementById('chosenAmount');
    let quantity = 1;

    if (chosenAmount != null) {
        quantity = chosenAmount.value;
    } 


    const selectedToy = {
        toyId: toyId,
        quantity: quantity,
        stock: 0
    };

    fetch(`https://localhost:44325/api/sessionuser/AddToCart`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json; charset=UTF-8' // denne linje siger at dataen som vi sender er en string 
        },
        body: JSON.stringify(selectedToy)
    })
    .then(response => {
        if (response.ok) {
            updateCartNumber(quantity);
            // TODO lav noget flot javascript
        } else {
            alert("Du kan ikke lægge så meget i kurven");
        }
    });
}


function updateCartNumber() {
    fetch(`https://localhost:44325/api/sessionuser/GetSessionUser`, {
        method: 'GET'
    })
        .then(response => {
            if (response.ok) {
                return response.json();
            }
            throw new Error("Error in getting sessionUser to update cartQuantity");
        })
        .then(data => {
            document.getElementById('cartQuantity').innerText = Object.entries(data.Cart).reduce((result, item) => {
                return result + item[1];
            }, 0);
        })
        .catch(error => console.log);
}
