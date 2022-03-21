
function addToWishlist(toyId) {

    const selectedToy = {
        toyId: toyId
    }

    fetch(`https://localhost:44325/api/sessionuser/AddToyToSessionUsersWishlist`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json; charset=UTF-8' // denne linje siger at dataen som vi sender er en string 
        },
        body: JSON.stringify(selectedToy)
    })
        .then(response => {
            if (response.ok) {
                changeOnclickAndFillHeartShop(toyId)
                changeOnclickAndFillHeartShopDetails(toyId)
                console.log("Toy blev tilføjet til ønskelisten");
            } else {
                console.log("Toy kunne ikke tilføjet til ønskelisten");
            }
        });
}

function removeFromWishlist(toyId) {

    fetch(`https://localhost:44325/api/sessionuser/RemoveToyFromSessionUsersWishlist?id=${toyId}`, {
        method: 'DELETE',
    })
        .then(response => {
            if (response.ok) {
                changeOnclickAndUnfillHeartShop(toyId)
                changeOnclickAndUnfillHeartShopDetails(toyId)
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
    var wishlistIl = document.getElementById(`wishlist-il-${toyId}`);
    var wishlistBtn = document.getElementById(`wishlist-btn-${toyId}`);

    if (wishlistBtn != null) {

        // Vi fjerner wishlistBtn og wishlistIcon fra siden
        wishlistIl.removeChild(wishlistBtn);

        // Laver nye btn og icon elementer
        var newWishlistBtn = document.createElement('button');
        var newWishlistIcon = document.createElement('img');

        // Tilføjer attributter til de nye elementer
        Object.assign(newWishlistBtn, {
            className: 'wishlist-btn',
            id: `wishlist-btn-${toyId}`,
            onclick: function () { removeFromWishlist(toyId) }
        });

        Object.assign(newWishlistIcon, {
            className: 'hover-icon',
            id: `wishlist-icon-${toyId}`,
            src: 'img/icon/filledheart.png',
            alt: '""'
        });

        // Appender de nye elementer
        newWishlistBtn.appendChild(newWishlistIcon);
        wishlistIl.appendChild(newWishlistBtn);
    }
}

function changeOnclickAndUnfillHeartShop(toyId) {
    var wishlistIl = document.getElementById(`wishlist-il-${toyId}`);
    var wishlistBtn = document.getElementById(`wishlist-btn-${toyId}`);

    if (wishlistBtn != null) {

        // Vi fjerner wishlistBtn og wishlistIcon fra siden
        wishlistIl.removeChild(wishlistBtn);

        // Laver nye btn og icon elementer
        var newWishlistBtn = document.createElement('button');
        var newWishlistIcon = document.createElement('img');

        // Tilføjer attributter til de nye elementer
        Object.assign(newWishlistBtn, {
            className: 'wishlist-btn',
            id: `wishlist-btn-${toyId}`,
            onclick: function () { addToWishlist(toyId) }
        });

        Object.assign(newWishlistIcon, {
            className: 'hover-icon',
            id: `wishlist-icon-${toyId}`,
            src: 'img/icon/heart.png',
            alt: '""'
        });

        // Appender de nye elementer
        newWishlistBtn.appendChild(newWishlistIcon);
        wishlistIl.appendChild(newWishlistBtn);
    }

}
function changeOnclickAndFillHeartShopDetails(toyId) {
    var shopDetailsWishlist = document.getElementById('shop-details-wishlist')
    var wishlistLink = document.getElementById(`wishlist-link-${toyId}`)


    if (wishlistLink != null) {

        shopDetailsWishlist.removeChild(wishlistLink);

        var newWishlistLink = document.createElement('a');
        var newWishlistImg = document.createElement('img');

        // Tilføjer attributter til de nye elementer
        Object.assign(newWishlistLink, {
            id: `wishlist-link-${toyId}`,
            href: 'javascript:void(0);',
            onclick: function () { removeFromWishlist(toyId) }
        });

        Object.assign(newWishlistImg, {
            className: 'shop-deatils-heart-icons',
            id: `wishlist-img-${toyId}`,
            src: 'img/icon/filledheart.png',
            alt: '""'
        });

        newWishlistLink.innerText = "Tilføj til ønskeliste "

        // Appender de nye elementer
        newWishlistLink.appendChild(newWishlistImg);
        shopDetailsWishlist.appendChild(newWishlistLink);
    }
}

function changeOnclickAndUnfillHeartShopDetails(toyId) {
    var shopDetailsWishlist = document.getElementById('shop-details-wishlist')
    var wishlistLink = document.getElementById(`wishlist-link-${toyId}`)


    if (wishlistLink != null) {

        shopDetailsWishlist.removeChild(wishlistLink);

        var newWishlistLink = document.createElement('a');
        var newWishlistImg = document.createElement('img');

        // Tilføjer attributter til de nye elementer
        Object.assign(newWishlistLink, {
            id: `wishlist-link-${toyId}`,
            href: 'javascript:void(0);',
            onclick: function () { addToWishlist(toyId) }
        });

        Object.assign(newWishlistImg, {
            className: 'shop-deatils-heart-icons',
            id: `wishlist-img-${toyId}`,
            src: 'img/icon/heart.png',
            alt: '""'
        });

        newWishlistLink.innerText = "Tilføj til ønskeliste "

        // Appender de nye elementer
        newWishlistLink.appendChild(newWishlistImg);
        shopDetailsWishlist.appendChild(newWishlistLink);
    }
}
