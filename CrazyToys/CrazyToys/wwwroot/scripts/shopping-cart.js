const toyTableBody = document.getElementById('toyTableBody');

if (toyTableBody != null && toyTableBody.childElementCount === 0) {
    const toyTable = document.getElementById('toyTable');
    
    toyTable.parentElement.removeChild(toyTable);
}

// TODO test ordentligt
function incQuantity(id) { // shoppingCartToyDTO) {

    var amountElement = document.getElementById(`chosenAmount-${id}`);
    var oldValue = parseFloat(amountElement.value);
    var newValue = oldValue;

    const selectedToy = {
        ToyId: id,
        QuantityToAdd: 1,
        OldAvailableQuantity: oldValue
    };

    fetch("/api/sessionuser/IncOrDecToyFromCart", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json; charset=UTF-8' // denne linje siger at dataen som vi sender er en string 
            },
        body: JSON.stringify(selectedToy)
    })
        .then(async response => {

        if (response.ok) {
            // hvis toyet blev incrementet successfuldt på sessionUsers cart, så vis det ude på siden
            const shoppingCartToyDTO = await response.json();

            newValue = shoppingCartToyDTO.quantity;
            amountElement.value = newValue;
            updateTotal(id, shoppingCartToyDTO.price * newValue);
            updateCartTotal(newValue > oldValue ? shoppingCartToyDTO.price : 0);
            updateCartNumber();

            // fjern unavailable fra view
            deleteUnavailableToyRowFromView(id);
        } else {
            const shoppingCartToyDTO = await response.json();

            if (shoppingCartToyDTO.hasOwnProperty('id')) {

                const stock = shoppingCartToyDTO.quantity;
                if (stock > 0) {
                    alert(`Der er kun ${shoppingCartToyDTO.quantity} stk. tilbage af ${shoppingCartToyDTO.name}`);

                } else {
                    alert(`${shoppingCartToyDTO.name} er desværre udsolgt`);
                }

                window.location.replace("/shopping-cart");

            }
            else {
                throw new Error("Error in incrementing toy in cart")
            }
        }

    })
    .catch(error => console.log);
}

// TODO få den til at rette i sessionUser når man trykker dec
function decQuantity(id) { //

    var amountElement = document.getElementById(`chosenAmount-${id}`);
    var oldValue = parseFloat(amountElement.value);
    var newValue = oldValue;

    if (oldValue > 1) {

        const selectedToy = {
            ToyID: id,
            QuantityToAdd: - 1,
            OldAvailableQuantity: oldValue
        };

        // fjern én fra quantity på sessionsUser
        fetch(`/api/sessionuser/IncOrDecToyFromCart`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json; charset=UTF-8' // denne linje siger at dataen som vi sender er en string 
            },
            body: JSON.stringify(selectedToy)
        }).then(async response => {
            if (response.ok) {
                // hvis toyet blev decrementet successfuldt på sessionUsers cart, så vis det ude på siden
                const shoppingCartToyDTO = await response.json();

                newValue = shoppingCartToyDTO.quantity;
                amountElement.value = newValue;
                updateTotal(id, shoppingCartToyDTO.price * newValue);
                updateCartTotal(newValue < oldValue ? shoppingCartToyDTO.price : 0);
                updateCartNumber();

                // fjern unavailable fra view
                deleteUnavailableToyRowFromView(id);
            } else {
                const shoppingCartToyDTO = await response.json();

                if (shoppingCartToyDTO.hasOwnProperty('id')) {

                    const stock = shoppingCartToyDTO.quantity;
                    if (stock > 0) {
                        alert(`Der er kun ${shoppingCartToyDTO.stock} stk. tilbage af ${shoppingCartToyDTO.name}`);

                    } else {
                        alert(`${shoppingCartToyDTO.name} er desværre udsolgt`);
                    }

                    updateCartNumber();

                    window.location.replace("/shopping-cart");

                }
                else {
                    throw new Error("Error in decrementing toy in cart")
                }
            }
        }).catch(error => console.log);
    }
}

function goToCheckout() {
    fetch(`/api/sessionuser/CheckCart`, {
        method: 'GET'
    }).then(async response =>  {
        if (response.ok) {
            window.location.replace("/checkout")
        } else {
            const errorMessage = await response.text(); // denne her henter fejlbeskeden som vi har sendt med fra controlleren
            alert(errorMessage);
            window.location.replace("/shopping-cart"); // vi redirecter til samme side, fordi så viser den de toys som er unavailable (hvis det er det der er fejlen)
        }
    }).catch(error => {
        console.log("error: ", error.message)
        debugger;
    });
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
    const priceToFreeShipping = 499 - subtotal;

    const text = priceToFreeShipping > 0
        ? priceToFreeShipping + " DKK til gratis fragt"
        : "";

    document.getElementById('subtotal').innerText = subtotal + " DKK";
    document.getElementById('deliveryPrice').innerText = deliveryPrice < 1 ? "Gratis levering" : deliveryPrice + " DKK";
    document.getElementById('total').innerText = total + " DKK";
    document.getElementById('priceToFreeShipping').innerText = text;
}

function removeUnavailbleToyFromCart(id, quantityToRemove) {

    const selectedToy = { ToyID: id, Quantity: -quantityToRemove };

    // fjern én fra quantity på sessionsUser
    fetch(`/api/sessionuser/AddToCart`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json; charset=UTF-8' // denne linje siger at dataen som vi sender er en string 
        },
        body: JSON.stringify(selectedToy)
    }).then(response => {
        if (response.ok) {
            // hvis toyet blev decrementet successfuldt på sessionUsers cart, så slet unavailbaleToyRow
            deleteUnavailableToyRowFromView(id);
            updateCartNumber();
        } else {
            throw new Error("Error in removing unavailable toy in cart");
        }
    }).catch(error => console.log);
}

function removeToyFromCart(id, price) { //

    // fjern én fra quantity på sessionsUser
    fetch(`/api/sessionuser/RemoveToyFromSessionUser?id=${id}`, {
        method: 'DELETE'
    }).then(response => {
        if (response.ok) {

            // find ud af hvor mange der har stået
            var quantity = parseFloat(document.getElementById(`chosenAmount-${id}`).value);

            // fjern fra view
            deleteToyRowFromView(id);
            deleteUnavailableToyRowFromView(id);


            // opdater updateCartTotal(shoppingCartToyDTO)
            updateCartTotal(-price * quantity);
            updateCartNumber();

        } else {
            alert("Der skete en fejl");
            throw new Error("Error in removing toy from cart");
        }
    }).catch(error => console.log);
}

function deleteToyRowFromView(id) {

    // hvis toyet blev fjernet successfuldt fra sessionUsers cart
    // så fjern rækken ("toyDataRow-@toyDTO.ID")
    const toyDataRow = document.getElementById(`toyDataRow-${id}`);

    if (toyDataRow != null) {
        toyDataRow.parentElement.removeChild(toyDataRow);
    }

    // hvis der ikke er flere toys i tabellen efter dette er blevet fjernet, så fjern hele tabellen
    deleteToyTableIfEmpty();
}

function deleteToyTableIfEmpty() {

    //hvis toyTableBody-element ikke har nogen childNodes
    if (toyTableBody.childElementCount === 0) {
        // slet toyTable-element
        var tableWrapper = document.getElementById('tableWrapper');

        var toyTable = document.getElementById('toyTable');
        tableWrapper.removeChild(toyTable);

        // hvis der ikke er nogen Ikke tilgængelig varer skal den lave "der er ingenting i kurven" overskriften
        var unavailableToyTable = document.getElementById('unavailableToyTable');

        if (unavailableToyTable == null) {

            // og tilføj "der er ikke noget i kurven"-besked
            var h2 = document.createElement('h2');
            h2.classList.add('headline');
            h2.setAttribute("id", "mainEmptyCartHeadline");
            h2.innerText = "Der er intet i indkøbskurven endnu";

            tableWrapper.appendChild(h2)
        }
    }
}

function deleteUnavailableToyRowFromView(id) {

    // hvis samme type toy er nede i "Ikke tilgængelige", skal den også slettes
    const unavailbaleToyDataRow = document.getElementById(`unavailableToyDataRow-${id}`);
    if (unavailbaleToyDataRow != null) {
        unavailbaleToyDataRow.parentElement.removeChild(unavailbaleToyDataRow);
    }

    deleteUnavailbaleToyTableIfEmpty();
}

function deleteUnavailbaleToyTableIfEmpty() {

    const unavailableToyTableBody = document.getElementById('unavailableToyTableBody');

    //hvis toyTableBody-element ikke har nogen childNodes
    if (unavailableToyTableBody != null && unavailableToyTableBody.childElementCount === 0) {
        // gør hidden
        var unavailableToyTable = document.getElementById('unavailableToyTable');
        unavailableToyTable.parentElement.removeChild(unavailableToyTable);
    }

    var mainEmptyCartHeadline = document.getElementById('mainEmptyCartHeadline');
    var toyTable = document.getElementById('toyTable');

    if (toyTable == null && mainEmptyCartHeadline == null) {

        var tableWrapper = document.getElementById('tableWrapper');
        
        // og tilføj "der er ikke noget i kurven"-besked
        var h2 = document.createElement('h2');
        h2.classList.add('headline');
        h2.innerText = "Der er intet i indkøbskurven endnu";

        tableWrapper.appendChild(h2)
    }
}
