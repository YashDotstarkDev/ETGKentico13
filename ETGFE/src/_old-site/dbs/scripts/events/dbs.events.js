'use strict';

window.dbs = window.dbs || {};

dbs.events = function () {
  var self = this;
  self.events = {};

  self.subscribe = function (eventName, callback) {
    self.events[eventName] = self.events[eventName] || [];
    self.events[eventName].push(callback);
  };

  self.emit = function (eventName, data, log) {
    if(window.dbs.loggingEnabled && log) {
      if(data) {
        console.log('Event: ' + eventName, data);
      } else {
        console.log('Event: ' + eventName);
      }
    }

    var evt = self.events[eventName];
    if(evt) {
      for (var i = 0; i < evt.length; i++) {
        evt[i].call(null, data);
      }
    }
  };
};

window.dbs.globalEvents = new dbs.events();
