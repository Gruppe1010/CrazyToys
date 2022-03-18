
// TODO test ordentligt
function incQuantity(shoppingCartToyDTO) {
    var amountElement = document.getElementById(`chosenAmount-${shoppingCartToyDTO.ID}`);
    var oldValue = parseFloat(amountElement.value);
    var newValue = oldValue;

    if (oldValue < shoppingCartToyDTO.Stock) {

        const selectedToy = { ToyID: shoppingCartToyDTO.ID, Quantity: 1 };

        fetch(`https://localhost:44325/api/sessionuser`, {
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

// TODO få den til at rette i sessionUser når man trykker dec
function decQuantity(shoppingCartToyDTO) {

    var amountElement = document.getElementById(`chosenAmount-${shoppingCartToyDTO.ID}`);
    var oldValue = parseFloat(amountElement.value);
    var newValue = oldValue;

    if (oldValue > 1) {

        const selectedToy = { ToyID: shoppingCartToyDTO.ID, Quantity: -1 };

        // fjern én fra quantity på sessionsUser
        fetch(`https://localhost:44325/api/sessionuser/AddToCart`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json; charset=UTF-8' // denne linje siger at dataen som vi sender er en string 
            },
            body: JSON.stringify(selectedToy)
        }).then(response => {
            if (response.ok) {
                // hvis toyet blev decrementet successfuldt på sessionUsers cart, så vis det ude på siden
                newValue--;
                amountElement.value = newValue;
                updateTotal(shoppingCartToyDTO.ID, shoppingCartToyDTO.Price * newValue);
                updateCartTotal(newValue < oldValue ? -shoppingCartToyDTO.Price : 0);
            } else {
                throw new Error("Error in incrementing toy in cart");
            }
        }).catch(error => console.log);
    }
}

function updateTotal(id, price) {
    document.getElementById(`total-${id}`).innerText = price + " DKK";
}


function updateCartTotal(priceChange) {

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



function removeToyFromCart(shoppingCartToyDTO) {

    // fjern én fra quantity på sessionsUser
    fetch(`https://localhost:44325/api/sessionuser/RemoveToyFromSessionUser?id=${shoppingCartToyDTO.ID}`, {
        method: 'DELETE'
    }).then(response => {
        if (response.ok) {
            debugger;

            // find ud af hvor mange der har stået
            var quantity = parseFloat(document.getElementById('chosenAmount').value);

            // hvis toyet blev fjernet successfuldt fra sessionUsers cart
            // så fjern "toyDataRow-@toyDTO.ID"-elementet fra siden
            var toyTableBody = document.getElementById('toyTableBody');

            var toyDataRow = document.getElementById(`toyDataRow-${shoppingCartToyDTO.ID}`);

            toyTableBody.removeChild(toyDataRow);

            //hvis toyTableBody-element ikke har nogen childNodes
            if (toyTableBody.childElementCount === 0) {
                // slet toyTable-element
                var tableWrapper = document.getElementById('tableWrapper');

                var toyTable = document.getElementById('toyTable');
                tableWrapper.removeChild(toyTable);

                // og tilføj "der er ikke noget i kurven"-besked
                var h2 = document.createElement('h2');
                h2.classList.add('headline');
                h2.innerText = "Der er intet i kurven endnu";

                tableWrapper.appendChild(h2)
            }
            // opdater updateCartTotal(shoppingCartToyDTO)
            updateCartTotal(-shoppingCartToyDTO.Price * quantity);

        } else {
            alert("Der skete en fejl");
            throw new Error("Error in removing toy from cart");
        }
    }).catch(error => console.log);
    // TODO lav et kald til api/sessionUser med id'et på toy'et som skal fjernes

    // i controlleren: 
    // få fat i sessionUser
    // delete sessionUser.cart[toyId]

}