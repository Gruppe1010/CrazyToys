



function createUrlFromParams(type, param)
{

    let paramsDict = "";
    debugger;
    console.log(paramsDict);

    updateParamsDict(paramsDict, type, param);

    let url = "https://localhost:44325/shop";

    //?brand=_brand.Barbie
    paramsDict.forEach(paramEntry => {
        
        // do something with entry.Value or entry.Key
        let paramUnit = "?" + paramEntry.Key + "=" + paramEntry.Key; //?brand=_brand

        paramEntry.Value.forEach(paramValue => {
            paramUnit = paramUnit + "." + paramValue;
        })
        url = url + paramUnit;
    })

    return url;
}

function updateParamsDict(paramsDict, type, param) // type == colour, param == rød
{
    // hvis den allerede har den TYPE param
    if (paramsDict.ContainsKey(type)) {
        // tjek om param'en allerede står der
        if (!paramsDict[type].Contains(param)) {
            // hvis den IKKE allerde står der: tilføj
            paramsDict[type].Add(param);
        }
        else // ellers: fjern
        {
            paramsDict[type].Remove(param);

        }

    }
    else // tilføj nyt key-value pair
    {
        paramsDict.Add(type, new HashSet < string > { param });
    }

}

/*
// http://solr:8983/solr/test/select?indent=true&q.op=OR&q=brand%3A%22Barbie%22


var ulBrand = $('#ulBrand');

fetch("http://localhost:8983/solr/test/select?facet.field=brand&facet=true&indent=true&q.op=OR&q=*%3A*&rows=0")
    .then(reponse => reponse.json())
    .then(data => console.log(data));




var chosenBrand = $('.chosen-brand');
chosenBrand.on('click', function () {
    console.log("HEEEEEEJEJEJEJEJEJEj")
    // lav funktion der viser produkter ud fra hvilke brand der er trykket på
});

*/
