
var Mapa;

var Marker;

var latitudRegisterProd;
var longitudRegisterProd;

navigator.geolocation.getCurrentPosition(localizacion, error);

function localizacion(posicion) {
  latitudRegisterProd = posicion.coords.latitude;
  longitudRegisterProd = posicion.coords.longitude;
  cargarMapa();
}

function error() {
  out.innerHTML = "No se pudo obtener tu coordenada!";
}

function cargarMapa() {
  var mapConfig = {
    center: new google.maps.LatLng(latitudRegisterProd, longitudRegisterProd),
    zoom: 13
  };
  var out = document.getElementById("Mapa");
  //Renderiza un GoogleMap dentro de out con la carateristica mapConfig.   
  Mapa = new google.maps.Map(out, mapConfig);
  var input = document.getElementById('pac-input');
      var options = {
        componentRestrictions: {city:'Buenos Aires'}
      };
    
    Mapa.controls[google.maps.ControlPosition.TOP_LEFT].push(input);
    var autocomplete = new google.maps.places.Autocomplete(input, options);
    autocomplete.bindTo('bounds', Mapa);

    var infowindow = new google.maps.InfoWindow();
    var marker = new google.maps.Marker({
      map: Mapa,
      anchorPoint: new google.maps.Point(0, -29)
    });
  
    autocomplete.addListener('place_changed', function () {
      var place = autocomplete.getPlace();
      if (!place.geometry) {
        // User entered the name of a Place that was not suggested and
        // pressed the Enter key, or the Place Details request failed.
        window.alert("No details available for input: '" + place.name + "'");
        return;
      }

      // If the place has a geometry, then present it on a map.
      if (place.geometry.viewport) {
        Mapa.fitBounds(place.geometry.viewport);
      } else {
        Mapa.setCenter(place.geometry.location);
        Mapa.setZoom(17);  // Why 17? Because it looks good.
      }
      marker.setIcon(/** @type {google.maps.Icon} */({
        url: place.icon,
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(35, 35)
      }));
      marker.setPosition(place.geometry.location);
      marker.setVisible(true);

      var address = '';
      if (place.address_components) {
        address = [
          (place.address_components[0] && place.address_components[0].short_name || ''),
          (place.address_components[1] && place.address_components[1].short_name || ''),
          (place.address_components[2] && place.address_components[2].short_name || '')
        ].join(' ');
      }
      infowindow.setContent('<div><strong>' + "El local que proporciono se ubica en: " + '</strong><br>' + address);
      infowindow.open(Mapa, marker);
    });
}

function addMarker(address, local, price, name, date) {
  var contentString = '<div>' + '<h1>' + name + '</h1>' + '<h3>' + '$' + price + '</h3>' + '<h6>' + address + '</h6>' + '<h6>' + local + '</h6>' + '</div>';
  var geocoder = new google.maps.Geocoder();
  geocoder.geocode({ 'address': address }, function (results, status) {
    if (status === 'OK') {
      latitudRegisterProd = results[0].geometry.location.lat();
      longitudRegisterProd = results[0].geometry.location.lng();
      name = new google.maps.Marker({
        map: Mapa,
        position: results[0].geometry.location
      });
      var infowindow = new google.maps.InfoWindow({
        content: contentString
      });
      name.addListener('click', function () {
        infowindow.open(Mapa, name);
      });
    }
  });
}
