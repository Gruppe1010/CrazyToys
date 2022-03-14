﻿
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
function updateDictAndCreateUrl(pageNumber, paramsDict, type, param) {
    updateParamsDict(paramsDict, type, param);
    createUrlFromParams(pageNumber, paramsDict, type, param);
}


function createUrlFromParams(pageNumber, paramsDict) {
    debugger;
    let url = "https://localhost:44325/shop?";

    //&brand=_brand.Barbie
    // tilføj 
    for (const property in paramsDict) {
        let paramUnit = property + "=" + property; //?brand=_brand

        paramsDict[property].forEach(valueOnArray => {
            paramUnit = paramUnit + "." + valueOnArray;
        });

        url = url + paramUnit + "&";
    }

    // Finder værdien fra select (f.eks. sort=price_asc)
    var sortOption = document.getElementById("sorter").value;

    // tilføjer paging og sorting til url
    url = url + `p=${pageNumber}&` + sortOption;

    // hvis der er +'er i vores url (fx ved prisgruppen 800+) skal det encodes til %2b
    url = url.replace("+", "%2b")
 
    window.location.replace(url);
}

function updateParamsDict(paramsDict, type, param) // type == colour, param == rød
{
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
            if (paramsDict[type].length > 1) {
                // fjern den enkelte param fra arrayet
                paramsDict[type] = paramsDict[type].filter(n => n != param);
            }
            else {
                // ellers fjern hele propertien == dvs. type
                delete paramsDict[type];
            }
        }
    }
    else // tilføj nyt key-value pair
    {
        paramsDict[type] = [param];
    }

}
