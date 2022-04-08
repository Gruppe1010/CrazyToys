
window.onload = function () {
    setSorter();
}

function setSorter() {
    // Henter URL og select
    var currentUrl = window.location.href;
    var sorterSelect = document.getElementById("sorter");
    // Sætter substring
    var currentUrlSubstring = "sort=price_desc"

    if (currentUrl.includes("shop?")) {
        // Ændre afhængig af om substring er en del af URL
        if (currentUrl.includes(currentUrlSubstring)) {
            sorterSelect.value = "sort=price_desc";
            // Opdatere nice-select value
            $("#sorter").val("sort=price_desc").niceSelect('update');
            
        } else {
            sorterSelect.value = "";
            // Opdaterer nice-select value
            $("#sorter").val("").niceSelect('update');
        }
    }
}

/** 
 *  Hvis man har trykket på én af filtreringsmulighederne ude til venstre på shop-siden, kaldes denne metode
 *  Opdaterer dictionary<string, set<string>>, som indeholder de forskellige typer af parametre og selve parameter-værdierne
 *      paramsDict == den nuværende dictionary med de filtreringsmuligheder som er valgt af brugeren
 *      type == typen af filtreringen som brugeren netop har trykket på
 *          fx: "categories"
 *      param == selve værdiern af det som brugeren har trykket på
 *          fx "Barbie"
 *          
 *  Først opdateres paramsDictet og derefter dannes urlen (ud fra paramsDictet) til at ramme vores shop-controller med de rette parans
 */
function updateDictAndCreateUrl(paramsDict, type, param) {
    updateParamsDict(paramsDict, type, param);
    // pageNumber-paramet som vi giver med er hardcodet til 1 fordi hver gang man tilføjer en filtrering, 
    //får vi jo vist nogle andre produkter, og så vil vi hen til forsiden igen
    createUrlFromParams(1, paramsDict);
}


function createUrlFromParams(pageNumber, paramsDict, event) {

    let url = "/shop";

    // search sættes default til "" og hvis der så er noget i inputfeltet, så kommes det ind i stedet
    let search = "";
    if (event != undefined) {
        // søgning i lille søgefelt
        const searchInputFromShop = document.getElementById('searchInput');
        const searchInputFromSearchBar = document.getElementById('searchInput1');

        if (searchInputFromShop && searchInputFromShop.value) { // hvis der er blevet søgt i det lille felt, så tag værdien derfra
            search = `search=${searchInputFromShop.value}&`;
        }
        else if (searchInputFromSearchBar && searchInputFromSearchBar.value) { // hvis der er blevet søgt i det store felt
            search = `search=${searchInputFromSearchBar.value}&`;
        }
    }
  

    // sider
    const pageParam = pageNumber == 1 ? "" : `&p=${pageNumber}`;
    let sortOption = "";
    // Finder værdien fra select (f.eks. sort=price_asc)
    var sorter = document.getElementById("sorter");
    if (sorter && sorter.value) {
        sortOption = sorter.value;
    }
    debugger;


    url = Object.keys(paramsDict).length != 0 || pageParam != "" || sortOption != "" || search
        ? url + "?" + search
        : url;


    


    //&brand=_brand.Barbie
    // tilføj 
    for (const property in paramsDict) {
        let paramUnit = property + "=" + property; //?brand=_brand

        paramsDict[property].forEach(valueOnArray => {
            paramUnit = paramUnit + "." + valueOnArray;
        });

        url = url + paramUnit + "&";
    }

    // tilføjer paging og sorting til url
    url = url + pageParam + (sortOption != "" && pageParam != ""
        ? "&" + sortOption
        : sortOption);

    // hvis der ikke er blevet tilføjet paging eller sorting, men der ER blevet tilføjet params, så står der et & til sidst som skal fjernes
    url = url.charAt(url.length - 1) == "&"
        ? url.substr(0, url.length - 1)
        : url;

    // hvis der er +'er i vores url (fx ved prisgruppen 800+) skal det encodes til %2b
    url = url.replace("+", "%2b")


    window.location.replace(url);
}

// type == categories
// param == Dukker
function updateParamsDict(paramsDict, type, param) // type == colour, param == rød
{
    // hvis type == subcategory

    // læg på Dict på en specielt måde

    // hvis den allerede har den TYPE param
    if (paramsDict.hasOwnProperty(type)) {
        // tjek om param'en IKKE allerede står der
        if (!paramsDict[type].includes(param)) {
            // hvis den IKKE allerde står der: tilføj
            paramsDict[type].push(param);
        }
        else // ellers: fjern
        {
            // hvis der er flere end den ene param tilbage på arrayet
            paramsDict[type].length > 1
                ? paramsDict[type] = paramsDict[type].filter(n => n != param) // fjern den enkelte param fra arrayet
                : delete paramsDict[type]; // fjern den enkelte param fra arrayet

            // hvis type der skal fjernes er categories skal alle dens subcategories også fjernes
            if (type == "categories") {
                // hvis der er nogle subcats på dictet
                if (paramsDict.hasOwnProperty("subCategory")) {
                    // så gennemgå alle subcategories på paramsDict og slet dem som har [0] == param

                    paramsDict.subCategory.forEach(subCat => {
                        subCat[0] == type && delete subCat;
                    })

                }
            }
        }
    }
    else // tilføj nyt key-value pair
    {
        paramsDict[type] = [param];
    }

}
