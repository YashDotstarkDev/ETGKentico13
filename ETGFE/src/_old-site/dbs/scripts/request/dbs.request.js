'use strict';

window.dbs = window.dbs || {};

dbs.request = function (endpoint, method) {
  var self = this;
  self.events = new dbs.events();

  self.send = function (data, ajaxSettings) {
    self.events.emit('Request:send', {
      endpoint: endpoint,
      method: method,
      data: data
    }, true);

    if (!ajaxSettings) {
      ajaxSettings = {
        url: endpoint,
        type: method,
        data: JSON.stringify(data),
        dataType: 'json',
        contentType: 'application/json'
      }
    } else {
      ajaxSettings.data = data;
    }

    $.ajax(ajaxSettings)
      .done(function(response) {
        self.events.emit('Request:send_success', {
          endpoint: endpoint,
          method: method,
          data: data,
          response: response
        }, true);
      })
      .fail(function(response) {
        self.events.emit('Request:send_fail', {
          endpoint: endpoint,
          method: method,
          data: data,
          response: response.responseJSON
        }, true);
      })
      .always(function(){
        self.events.emit('Request:send_finished', {
          endpoint: endpoint,
          method: method,
          data: data
        }, true);
      });
  };
};
