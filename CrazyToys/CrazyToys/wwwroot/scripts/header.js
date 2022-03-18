const cartQuantity = document.getElementById('cartQuantity');


fetch(`https://localhost:44325/api/sessionUser/GetSessionUser`, {
    method: 'GET',
}).then(response => {
    if (!response.ok) {
        alert("404 - getting cartQuantity");
        throw new Error('Error getting cartQuantity from /api/sessionUser/GetSessionUser')
    }
    return response.json();
}).then(data => {
    if (Object.keys(data.Cart).length > 0) {
        cartQuantity.innerText = Object.entries(data.Cart).reduce((result, item) => {
            return result + item[1];
        }, 0);
    } else {
        cartQuantity.innerText = 0;
    }
}).catch(error => console.log);

