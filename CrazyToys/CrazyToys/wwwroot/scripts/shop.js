
var sortOption = document.getElementById("sorter").value;
console.log(sortOption);


function updateDictAndCreateUrl(paramsDict, type, param) {
    updateParamsDict(paramsDict, type, param);
    createUrlFromParams(paramsDict, type, param);
}


function createUrlFromParams(paramsDict) {

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

    // tilføj sorting til url
    url = url + sortOption;
    
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
