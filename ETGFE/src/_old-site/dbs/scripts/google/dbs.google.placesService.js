'use strict';

window.dbs = window.dbs || {};
dbs.google = dbs.google || {};

/**
 *
 * @param {string[]} types - <a href="https://developers.google.com/places/web-service/autocomplete#place_types">Place types</a>
 * @param {string[]} countries - An array of country codes to restrict search
 */

dbs.google.placesService = function () {
  var self = this;
  self.events = new dbs.events();
  self.tempDiv = null;
  self.place = null;
  self.placesService = new google.maps.places.PlacesService(document.createElement('div'));
  self.getDetails = function (placeId, callback) {
    self.events.emit('PlacesService:getDetails', null, true);
    self.placesService.getDetails({placeId: placeId}, function(place) {
      self.tempDiv = $('<div></div>');
      self.tempDiv.append(place.adr_address);
      self.place = {
        formattedAddress: place.formatted_address,
        address: {
          street: $(self.tempDiv).find('.street-address').text(),
          suburb: $(self.tempDiv).find('.locality').text(),
          state: $(self.tempDiv).find('.region').text(),
          postcode: $(self.tempDiv).find('.postal-code').text(),
          country: $(self.tempDiv).find('.country-name').text()
        },
        lat: place.geometry.location.lat(),
        lng: place.geometry.location.lng(),
        placeId: place.place_id
      };
      self.events.emit('PlacesService:getDetails_response', self.place, true);

      if(callback) {
        callback(self.place);
      }
    });
  };
};
