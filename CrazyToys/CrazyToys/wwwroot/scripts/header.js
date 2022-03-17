const cartQuantity = document.getElementById('cartQuantity');


fetch(`https://localhost:44325/api/sessionUser`, {
    method: 'GET',
    headers: {
        'Content-Type': 'application/json; charset=UTF-8' // denne linje siger at dataen som vi sender er en string 
    }
}).then(response => {
    if (!response.ok) {
        alert("404 - getting cartQuantity");
        throw new Error('Error getting cartQuantity from /api/sessionUser')
    }
    return response.json();
}).then(data => {
    if (Object.keys(data.Cart).length > 0) {
        // TODO test at det virker!
        // plusser alle entries i dictets quantity-værdier sammen
        cartQuantity.innerText = Object.entries(data.Cart).reduce((result, item) => {
            console.log("item", item)

            console.log("result", result)
            return result + item[1];
        }, 0);
    } else {
        cartQuantity.innerText = 0;
    }
});

