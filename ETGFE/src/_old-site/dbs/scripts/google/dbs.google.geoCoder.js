'use strict';

window.dbs = window.dbs || {};
dbs.google = dbs.google || {};

dbs.google.geoCoder = function () {
  var self = this;
  self.events = new dbs.events();
  self.geoCoder = new google.maps.Geocoder();
  self.googleUtilities = new dbs.google.utilities();

  self.geocode = function (lat, lng) {
    self.events.emit('GeoCoder:geocode', null, true);
    self.geoCoder.geocode({
      'location': {lat: lat, lng: lng}
    }, function(results, status) {
      self.events.emit('GeoCoder:geocode_place', {
        formattedAddress: results[0].formatted_address,
        address: self.googleUtilities.addressFromComponents(results[0].address_components),
        lat: results[0].geometry.location.lat(),
        lng: results[0].geometry.location.lng(),
        placeId: results[0].place_id
      }, true);
    });
  }
};
