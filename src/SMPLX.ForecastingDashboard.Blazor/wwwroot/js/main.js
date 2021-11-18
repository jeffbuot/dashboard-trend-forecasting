var map,legend,heatmapLayer,baseLayer;
window.initMap = (heatdata) => {
    if (!document.getElementById("hmap")) return;
    let mapData = {
        data: heatdata
    };

    if (!map) {
        baseLayer = L.tileLayer(
            'http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, ' +
                    '<a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © ' +
                    '<a href="http://cloudmade.com">CloudMade</a>',
                maxZoom: 14
            }
        );
        map = new L.Map('hmap', {
            center: new L.LatLng(7.05626, 126.06125),//,7.092015, 126.105506
            zoom: 12,
            layers: [baseLayer]
        });
    }
    //      L.popup()
    // .setLatLng([ 6.961836,126.012958])
    // .setContent("POBLACION")
    // .openOn(map);
    if (legend) {
        legend.remove();
    }
    legend = L.control({ position: "bottomleft" });

    var total = 0;
    legend.onAdd = function (map) {
        var div = L.DomUtil.create("div", "legend");
        div.innerHTML += "<h4>Barangays</h4>";
        let ht = '<table>';
        heatdata.forEach(h => {
            total += h.count;
            ht += '<tr><td>' + h.count + '</td><td>' + h.name + '</td></tr>';
        });
        ht += '<tr><td>' + total + '</td><td>Total</td></tr></table>';
        div.innerHTML += ht;
        return div;
    }
    legend.addTo(map);

    if (heatmapLayer) {
        map.removeLayer(heatmapLayer);
    }
    if (total > 0) {
        heatmapLayer = new HeatmapOverlay({
            // radius should be small ONLY if scaleRadius is true (or small radius is intended)
            "radius": .009,
            "maxOpacity": 1,
            // scales the radius based on map zoom
            "scaleRadius": true,
            // if set to false the heatmap uses the global maximum for colorization
            // if activated: uses the data maximum within the current map boundaries 
            //   (there will always be a red spot with useLocalExtremas true)
            "useLocalExtrema": true,
            // which field name in your data represents the latitude - default "lat"
            latField: 'lat',
            // which field name in your data represents the longitude - default "lng"
            lngField: 'lng',
            // which field name in your data represents the data value - default "value"
            valueField: 'count'
        });
        heatmapLayer.setData(mapData);
        map.addLayer(heatmapLayer);
    }
    map.invalidateSize();
}