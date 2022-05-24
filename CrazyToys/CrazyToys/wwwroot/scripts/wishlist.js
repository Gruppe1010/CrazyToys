


function addToOrRemoveFromWishlist(toyId) {

    debugger;
 
    const wishlistImages = document.getElementsByClassName(`wishlist-img-${toyId}`);

    if (wishlistImages.length === 0) {
        // puha gør noget
        alert("Der skete en fejl");
        return;
    }

    const src = wishlistImages[0].src;
    let newSrc;

    if (src.includes("filledheart")) {
        removeFromWishlist(toyId);
        newSrc = src.replace("filledheart", "heart");

    } else {
        addToWishlist(toyId);
        newSrc = src.replace("heart", "filledheart");
    }

    Array.from(wishlistImages).forEach(imgEl => changeSrc(imgEl, newSrc));
}

function changeSrc(el, src) {
    el.src = src;
}






function addToWishlist(toyId) {

    const selectedToy = {
        toyId: toyId
    }

    fetch(`/api/sessionuser/AddToyToSessionUsersWishlist`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json; charset=UTF-8' // denne linje siger at dataen som vi sender er en string 
        },
        body: JSON.stringify(selectedToy)
    })
        .then(response => {
            if (response.ok) {
                //changeOnclickAndFillHeartShop(toyId)
                //changeOnclickAndFillHeartShopDetails(toyId)
                console.log("Toy blev tilføjet til ønskelisten");
            } else {
                console.log("Toy kunne ikke tilføjet til ønskelisten");
            }
        });
}

function removeFromWishlist(toyId) {
    debugger;

    fetch(`/api/sessionuser/RemoveToyFromSessionUsersWishlist?id=${toyId}`, {
        method: 'DELETE',
    })
        .then(response => {
            if (response.ok) {
                //changeOnclickAndUnfillHeartShop(toyId)
                //changeOnclickAndUnfillHeartShopDetails(toyId)
                removeChildNodeFromWishlist(toyId);
                console.log("Toy blev fjernet fra ønskelisten");
            } else {
                console.log("Toy kunne ikke fjernet fra ønskelisten");
            }
        });
}

// hvis toyet blev fjernet successfuldt fra sessionUsers wishlist
// så fjern "wishlist-toy-${toyId}"-elementet fra siden
function removeChildNodeFromWishlist(toyId) {

    debugger;

    var wishlistToyRow = document.getElementById('wishlist-toy-row');

    var wishlistToy = document.getElementById(`wishlist-toy-${toyId}`);

    // Hvis vi er på shop vil wishlistToy være null
    if (wishlistToy != null) {

        // Vi fjerner wishlistToy fra siden
        wishlistToyRow.removeChild(wishlistToy);

        // Hvis der ikke er flere wishlistToys skal tekst ændres
        if (wishlistToyRow.childElementCount === 0) {
            var wishlistCount = document.getElementById('wishlist-count');

            wishlistCount.innerText = "Du har ingen produkter på din ønskelisten";
        }
    }
}

function changeOnclickAndFillHeartShop(toyId) {
    var wishlistBtn = document.getElementById(`wishlist-btn-${toyId}`);
    var wishlistIcon = document.getElementById(`wishlist-icon-${toyId}`);

    if (wishlistBtn != null) {

        // Ændrer onclick og image
        Object.assign(wishlistBtn, {
            onclick: function () { removeFromWishlist(toyId) }
        });

        Object.assign(wishlistIcon, {
            src: 'img/icon/filledheart.png'
        });
    }
}

function changeOnclickAndUnfillHeartShop(toyId) {
    var wishlistBtn = document.getElementById(`wishlist-btn-${toyId}`);
    var wishlistIcon = document.getElementById(`wishlist-icon-${toyId}`);

    if (wishlistBtn != null) {

        // Ændrer onclick og image
        Object.assign(wishlistBtn, {
            onclick: function () { addToWishlist(toyId) }
        });

        Object.assign(wishlistIcon, {
            src: 'img/icon/heart.png'
        });
    }

}
function changeOnclickAndFillHeartShopDetails(toyId) {
    var wishlistLink = document.getElementById(`wishlist-link-${toyId}`)
    var wishlistImg = document.getElementById(`wishlist-img-${toyId}`)

    if (wishlistLink != null) {

        // Ændrer onclick og image
        Object.assign(wishlistLink, {
            onclick: function () { removeFromWishlist(toyId) }
        });

        Object.assign(wishlistImg, {
            src: 'img/icon/filledheart.png'
        });
    }
}

function changeOnclickAndUnfillHeartShopDetails(toyId) {
    var wishlistLink = document.getElementById(`wishlist-link-${toyId}`)
    var wishlistImg = document.getElementById(`wishlist-img-${toyId}`)

    if (wishlistLink != null) {

        // Ændrer onclick og image
        Object.assign(wishlistLink, {
            onclick: function () { addToWishlist(toyId) }
        });

        Object.assign(wishlistImg, {
            src: 'img/icon/heart.png'
        });
    }
}
