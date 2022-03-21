const toyTableBody = document.getElementById('toyTableBody');

if (toyTableBody.childElementCount === 0) {

    debugger;
    const tHead = document.getElementById('tHead');

    tHead.parentElement.removeChild(tHead);
}



// TODO test ordentligt
function incQuantity(id, stock, price) { // shoppingCartToyDTO) {

    debugger;
    var amountElement = document.getElementById(`chosenAmount-${id}`);
    var oldValue = parseFloat(amountElement.value);
    var newValue = oldValue;

    if (oldValue < stock) {

        const selectedToy = { ToyID: id, Quantity: 1 };

        fetch("https://localhost:44325/api/sessionuser/AddToCart", {
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
                updateTotal(id, price * newValue);
                updateCartTotal(newValue > oldValue ? price : 0);
                updateCartNumber();
            } else {
                throw new Error("Error in incrementing toy in cart");
            }
        }).catch(error => console.log);

    } else {
        alert(`Der er kun ${oldValue} stk. tilbage af denne vare`);
    }
}

// TODO få den til at rette i sessionUser når man trykker dec
function decQuantity(id, stock, price) { //

    var amountElement = document.getElementById(`chosenAmount-${id}`);
    var oldValue = parseFloat(amountElement.value);
    var newValue = oldValue;

    if (oldValue > 1) {

        const selectedToy = { ToyID: id, Quantity: -1 };

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
                updateTotal(id, price * newValue);
                updateCartTotal(newValue < oldValue ? -price : 0);
                updateCartNumber();

                // hvis den også er på ikke-tilgængelig listen og den nye værdie er indenfor available stock
                // TODO overvej at lave et kald ned for at få stock - fordi denne stock hentes kun når vi går ind i kurven og så opdateres den ikke derfra - og det er ikke superrr godt
                const unavailbaleToyDataRow = document.getElementById(`unavailableToyDataRow-${id}`);
                if (unavailbaleToyDataRow != null && newValue <= stock) {
                    deleteUnavailableToyRowFromView(id);
                }


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
    fetch(`https://localhost:44325/api/sessionuser/AddToCart`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json; charset=UTF-8' // denne linje siger at dataen som vi sender er en string 
        },
        body: JSON.stringify(selectedToy)
    }).then(response => {
        if (response.ok) {
            // hvis toyet blev decrementet successfuldt på sessionUsers cart, så slet unavailbaleToyRow
            deleteUnavailableToyRowFromView(id);
        } else {
            throw new Error("Error in removing unavailable toy in cart");
        }
    }).catch(error => console.log);


}


function removeToyFromCart(id, stock, price) { //

    // fjern én fra quantity på sessionsUser
    fetch(`https://localhost:44325/api/sessionuser/RemoveToyFromSessionUser?id=${id}`, {
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

    debugger;
    // hvis toyet blev fjernet successfuldt fra sessionUsers cart
    // så fjern rækken ("toyDataRow-@toyDTO.ID")
    const toyDataRow = document.getElementById(`toyDataRow-${id}`);

    toyDataRow.parentElement.removeChild(toyDataRow);

    // hvis der ikke er flere toys i tabellen efter dette er blevet fjernet, så fjern hele tabellen
    deleteToyTableIfEmpty();
}

function deleteUnavailableToyRowFromView(id) {
    // hvis samme type toy er nede i "Ikke tilgængelige", skal den også slettes
    const unavailbaleToyDataRow = document.getElementById(`unavailableToyDataRow-${id}`);
    if (unavailbaleToyDataRow != null) {
        unavailbaleToyDataRow.parentElement.removeChild(unavailbaleToyDataRow);
    }

    deleteUnavailbaleToyTableIfEmpty();
}


function deleteToyTableIfEmpty() {

    //hvis toyTableBody-element ikke har nogen childNodes
    if (toyTableBody.childElementCount === 0) {
        // slet toyTable-element
        var tableWrapper = document.getElementById('tableWrapper');

        var toyTable = document.getElementById('toyTable');
        tableWrapper.removeChild(toyTable);

        // og tilføj "der er ikke noget i kurven"-besked
        var h2 = document.createElement('h2');
        h2.classList.add('headline');
        h2.innerText = "Der er intet i indkøbskurven endnu";

        tableWrapper.appendChild(h2)
    }

}


function deleteUnavailbaleToyTableIfEmpty() {
    const unavailableToyTableBody = document.getElementById('unavailableToyTableBody');

    //hvis toyTableBody-element ikke har nogen childNodes
    if (unavailableToyTableBody != null && unavailableToyTableBody.childElementCount === 0) {
        // slet toyTable-element
        var unavailableToyTable = document.getElementById('unavailableToyTable');
        unavailableToyTable.parentElement.removeChild(unavailableToyTable);
    }
}

