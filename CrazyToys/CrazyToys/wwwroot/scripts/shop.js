
function addSortToUrl(paramsDict) {

}


function updateDictAndCreateUrl(paramsDict, type, param, sorting) {
    updateParamsDict(paramsDict, type, param);
    createUrlFromParams(paramsDict, type, param, sorting);
}


function createUrlFromParams(paramsDict, sorting) {
    //updateParamsDict(paramsDict, type, param);

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

    url = url.charAt(url.length - 1) == "&"
            ? url.substring(0, url.length - 1)
        : url;

    // tilføj sorting til url
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
