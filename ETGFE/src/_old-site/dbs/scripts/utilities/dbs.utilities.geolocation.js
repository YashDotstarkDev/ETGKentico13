'use strict';

window.dbs = window.dbs || {};
dbs.utilities = dbs.utilities || {};

dbs.utilities.geolocation = function () {
  var self = this;
  self.events = new dbs.events();
  self.position = null;

  if(navigator.geolocation) {
    self.events.emit('Geolocation:supported', null, true);
  } else {
    self.events.emit('Geolocation:notSupported', null, true);
  }

  self.handlePosition = function (position) {
    self.position = position;
    self.events.emit('Geolocation:getPosition_response', self.position, true);
  };

  self.handleError = function (error) {
    self.events.emit('Geolocation:getPosition_error', error, true);
  };

  self.getPosition = function () {
    if(navigator.geolocation) {
      self.events.emit('Geolocation:getPosition', null, true);
      navigator.geolocation.getCurrentPosition(self.handlePosition, self.handleError);
    }
  };
};
