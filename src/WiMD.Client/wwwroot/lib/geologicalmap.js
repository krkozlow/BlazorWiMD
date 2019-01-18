var mymap;
var layerGroup;
function SetMap(latitude, longitute) {
    mymap = L.map('mapid').setView([latitude, longitute], 13);
    layerGroup = L.layerGroup().addTo(mymap);

    L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token=pk.eyJ1Ijoia3Jrb3psb3ciLCJhIjoiY2pweTlsMzBiMHIyMTQ0bXY0eHk2cGpwcSJ9.US6zUD29EZMAdpClbEwMzw', {
        attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
        maxZoom: 18,
        id: 'mapbox.streets',
        accessToken: 'your.mapbox.access.token'
    }).addTo(mymap);
}

function AddLocation(latitude, longitute, userName) {
    let marker = L.marker([latitude, longitute]).addTo(layerGroup);
}

function CleanMarkups() {
    layerGroup.clearLayers();
}
