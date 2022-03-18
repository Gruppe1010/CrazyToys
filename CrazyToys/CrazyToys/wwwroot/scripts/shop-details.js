
function incOrDecChosenAmount(stockAmount, inc) {
    var chosenAmount = document.getElementById('chosenAmount');
    var oldValue = chosenAmount.value;

    var newVal;

    if (inc) {
        if (oldValue < stockAmount) {
            newVal = parseFloat(oldValue) + 1;
        } else {

            newVal = oldValue;

            alert(`Der er kun ${oldValue} stk. tilbage af denne vare`);
        }

    } else {
        // Don't allow decrementing below zero
        if (oldValue > 0) {
            newVal = parseFloat(oldValue) - 1;
        } else {
            newVal = 0;
        }
    }
    chosenAmount.value = newVal;
}


function addToCart(toyId) {
    const quantity = document.getElementById('chosenAmount').value;

    const selectedToy = {
        toyId: toyId,
        quantity: quantity
    }

    fetch(`https://localhost:44325/api/sessionuser`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json; charset=UTF-8' // denne linje siger at dataen som vi sender er en string 
        },
        body: JSON.stringify(selectedToy)
    })
    .then(response => {
        if (response.ok) {
            updateCartNumber(quantity);
            alert("Wow den blev tilføjet til kurven");
        } else {
            alert("Du kan ikke lægge så meget i kurven");
        }
    });
  
}


function updateCartNumber() {
    fetch(`https://localhost:44325/api/sessionUser`, {
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

