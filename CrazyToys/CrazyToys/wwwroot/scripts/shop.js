

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
