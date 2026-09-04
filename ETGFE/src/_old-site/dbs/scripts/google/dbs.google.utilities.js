'use strict';

window.dbs = window.dbs || {};
dbs.google = dbs.google || {};

dbs.google.utilities = function () {
  var self = this;
  self.events = new dbs.events();
  self.addressFromComponents = function (components) {

    var streetArray = [];
    var suburb = '';
    var state = '';
    var postcode = '';
    var country = '';

    $.each(components, function(index, component) {
      switch (component.types[0]) {
        case 'subpremise':
          streetArray.push(component.long_name + '/');
          break;
        case 'street_number':
          streetArray.push(component.long_name + ' ');
          break;
        case 'route':
          streetArray.push(component.short_name);
          break;
        case 'locality':
          suburb = component.long_name;
          break;
        case 'administrative_area_level_1':
          state = component.short_name;
          break;
        case 'postal_code':
          postcode = component.long_name;
          break;
        case 'country':
          country = component.long_name;
          break;
      }
    });

    return {
      street: streetArray.join(''),
      suburb: suburb,
      state: state,
      postcode: postcode,
      country: country
    };
  };
};
