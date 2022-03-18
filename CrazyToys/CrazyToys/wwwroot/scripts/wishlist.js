function addOrRemoveFromWishlist(toyId) {
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
            alert("Wow den blev tilføjet til ønskelisten");
        } else {
            alert("Kunne ikke ligge produktet på ønskelisten");
        }
    });
}
