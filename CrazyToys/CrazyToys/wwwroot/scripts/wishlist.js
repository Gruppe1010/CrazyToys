
function addToWishlist(toyId) {

    const selectedToy = {
        toyId: toyId,
    }

    fetch(`https://localhost:44325/api/sessionuser/AddOrRemoveFromWishlist`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json; charset=UTF-8' // denne linje siger at dataen som vi sender er en string 
        },
        body: JSON.stringify(selectedToy)
    })
    .then(response => {
        if (response.ok) {
            console.log("Toy blev tilføjet/fjernet til/fra ønskelisten");
        } else {
            console.log("Toy kunne ikke tilføjet/fjernet til/fra ønskelisten");
        }
    });
}

function removeFromWishlist(toyId) {

    const selectedToy = {
        toyId: toyId,
    }

    fetch(`https://localhost:44325/api/sessionuser/AddOrRemoveFromWishlist`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json; charset=UTF-8' // denne linje siger at dataen som vi sender er en string 
        },
        body: JSON.stringify(selectedToy)
    })
        .then(response => {
            if (response.ok) {
                console.log("Toy blev tilføjet/fjernet til/fra ønskelisten");
            } else {
                console.log("Toy kunne ikke tilføjet/fjernet til/fra ønskelisten");
            }
        });
}
