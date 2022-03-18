
function updateCartTotalPrices(shoppingCartToyDTOs) {

    let subtotal = 0;
    let deliveryPrice = 39;

    shoppingCartToyDTOs.forEach(shoppingCartToyDTO => {
        subtotal = subtotal + shoppingCartToyDTO.Price * shoppingCartToyDTO.Quantity;
    });
   
    if (subtotal > 499.0) {
        deliveryPrice = 0.0;
    }

    const total = subtotal + deliveryPrice;

    document.getElementById('subtotal').innerText = subtotal + " DKK";
    document.getElementById('deliveryPrice').innerText = deliveryPrice < 1 && "Gratis levering";
    document.getElementById('total').innerText = total + " DKK";
}



function removeToyFromCart() {
    // TODO lav et kald til api/sessionUser med id'et på toy'et som skal fjernes

    // i controlleren: 
        // få fat i sessionUser
        // delete sessionUser.cart[toyId]

}

/*
var proQty = $('.pro-qty-2');
proQty.prepend('<span class="fa fa-angle-left dec qtybtn"></span>');
proQty.append('<span class="fa fa-angle-right inc qtybtn"></span>');
proQty.on('click', '.qtybtn', function () {
    var $button = $(this);
    var oldValue = $button.parent().find('input').val();
    if ($button.hasClass('inc')) {
        var newVal = parseFloat(oldValue) + 1;
    } else {
        // Don't allow decrementing below zero
        if (oldValue > 0) {
            var newVal = parseFloat(oldValue) - 1;
        } else {
            newVal = 0;
        }
    }
    $button.parent().find('input').val(newVal);
});
*/


function incAmount(stockAmount) {

    var amountElement = document.getElementById('chosenAmount');
    var oldValue = amountElement.value;

    var newVal = oldValue < stockAmount
        ? parseFloat(oldValue) + 1
        : oldValue;

    newVal == oldValue && alert(`Der er kun ${oldValue} stk. tilbage af denne vare`);

    amountElement.value = newVal;
}

function decAmount() {

    var amountElement = document.getElementById('chosenAmount');
    var oldValue = amountElement.value;

    var newVal = oldValue > 0
        ? parseFloat(oldValue) - 1
        : 0;

    amountElement.value = newVal;
}
