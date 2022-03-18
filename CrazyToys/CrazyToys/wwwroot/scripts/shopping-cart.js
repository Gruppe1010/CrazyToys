
function incAmount(shoppingCartToyDTO) {
    debugger;

    var amountElement = document.getElementById('chosenAmount');
    var oldValue = parseFloat(amountElement.value);
    var newValue = oldValue;

    if (oldValue < shoppingCartToyDTO.Stock) {

        const selectedToy = { ToyID: shoppingCartToyDTO.ID, Quantity: 1 };

        fetch(`https://localhost:44325/api/sessionuser/AddToCart`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json; charset=UTF-8' // denne linje siger at dataen som vi sender er en string 
            },
            body: JSON.stringify(selectedToy)
        }).then(response => {
            if (response.ok) {
                // hvis toyet blev incrementet successfuldt på sessionUsers cart, så vis det ude på siden
                newValue++;
                amountElement.value = newValue;
                updateTotal(shoppingCartToyDTO.ID, shoppingCartToyDTO.Price * newValue);
                updateCartTotal(newValue > oldValue ? shoppingCartToyDTO.Price : 0);
            } else {
                throw new Error("Error in incrementing toy in cart");
            }
        }).catch(error => console.log);

    } else {
        alert(`Der er kun ${oldValue} stk. tilbage af denne vare`);
    }
}

// TODO indsæt at den også skal rette i SessionUser hver gang man trykker inc eller dec

function decAmount() {

    var amountElement = document.getElementById('chosenAmount');
    var oldValue = amountElement.value;

    var newVal = oldValue > 1
        ? parseFloat(oldValue) - 1
        : 1;

    amountElement.value = newVal;

    updateCartTotal(newValue < oldValue ? -price : 0);
}

function updateTotal(id, price) {
    document.getElementById(`total-${id}`).innerText = price + " DKK";
}


function updateCartTotal(priceChange) {
    debugger;

    let subtotal = parseFloat(document.getElementById('subtotal').innerText.split(" ")[0]);
    let deliveryPrice = 39;

    subtotal = subtotal + priceChange;
    
    if (subtotal > 499.0) {
        deliveryPrice = 0.0;
    }

    const total = subtotal + deliveryPrice;

    document.getElementById('subtotal').innerText = subtotal + " DKK";
    document.getElementById('deliveryPrice').innerText = deliveryPrice < 1 ? "Gratis levering" : deliveryPrice;
    document.getElementById('total').innerText = total + " DKK";
}



function removeToyFromCart() {
    // TODO lav et kald til api/sessionUser med id'et på toy'et som skal fjernes

    // i controlleren: 
    // få fat i sessionUser
    // delete sessionUser.cart[toyId]

}