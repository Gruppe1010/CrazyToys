function shipOrder(orderId) {
    var currentUrl = window.location.href;

    fetch(`/api/admin/ShipOrder?orderId=${orderId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json; charset=UTF-8' // denne linje siger at dataen som vi sender er en string 
        }
    })
    .then(async response => {
        if (response.ok) {
            alert("Ordren er nu markeret som afsendt")
            window.location.replace(`${currentUrl}`);
        } else {
            alert("Puha upsi skete en fejl")
        }
    })
    .catch(error => console.log);
}