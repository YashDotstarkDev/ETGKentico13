'use strict';

window.dbs = window.dbs || {};
dbs.google = dbs.google || {};

/**
 *
 * @param {jqueryDomObject} el - Use a semantic ui.search here
 * @param {string[]} types - <a href="https://developers.google.com/places/web-service/autocomplete#place_types">Place types</a>
 * @param {string[]} countries - An array of country codes to restrict search
 */

dbs.google.placesDropdown = function (el, types, countries) {
  var self = this;
  self.el = el;
  self.events = new dbs.events();
  self.placeSelected = null;

  self.countries = countries ? {country: countries} : null;
  self.types = types ? types : null;

  self.autoCompleteService = new dbs.google.placesAutocompleteService(self.types, self.countries);
  self.placesService = new dbs.google.placesService();

  self.placesService.events.subscribe('PlacesService:getDetails', function () {
    self.el.addClass('loading disabled');
  });

  self.placesService.events.subscribe('PlacesService:getDetails_response', function (place) {
    self.el.removeClass('loading disabled');
    self.placeSelected = place;
    self.events.emit('PlacesDropdown:placeSelected', self.placeSelected, true);
  });

  self.dropdown = self.el.dropdown({
    minCharacters: 3,
    selectFirstResult: true,
    forceSelection: true,
    apiSettings: {
      responseAsync: function(settings, callback) {
        self.autoCompleteService.getPlacePredictions(settings.urlData.query, function(predictions, status) {
          callback({
            predictions: predictions,
            status: status
          });
        });
      },
      onResponse: function (results) {
        console.log(results)
        var response = {
          results: []
        };
        $.each(results.predictions, function(index, item){
          response.results.push({
            value: item.place_id,
            name: item.structured_formatting.main_text + '<br>' + item.structured_formatting.secondary_text,
            text: item.structured_formatting.main_text + ', ' + item.structured_formatting.secondary_text
          });
        });
        return response;
      },
      onFailure: function () {
        return {
          results: []
        }
      },
      onError: function () {
        return {
          results: []
        }
      }
    },
    onChange: function(placeId) {
      self.placesService.getDetails(placeId);
      return false;
    }
  });

};
