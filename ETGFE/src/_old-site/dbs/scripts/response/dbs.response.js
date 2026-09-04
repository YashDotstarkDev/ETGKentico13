'use strict';

window.dbs = window.dbs || {};

dbs.response = function (response, el) {
  var self = this;
  self.el = el;

  self.handle = function () {

    if(response.objectSet) {
      if(response.objectSet.actions) {
        for (var a = 0; a < response.objectSet.actions.length; a++) {
          var action = response.objectSet.actions[a];
          switch (action.type) {
            case 'message':
              if(self.el) {
                if(!self.el.find('.response-container').length) {
                  console.warn('message action requires a .response-container');
                }
                self.el.find('.response-container').append(action.content);

                if (self.el.find('.form-container').length){
                  self.el.find('.form-container').hide();
                }
                if (self.el.find('.popup-close-button-container').length){
                  self.el.find('.popup-close-button-container').show();
                }
              } else {
                console.warn('message action requires a parent element');
              }
              break;
            case 'setCookie':
              if(!action.name) {
                console.warn('setCookie action must specify name');
                break;
              }
              if(!action.value) {
                console.warn('setCookie action must specify value');
                break;
              }
              var cookieParams = {path: '/'};
              if(action.path) {
                cookieParams.path = action.path
              }
              if(action.expires) {
                cookieParams.expires = action.expires
              }
              Cookies.set(name, value, cookieParams);
              break;
            case 'redirect':
              if(action.uri) {
                window.location.href = action.uri;
              } else {
                console.warn('redirect action requires a uri');
              }
              break;
            case 'modal':
              $('body').append('<div class="ui mini modal"><i class="close icon icon-close"></i><div class="content">' + action.content + '</div></div>');
              var modal = $('body').find('.modal:last');
              $(modal).modal('show');
              break;
            case 'fbqTrack':
              if(!action.eventName) {
                console.warn('fbqTrack action must specify eventName');
                break;
              }
              if(!action.customData) {
                console.warn('fbqTrack action must specify customData');
                break;
              }
              try {
                fbq('track', action.eventName, action.customData);
              }
              catch (err) {
                console.warn(err)
              }
              break;
            case 'gtmTrack':
              if(!action.dataLayer) {
                console.warn('gtmTrack action must specify dataLayer');
                break;
              }
              try {
                dataLayer.push(action.dataLayer);
              }
              catch (err) {
                console.warn(err)
              }
              break;
            case 'gaTrack':
              if(!action.fields) {
                console.warn('gaTrack action must specify fields');
                break;
              }
              try {
                ga('send', action.fields);
              }
              catch (err) {
                console.warn(err)
              }
              break;
          }
        }
      }
    }


  }
};
