var map;
var Geo;

function InitLocation(sEndereco, mapDiv) {
    var llocationLat = null;
    var llocationLng = null;
    var strTmp = "";
    try { 
        Geo = new google.maps.Geocoder();
        Geo.geocode({ 'address': sEndereco },
                     function (results, status) {
                         try {
                             if (status === google.maps.GeocoderStatus.OK) {
                                 strTmp = results[0].geometry.location.toString();
                                 strTmp = strTmp.replace('(', '');
                                 strTmp = strTmp.replace(')', '');
                                 llocationLat = parseFloat(strTmp.toString().split(',')[0]);
                                 llocationLng = parseFloat(strTmp.toString().split(',')[1]);

                                 var lLatLng = new google.maps.LatLng(llocationLat, llocationLng);

                                 ShowMap(lLatLng, mapDiv);
                                 addMarkers(lLatLng);
                                 centralizarMap(llocationLat, llocationLng)
                             }
                             else {
                                // alert("Erro em Geocode: Não foi possível consultar o Google Maps, " + status);
                             }

                         } catch (erroGeo) {
                             console.log(erroGeo)
                             //alert("Erro em Geocode: " + erroGeo.Message)
                             
                         }

                     });

    } catch (erro) {
        //TODO Melhorar tratamento de erro
        // alert("Erro em InitLocation: " + erro.Message);
    }
}

function ShowMap(lLatLng, mapDiv) {
    var mapOptions = {
        center: lLatLng,
        zoom: 15,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    } 
    
    map = new google.maps.Map(mapDiv, mapOptions);
    //   google.maps.event.addListenerOnce(map, 'tilesloaded', LL);
}

function addMarkers(lLatLng) { 
    var marker = new google.maps.Marker({
        position: lLatLng,
        map: map,
        //icon: 'images/marker.png' 
        title : ""
    });
}

function centralizarMap(lLat, lLng) {
    var IECentLant = 0
    var IECentLng = 0

    if (Function('/*@cc_on return document.documentMode===10@*/')()) {
        IECentLant = 0.0055;
        IECentLng = -0.015
    }
    map.setCenter(new google.maps.LatLng(lLat + IECentLant, lLng + IECentLng)); 
}