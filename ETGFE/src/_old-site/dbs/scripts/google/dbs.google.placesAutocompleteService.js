'use strict';

window.dbs = window.dbs || {};
dbs.google = dbs.google || {};

/**
 *
 * @param {string[]} types - <a href="https://developers.google.com/places/web-service/autocomplete#place_types">Place types</a>
 * @param {string[]} countries - An array of country codes to restrict search
 */

dbs.google.placesAutocompleteService = function (types, countries) {
  var self = this;
  self.events = new dbs.events();
  self.autoCompleteService = new google.maps.places.AutocompleteService();
  self.countries = countries ? countries : null;
  self.types = types ? types : null;
  self.getPlacePredictions = function (query, callback) {
    self.autoCompleteService.getPlacePredictions({
      input: query,
      types: self.types,
      componentRestrictions: self.countries
    }, function(predictions, status) {
      callback(predictions,status);
    });
  };
};
