'use strict';

window.dbs = window.dbs || {};
dbs.form = dbs.form || {};

$.fn.form.settings.rules.emailDependOnRadio = function (value, dependent) {
  let [dependentsName, dependentsValue] = dependent.split('|')
  let dependentRadio = $(this).closest('.ui.form').find(`input[name="${dependentsName}"][value="${dependentsValue}"]`)

  if (dependentRadio.is(':checked')) {
    // is most likely an email
    return $.fn.form.settings.regExp.email.test(value)
  }

  // If dependent radio is not checked, this field is not required, so return true (to pass validation)
  return true
}
$.fn.form.settings.prompt.emailDependOnRadio = $.fn.form.settings.prompt.email

$.fn.form.settings.rules.phoneDependOnRadio = function (value, dependent) {
  let [dependentsName, dependentsValue] = dependent.split('|')
  let dependentRadio = $(this).closest('.ui.form').find(`input[name="${dependentsName}"][value="${dependentsValue}"]`)

  if (dependentRadio.is(':checked')) {
    // NOTE this is only checking for a valid number, not a valid phone number
    return value.trim() !== '' && $.fn.form.settings.regExp.number.test(value);
  }

  // If dependent radio is not checked, this field is not required, so return true (to pass validation)
  return true
}
$.fn.form.settings.prompt.phoneDependOnRadio = $.fn.form.settings.prompt.number


$.fn.form.settings.rules.mobileVerified = function() {
  return $('#mobileVerified').val() === 'verified'
};

$.fn.form.settings.rules.checkboxCount = function(value, checkboxCount) {
  return $(this).parent().find('input:checked').length >= parseInt(checkboxCount);
};

$.fn.form.settings.rules.checkboxCountVisible = function(value, checkboxCount) {

  if ($(this).parent().find('.field').is(':visible')){
    return $(this).parent().find('input:checked').length >= parseInt(checkboxCount);
  }

  return true;
};


$.fn.form.settings.rules.checkHiddenInputVisible = function(value) {
  if(!$(this).parents('.field').is(':visible')) {
    return true;
  }
  return (value);
};

$.fn.form.settings.rules.isVisible = function(value, text) {
  if(!$(this).is(':visible')) {
    return true;
  }
  text = (typeof text == 'string')
    ? text.toLowerCase()
    : text
  ;
  value = (typeof value == 'string')
    ? value.toLowerCase()
    : value
  ;
  return (value == text);
};

$.fn.form.settings.rules.username = function(value) {
  var reg = new RegExp('^[a-zA-Z0-9_\\-]{3,20}$');
  return reg.test(value);
};

$.fn.form.settings.rules.pwd = function(value) {
  var reg = new RegExp($('#pwdValidation').val());
  return reg.test(value);
};

$.fn.form.settings.rules.emailRegex = function(value) {
  if(!value.length) {
    return true;
  }
  var reg = new RegExp('^((([a-z]|\\d|[!#\\$%&\'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+(\\.([a-z]|\\d|[!#\\$%&\'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(\\\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\\d|[\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([a-z]|\\d|[\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-||_|~|[\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|\\d|[\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.)+(([a-z]|[\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+|(([a-z]|[\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+([a-z]+|\\d|-|\\.{0,1}|_|~|[\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])?([a-z]|[\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))$');
  return reg.test(value.toLowerCase());
};

$.fn.form.settings.rules.greaterTime = function(value) {

  if(!$(this).parent().is(':visible')) {
    return true;
  }


  var thisIndex = $(this).get(0).selectedIndex;
  var otherIndex = $(this).parents('.inner').find('.time-from select').get(0).selectedIndex;

  return thisIndex > otherIndex;



  // console.log('GREATERTIME', $(this));
  // console.log('GREATERTIME value', value);
  //
  // if(!$(this).parent().is(':visible')) {
  //   return true;
  // }
  //
  //
  // var thisIndex = $(this).get(0).selectedIndex;
  // var otherIndex = $(this).parents('.inner').find('select').get(0).selectedIndex;
  //
  //
  //
  // console.log('GREATERTIME', $(this).parents('.inner').length);
  //
  // console.log(thisIndex);
  // console.log(otherIndex);
  //
  // if (thisIndex && otherIndex === 0){
  //   return thisIndex > otherIndex;
  // }
};


// is not empty or blank string
$.fn.form.settings.rules.emptyVisible = function(value) {
  if(!$(this).is(':visible')) {
    return true;
  }
  return !(value === undefined || '' === value || $.isArray(value) && value.length === 0);
};

// checkbox checked
$.fn.form.settings.rules.checkedVisible = function() {
  if(!$(this).is(':visible')) {
    return true;
  }
  return ($(this).filter(':checked').length > 0);
};

// is most likely an email
$.fn.form.settings.rules.emailVisible = function(value){
  if(!$(this).is(':visible')) {
    return true;
  }
  return $.fn.form.settings.regExp.email.test(value);
};

// value is most likely url
$.fn.form.settings.rules.urlVisible = function(value) {
  if(!$(this).is(':visible')) {
    return true;
  }
  return $.fn.form.settings.regExp.url.test(value);
};

$.fn.form.settings.rules.validatesetselect = function() {
  var valid = false
  $(this).parents('.validation-set').find('select').each(function(i, el){
    if ($(el).val() && $(el).val() != " "){
      valid = true
    }
  });
  return valid;
};

$.fn.form.settings.rules.validateTraveller = function() {
  var valid = false
  var doubleRoomCount = parseInt($(this).parents('.validation-set').find('.double-room').dropdown('get value'));
  var doubleRoomPersons = doubleRoomCount * 2;
  var singleRoomPersons = parseInt($(this).parents('.validation-set').find('.single-room').dropdown('get value'));

  var totalRooms = doubleRoomPersons + singleRoomPersons

  if (totalRooms > 9)
  {
    $(this).parents('.validation-set').addClass('max-guests');
    return false;
  }

  $(this).parents('.validation-set').removeClass('max-guests');
  $(this).parents('.validation-set').removeClass('incomplete-rooms-type');
  var hasSelectedRoomCount = false;
  $(this).parents('.validation-set').find('.select-room-count select').each(function(i, el){

      if ($(el).val() && $(el).val() != " "){

        hasSelectedRoomCount = true;
      }
  });

  var isValid = true;
  if (!hasSelectedRoomCount){
    return false;
  }else if (doubleRoomCount == 0){
    return true;
  }else
  {
     $(this).parents('.validation-set').find('.room-type').each(function(i, el){

        if ($(el).dropdown('get value') ==" "){
          $(this).parents('.validation-set').addClass('incomplete-rooms-type');
          isValid = false;
        }
    });
  }
  console.log('isValid', isValid)
  return isValid;
};

// matches specified regExp
$.fn.form.settings.rules.regExpVisible = function(value, regExp) {
  if(!$(this).is(':visible')) {
    return true;
  }
  if(regExp instanceof RegExp) {
    return value.match(regExp);
  }
  var
    regExpParts = regExp.match($.fn.form.settings.regExp.flags),
    flags
  ;
  // regular expression specified as /baz/gi (flags)
  if(regExpParts) {
    regExp = (regExpParts.length >= 2)
      ? regExpParts[1]
      : regExp
    ;
    flags = (regExpParts.length >= 3)
      ? regExpParts[2]
      : ''
    ;
  }
  return value.match( new RegExp(regExp, flags) );
};

// is valid integer or matches range
$.fn.form.settings.rules.integerVisible = function(value, range) {
  if(!$(this).is(':visible')) {
    return true;
  }
  var
    intRegExp = $.fn.form.settings.regExp.integer,
    min,
    max,
    parts
  ;
  if( !range || ['', '..'].indexOf(range) !== -1) {
    // do nothing
  }
  else if(range.indexOf('..') == -1) {
    if(intRegExp.test(range)) {
      min = max = range - 0;
    }
  }
  else {
    parts = range.split('..', 2);
    console.log(parts)
    if(intRegExp.test(parts[0])) {
      min = parts[0] - 0;
    }
    if(intRegExp.test(parts[1])) {
      max = parts[1] - 0;
    }

    console.log(min, max)
  }
  return (
    intRegExp.test(value) &&
    (min === undefined || value >= min) &&
    (max === undefined || value <= max)
  );
};

// is valid number (with decimal)
$.fn.form.settings.rules.decimalVisible = function(value) {
  if(!$(this).is(':visible')) {
    return true;
  }
  return $.fn.form.settings.regExp.decimal.test(value);
};

// is valid number
$.fn.form.settings.rules.numberVisible = function(value) {
  if(!$(this).is(':visible')) {
    return true;
  }
  return value != '' && $.fn.form.settings.regExp.number.test(value);
};

// is valid phone number
$.fn.form.settings.rules.phone = function(value) {
  if(!$(this).is(':visible')) {
    return true;
  }

  value = value.replaceAll(' ','');
  return $.fn.form.settings.regExp.phoneNumber.test(value);
};

$.fn.form.settings.rules.phoneOptional = function(value) {

  if(!$(this).is(':visible') || value.trim().length === 0) {
    return true;
  }
  value = value.replaceAll(' ','');
  return $.fn.form.settings.regExp.phoneNumber.test(value);
};


// is value (case insensitive)
$.fn.form.settings.rules.isVisible = function(value, text) {
  if(!$(this).is(':visible')) {
    return true;
  }
  text = (typeof text == 'string')
    ? text.toLowerCase()
    : text
  ;
  value = (typeof value == 'string')
    ? value.toLowerCase()
    : value
  ;
  return (value == text);
};

// is value
$.fn.form.settings.rules.isExactlyVisible = function(value, text) {
  if(!$(this).is(':visible')) {
    return true;
  }
  return (value == text);
};

// is not less than
$.fn.form.settings.rules.notLessThanVisible = function(value, compareValue) {
  if(!$(this).is(':visible')) {
    return true;
  }

  if (isNaN(value) || isNaN(compareValue)){
    return false;
  }

  return (Number(value) >= Number(compareValue));
};


// value is not another value (case insensitive)
$.fn.form.settings.rules.notVisible = function(value, notValue) {
  if(!$(this).is(':visible')) {
    return true;
  }
  value = (typeof value == 'string')
    ? value.toLowerCase()
    : value
  ;
  notValue = (typeof notValue == 'string')
    ? notValue.toLowerCase()
    : notValue
  ;
  return (value != notValue);
};

// value is not another value (case sensitive)
$.fn.form.settings.rules.notExactlyVisible = function(value, notValue) {
  if(!$(this).is(':visible')) {
    return true;
  }
  return (value != notValue);
};

// value contains text (insensitive)
$.fn.form.settings.rules.containsVisible = function(value, text) {
  if(!$(this).is(':visible')) {
    return true;
  }
  // escape regex characters
  text = text.replace($.fn.form.settings.regExp.escape, "\\$&");
  return (value.search( new RegExp(text, 'i') ) !== -1);
};

// value contains text (case sensitive)
$.fn.form.settings.rules.containsExactlyVisible = function(value, text) {
  if(!$(this).is(':visible')) {
    return true;
  }
  // escape regex characters
  text = text.replace($.fn.form.settings.regExp.escape, "\\$&");
  return (value.search( new RegExp(text) ) !== -1);
};

// value contains text (insensitive)
$.fn.form.settings.rules.doesntContainVisible = function(value, text) {
  if(!$(this).is(':visible')) {
    return true;
  }
  // escape regex characters
  text = text.replace($.fn.form.settings.regExp.escape, "\\$&");
  return (value.search( new RegExp(text, 'i') ) === -1);
};

// value contains text (case sensitive)
$.fn.form.settings.rules.doesntContainExactlyVisible = function(value, text) {
  if(!$(this).is(':visible')) {
    return true;
  }
  // escape regex characters
  text = text.replace($.fn.form.settings.regExp.escape, "\\$&");
  return (value.search( new RegExp(text) ) === -1);
};

// is at least string length
$.fn.form.settings.rules.minLengthVisible = function(value, requiredLength) {
  if(!$(this).is(':visible')) {
    return true;
  }
  return (value !== undefined)
    ? (value.length >= requiredLength)
    : false
    ;
};

// see rls notes for 2.0.6 (this is a duplicate of minLength)
$.fn.form.settings.rules.lengthVisible = function(value, requiredLength) {
  if(!$(this).is(':visible')) {
    return true;
  }
  return (value !== undefined)
    ? (value.length >= requiredLength)
    : false
    ;
};

$.fn.form.settings.rules.moreThanZero = function(value) {

  if(!$(this).parent().is(':visible')) {
    return true;
  }

  return value > 0;
};

// is exactly length
$.fn.form.settings.rules.exactLengthVisible = function(value, requiredLength) {
  if(!$(this).is(':visible')) {
    return true;
  }
  return (value !== undefined)
    ? (value.length == requiredLength)
    : false
    ;
};

// is less than length
$.fn.form.settings.rules.maxLengthVisible = function(value, maxLength) {
  if(!$(this).is(':visible')) {
    return true;
  }
  return (value !== undefined)
    ? (value.length <= maxLength)
    : false
    ;
};

// matches another field
$.fn.form.settings.rules.matchVisible = function(value, identifier) {
  if(!$(this).is(':visible')) {
    return true;
  }
  var
    $form = $(this),
    matchingValue
  ;
  if( $('[data-validate="'+ identifier +'"]').length > 0 ) {
    matchingValue = $('[data-validate="'+ identifier +'"]').val();
  }
  else if($('#' + identifier).length > 0) {
    matchingValue = $('#' + identifier).val();
  }
  else if($('[name="' + identifier +'"]').length > 0) {
    matchingValue = $('[name="' + identifier + '"]').val();
  }
  else if( $('[name="' + identifier +'[]"]').length > 0 ) {
    matchingValue = $('[name="' + identifier +'[]"]');
  }
  return (matchingValue !== undefined)
    ? ( value.toString() == matchingValue.toString() )
    : false
    ;
};

// different than another field
$.fn.form.settings.rules.differentVisible = function(value, identifier) {
  if(!$(this).is(':visible')) {
    return true;
  }
  // use either id or name of field
  var
    $form = $(this),
    matchingValue
  ;
  if( $('[data-validate="'+ identifier +'"]').length > 0 ) {
    matchingValue = $('[data-validate="'+ identifier +'"]').val();
  }
  else if($('#' + identifier).length > 0) {
    matchingValue = $('#' + identifier).val();
  }
  else if($('[name="' + identifier +'"]').length > 0) {
    matchingValue = $('[name="' + identifier + '"]').val();
  }
  else if( $('[name="' + identifier +'[]"]').length > 0 ) {
    matchingValue = $('[name="' + identifier +'[]"]');
  }
  return (matchingValue !== undefined)
    ? ( value.toString() !== matchingValue.toString() )
    : false
    ;
};

$.fn.form.settings.rules.creditCardVisible = function(cardNumber, cardTypes) {
  if(!$(this).is(':visible')) {
    return true;
  }
  var
    cards = {
      visa: {
        pattern : /^4/,
        length  : [16]
      },
      amex: {
        pattern : /^3[47]/,
        length  : [15]
      },
      mastercard: {
        pattern : /^5[1-5]/,
        length  : [16]
      },
      discover: {
        pattern : /^(6011|622(12[6-9]|1[3-9][0-9]|[2-8][0-9]{2}|9[0-1][0-9]|92[0-5]|64[4-9])|65)/,
        length  : [16]
      },
      unionPay: {
        pattern : /^(62|88)/,
        length  : [16, 17, 18, 19]
      },
      jcb: {
        pattern : /^35(2[89]|[3-8][0-9])/,
        length  : [16]
      },
      maestro: {
        pattern : /^(5018|5020|5038|6304|6759|676[1-3])/,
        length  : [12, 13, 14, 15, 16, 17, 18, 19]
      },
      dinersClub: {
        pattern : /^(30[0-5]|^36)/,
        length  : [14]
      },
      laser: {
        pattern : /^(6304|670[69]|6771)/,
        length  : [16, 17, 18, 19]
      },
      visaElectron: {
        pattern : /^(4026|417500|4508|4844|491(3|7))/,
        length  : [16]
      }
    },
    valid         = {},
    validCard     = false,
    requiredTypes = (typeof cardTypes == 'string')
      ? cardTypes.split(',')
      : false,
    unionPay,
    validation
  ;

  if(typeof cardNumber !== 'string' || cardNumber.length === 0) {
    return;
  }

  // allow dashes in card
  cardNumber = cardNumber.replace(/[\-]/g, '');

  // verify card types
  if(requiredTypes) {
    $.each(requiredTypes, function(index, type){
      // verify each card type
      validation = cards[type];
      if(validation) {
        valid = {
          length  : ($.inArray(cardNumber.length, validation.length) !== -1),
          pattern : (cardNumber.search(validation.pattern) !== -1)
        };
        if(valid.length && valid.pattern) {
          validCard = true;
        }
      }
    });

    if(!validCard) {
      return false;
    }
  }

  // skip luhn for UnionPay
  unionPay = {
    number  : ($.inArray(cardNumber.length, cards.unionPay.length) !== -1),
    pattern : (cardNumber.search(cards.unionPay.pattern) !== -1)
  };
  if(unionPay.number && unionPay.pattern) {
    return true;
  }

  // verify luhn, adapted from  <https://gist.github.com/2134376>
  var
    length        = cardNumber.length,
    multiple      = 0,
    producedValue = [
      [0, 1, 2, 3, 4, 5, 6, 7, 8, 9],
      [0, 2, 4, 6, 8, 1, 3, 5, 7, 9]
    ],
    sum           = 0
  ;
  while (length--) {
    sum += producedValue[multiple][parseInt(cardNumber.charAt(length), 10)];
    multiple ^= 1;
  }
  return (sum % 10 === 0 && sum > 0);
};

$.fn.form.settings.rules.minCountVisible = function(value, minCount) {
  if(!$(this).is(':visible')) {
    return true;
  }
  if(minCount == 0) {
    return true;
  }
  if(minCount == 1) {
    return (value !== '');
  }
  return (value.split(',').length >= minCount);
};

$.fn.form.settings.rules.exactCountVisible = function(value, exactCount) {
  if(!$(this).is(':visible')) {
    return true;
  }
  if(exactCount == 0) {
    return (value === '');
  }
  if(exactCount == 1) {
    return (value !== '' && value.search(',') === -1);
  }
  return (value.split(',').length == exactCount);
};

$.fn.form.settings.rules.maxCountVisible = function(value, maxCount) {
  if(!$(this).is(':visible')) {
    return true;
  }
  if(maxCount == 0) {
    return false;
  }
  if(maxCount == 1) {
    return (value.search(',') === -1);
  }
  return (value.split(',').length <= maxCount);
};


$.fn.form.settings.rules.validateIfVisible = function() {
  switch ($(this).get(0).nodeName){
    case 'SELECT':
      if(!$(this).parent().is(':visible')) {
        return true;
      }
      break;

    case 'INPUT':

      switch ($(this).attr('type').toLowerCase()){
        case 'checkbox':
          if(!$(this).parent().is(':visible')) {
            return true;
          } else {
            return
          }
          break;
        case 'radio':
          if(!$(this).parent().is(':visible')) {
            return true;
          }
          break;

        default:
          if(!$(this).is(':visible')) {
            return true;
          }
          break;
      }

      break;

    default:
      if(!$(this).is(':visible')) {
        return true;
      }
      break;
  }

  return $(this).val().length > 0;
};


dbs.form.genericForm = function() {
  var self = this;

  self.init = function(el) {
    self.el = $(el);
    self.submitButton = self.el.find('.submit');
    self.fields = {};
    self.validateInline = false;
    self.validateOn = 'submit';
    self.endpoint = self.el.attr('data-endpoint');
    self.method = 'POST';
    self.events = new dbs.events();
    self.clearOnSubmit = false;

    self.parseSettings();
    self.parseValidation();
    self.bindInputs();
    self.bindSubmit();

    self.form = self.el.form({
      fields: self.fields,
      on: self.validateOn,
      inline: self.validateInline
    });

    if(window.dbs.loggingEnabled) {
      console.table(self.fields);
    }
  };

  self.bindInputs = function () {
    self.el.find('input').not('.submit').on('keypress', function(e){
      if(e.which === 13) {
        e.preventDefault();
        self.el.find('.submit:first').trigger('click');
      }
    });
  };

  self.sendData = function (data) {
    // create a request
    var sendRequest = new dbs.request(self.endpoint, self.method);

    // subscribe to the send event
    sendRequest.events.subscribe('Request:send', function (data) {
      console.log('REQUEST SEND');
      self.el.find('.submit').addClass('loading').blur();

      if ($('.confirm-agent-price .submit').length){
        $('.confirm-agent-price .submit').addClass('loading').blur();
      }
      self.events.emit('Form:send', {
        data: data.data,
        endpoint: data.endpoint,
        method: data.method
      }, true);
    });

    // subscribe to the send_finished event
    sendRequest.events.subscribe('Request:send_finished', function (data) {
      self.el.find('.submit').removeClass('loading').blur();

      self.events.emit('Form:send_finished', {
        data: data.data,
        endpoint: data.endpoint,
        method: data.method
      }, true);
    });


    /* Handle Responses */

    // subscribe to the send_success event
    sendRequest.events.subscribe('Request:send_success', function (data) {

      // always clear the form
      if(self.clearOnSubmit) {
        self.form.form('reset');
      }
      // create a new response
      var response = new dbs.response(data.response, self.el);
      response.handle();

      self.events.emit('Form:send_success', {
        data: data.data,
        endpoint: data.endpoint,
        method: data.method,
        response: data.response
      }, true);
    });

    // subscribe to the send_error event
    sendRequest.events.subscribe('Request:send_error', function (data) {
      // create a new response
      var response = new dbs.response(data.response, self.el);
      response.handle();

      self.events.emit('Form:send_error', {
        data: data.data,
        endpoint: data.endpoint,
        method: data.method,
        response: data.response
      }, true);
    });

    // subscribe to the send_fail event
    sendRequest.events.subscribe('Request:send_fail', function (data) {
      var response = new dbs.response(data.response, self.el);
      response.handle();

      self.events.emit('Form:send_fail', {
        data: data
      }, true);
    });

    // send the request
    sendRequest.send(data);
  }

  self.send = async function () {

    console.log('send function')

    if(window.dbs.loggingEnabled) {
      console.log('Form: send');
    }
    // get the data
    var data = self.form.form('get values');

    console.log('got form values', data)

    // add recaptcha v3
    if (self.el.find('.recaptcha').length > 0){
      grecaptcha.ready(function() { // Wait for the recaptcha to be ready
        grecaptcha
          .execute(self.el.find('.recaptcha').attr('data-site-key'), {
            action: self.el.find('.recaptcha').attr('data-action')
          }) // Execute the recaptcha
          .then(function(token){

            data.g_recaptcha_response = token;

            console.log('recaptcha token', token)

            self.sendData(data)
          })
      })
    } else {
      self.sendData(data)
    }
  };

  self.unbindSubmit = function(){
    self.submitButton.unbind("click");
  }
  self.bindSubmit = function () {
    self.submitButton.on('click', async function(e){
      if(!$(this).hasClass('loading')) {



        if(self.form.form('is valid')) {
          if ($(this).hasClass('needs-confirm')){

            if (self.el.find('.your-price-container').length &&
            self.el.find('input[name=hidGrossPrice]').length && self.el.find('input[name=agentprice]').length
            && self.el.find('input[name=agentprice]').is(':visible')){

                if (Number(self.el.find('input[name=agentprice]').val()) != Number(self.el.find('input[name=hidGrossPrice]').val())){

                  e.preventDefault();
                  $('.' + $(this).attr('data-modal')).modal('show');
                  return;
                }

            }

          }

          if(window.dbs.loggingEnabled) {
            console.log('SUBMIT CLICKED - FORM IS VALID');
          }
          self.events.emit('Form:validation_success', {
            values: self.form.form('get values'),
            el: self.el
          }, true);
          // hide responses
          self.el.find('.response-container').html('');

          // if theres an endpoint, prevent default and send
          if(self.endpoint != null && self.endpoint != undefined && self.endpoint != '') {
            if(window.dbs.loggingEnabled) {
              console.log('ENDPOINT FOUND - SENDING DATA');
            }
            e.preventDefault();
            self.send();
          } else {
            self.el.find('.submit').addClass('loading').blur();
          }
        } else {
          if(window.dbs.loggingEnabled) {
            console.log('SUBMIT CLICKED - FORM IS NOT VALID');
          }
          e.preventDefault();
          self.events.emit('Form:validation_fail', {
            values: self.form.form('get values'),
            el: self.el
          }, true);
          setTimeout(function () {
            if(!$('.modal.visible').length){
              if (self.el.find('.field.error:first').length){

                $('html, body').animate({scrollTop: self.el.find('.field.error:first').offset().top - 120})
              }
            }
          }, 100)
        }
      } else {
        if(window.dbs.loggingEnabled) {
          console.log('SUBMIT CLICKED - DISABLED FROM LOADING STATE');
        }
        e.preventDefault();
      }
    });
  };

  self.parseSettings = function () {
    // data-validate-inline
    if (self.el.attr('data-validate-inline') !== undefined) {
      self.validateInline = true;
    }
    // data-validate-on
    if (self.el.attr('data-validate-on') !== undefined) {
      self.validateOn = self.el.attr('data-validate-on');
    }
    // method
    if (self.el.attr('data-method') !== undefined) {
      self.method = self.el.attr('data-method');
    }
    // clearOnSubmit
    if (self.el.attr('data-clear') !== undefined) {
      self.clearOnSubmit = true;
    }
  };

  self.parseValidation = function () {
    self.el.find('[data-rules]').each(function(vi, vel){
      var identifier = $(vel).attr('name');
      if($(vel).attr('data-validate')) {
        identifier = $(vel).attr('data-validate');
      }
      self.fields[identifier] = {
        identifier: identifier,
        rules: []
      };
      var rules = $(vel).attr('data-rules').split(',');
      var prompts = [];
      if($(vel).attr('data-prompts')) {
        prompts = $(vel).attr('data-prompts').split(',');
      }

      for (var r = 0; r < rules.length; r++) {
        self.fields[identifier].rules.push({
          type: rules[r]
        });
        if(prompts.length && prompts[r]) {
          self.fields[identifier].rules[r].prompt = prompts[r].split('[comma]').join(',');
        }
      }
    });
  };

  self.handleError = function (message) {
    self.el.find('.response-container').append('<div class="ui negative message"><i class="icon close"></i><div class="ui header">Error</div><p>' + message + '</p></div>');
  };

  self.handleFail = function () {
    self.el.find('.response-container').append('<div class="ui negative message"><i class="icon close"></i><div class="ui header">Error</div><p>Sorry there was a problem on our end. Please try again.</p></div>');
  };
};

$(document).ready(function() {
  $('.generic-form').each(function(i, el){
    $(el).data('Form', new dbs.form.genericForm());
    $(el).data('Form').init(el);
  });
});



