import('../../plugins/numeraljs/numeral.min.js').then(({ default: numeral }) => {
  window.numeral = numeral
  import('lodash').then(({ default: lodash }) => {
    window._ = lodash
    Promise.all([
      import(/* webpackMode: "eager" */ './departures-pricing.scss'),
      import('../../plugins/semantic/form.scss'),
      import('../../plugins/semantic/form.js'),
      import('../../plugins/semantic/checkbox.css'),
      import('../../plugins/semantic/checkbox.js'),
      import('../../plugins/semantic/dropdown.scss'),
      import('../../plugins/semantic/dropdown.js'),
      import('../../dbs/scripts/form/dbs.semantic.form.js'),
    ]).then(() => {
      $('.widget.departures-pricing').each(function (i, el) {
        if (!$(this).hasClass('no-flight')) {
          $(el).data('widget', new DeparturesPricing(el))
          $(el).data('widget').init()
        } else {
          $(el).data('widget', new NoFlightPricing(el))
          $(el).data('widget').init()
        }
      })
    })
  })
})

function DeparturesPricing (el) {
  const self = this;
  self.el = $(el);

  self.data = [];


  self.parseData = function () {
    self.el.find('.datapricing').each(function(i, el){
      self.data.push({
        date: $(el).attr('data-date'),
        cls: $(el).attr('data-class'),
        city: $(el).attr('data-city'),
        price: $(el).attr('data-price'),
        priceInAUD: $(el).attr('data-price-aud')
      });
    });

  };


  self.clearPriceHiddenFields = function(){
    if ($('.hidden-price').length > 0){
      $('.hidden-price').val('')
    }
  }


  self.setClassHiddenFieldAfterSelectUpdate = function(classValue){
    self.clearPriceHiddenFields();
    if ($('.hidden-class').length > 0){
      $('.hidden-class').val(classValue)
    }
  }

  self.handleDepartureChange = function (initialClass, initialCity) {

    // clone original data
    self.filteredData = _.cloneDeep(self.data);
    // filter by departure
    var date = self.el.find('.select-date').dropdown('get value');

    self.filteredData = _.filter(self.filteredData, {date: date});
    self.cities = _.uniqBy(self.filteredData, 'city');
    self.classes = _.uniqBy(self.filteredData, 'cls');
    // clear class
    self.el.find('.city-dropdown select').dropdown('clear');


    // re-render classes
    var classHtml;
    classHtml = '<select name="flightclass" class="select-class">';
    for (var i = 0; i < self.classes.length; i++) {
      var clsItem = self.classes[i];
      var selected = "";
      if (initialClass == clsItem.cls){
        selected = " selected"
      }
      classHtml += '<option value="' + clsItem.cls + '"' + selected +'>' + clsItem.cls + '</option>'
    }
    classHtml += '</select>';
    self.el.find('.class-dropdown').html(classHtml);
    self.el.find('.class-dropdown select').dropdown({
      placeholder: false,
      onChange: function(value) {
        self.handleClassChange();
      }
    });

    if (selected == "" && self.classes.length > 0){
      initialClass = self.classes[0].cls;
    }
    self.setClassHiddenFieldAfterSelectUpdate(initialClass);
    self.handleClassChange(initialCity);

  };

  self.handleClassChange = function (initialCity) {
    // clear city select
    self.el.find('.city-dropdown select').dropdown('clear');


    // filter by class
    // clone original data
    self.filteredData = _.cloneDeep(self.data);

    // filter by departure
    var date = self.el.find('.select-date').dropdown('get value');
    var cls = self.el.find('.select-class').dropdown('get value');
    self.filteredData = _.filter(self.filteredData, {date: date});
    self.filteredData = _.filter(self.filteredData, {cls: cls});
    self.cities = _.uniqBy(self.filteredData, 'city');

    // re-render cities
    var cityHtml;
    cityHtml = '<select name="city" class="select-city">';
    for (var i = 0; i < self.cities.length; i++) {
      var cityItem = self.cities[i];
      var selected = "";

      if (initialCity == cityItem.city){
        selected = " selected";
      }
      cityHtml += '<option value="' + cityItem.city + '"' + selected + '>' + cityItem.city + '</option>'
    }
    cityHtml += '</select>';
    self.el.find('.city-dropdown').html(cityHtml);
    self.el.find('.city-dropdown select').dropdown({
      placeholder: false,
      onChange: function(value) {
        self.handleCityChange();
      }
    });

    if(self.el.find('.ui.form').data('Form')) {
      self.el.find('.ui.form').data('Form').init(self.el.find('.ui.form'))
    }

    if (selected == "" && self.cities.length > 0){
      initialCity = self.cities[0].city;
    }


    if ($('.hidden-city').length > 0){
      $('.hidden-city').val(initialCity)
    }

    self.handleCityChange();

  };


  self.showAUDPricePopup = function (el) {

      var content = self.el.find('.see-in-aud').attr('data-content-template');
      var currentForeignPrice = GetDisplayPrice(self.el);
      var audPrice = GetAudPriceFromForeignPrice(currentForeignPrice, self.data) ;
      var formattedPrice = numeral(audPrice).format('0,0');

      var exchangeRate = self.el.find('.price').attr('data-exchange-rate');
      var currency = self.el.find('.price').attr('data-currency');

      content = content.replace('[price]', formattedPrice).replace('[rate]', exchangeRate).replace('[foreigncurrency]', currency);

      $(el).attr('data-content',  content)

  };

  self.handleCityChange = function () {
    // clone original data
    self.filteredData = _.cloneDeep(self.data);

    // filter by departure
    var date = self.el.find('.select-date').dropdown('get value');
    var cls = self.el.find('.select-class').dropdown('get value');
    var city = self.el.find('.select-city').dropdown('get value');
    self.filteredData = _.filter(self.filteredData, {date: date});
    self.filteredData = _.filter(self.filteredData, {cls: cls});
    self.filteredData = _.filter(self.filteredData, {city: city});

    setTimeout(function(){
      displayLowestPrice(self);
    }, 500);

  };



  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('DeparturesPricing init', self);
    }
    self.el.css('opacity', 1);

    self.parseData();

    self.el.find('.see-in-aud').click(function(e, el){
      e.preventDefault();

      self.showAUDPricePopup($(this));

    })

    self.el.find('.see-in-aud').popup({
      on: 'click'
    });

    self.el.find('.departure-dropdown select').dropdown({
      onChange: function(value) {

        self.handleDepartureChange();
      }
    });

    self.el.find('.class-dropdown select').dropdown({
      onChange: function(value) {
        self.handleClassChange();
      }
    });

    self.el.find('.city-dropdown select').dropdown();

    if ($('.hidden-class').length > 0 && $('.hidden-class').val()!= ""){
      self.handleDepartureChange($('.hidden-class').val(), $('.hidden-city').val());
    }else
    {
      self.handleDepartureChange();
    }

  }
}


function NoFlightPricing (el) {
  const self = this;
  self.el = $(el);

  self.data = [];
  self.parseData = function () {

    if (self.el.find('.datapricing').length > 0){

      self.el.find('.datapricing').each(function(i, el){
        self.data.push({
          date: $(el).attr('data-date'),
          price: $(el).attr('data-price'),
          priceInAUD: $(el).attr('data-price-aud')
        });
      });
    }else{
      self.data.push({
        date: self.el.find('.price span').attr('data-date'),
        price: self.el.find('.price span').attr('data-price'),
        priceInAUD: self.el.find('.price span').attr('data-price-aud')
      });
    }

  };
  self.handleDateChange = function () {
    // clone original data
    self.filteredData = _.cloneDeep(self.data);

    // filter by departure
    var date = self.el.find('.select-date').dropdown('get value');
    self.filteredData = _.filter(self.filteredData, {date: date});

    setTimeout(function(){
      displayLowestPrice(self);
    }, 500);

  };
  self.showAUDPricePopup = function (e) {

    var content = $(e).attr('data-content-template');
    var currentForeignPrice = GetDisplayPrice(self.el);
    var audPrice = 0;

    if (self.el.find('.select-date').length>0){

      audPrice = GetAudPriceFromForeignPrice(currentForeignPrice, self.data) ;
    }else
    {
      audPrice = self.el.find('.price span').attr('data-aud-price');
    }

    var formattedPrice = numeral(audPrice).format('0,0');

    var exchangeRate = self.el.find('.price').attr('data-exchange-rate');
    var currency = self.el.find('.price').attr('data-currency');

    content = content.replace('[price]', formattedPrice).replace('[rate]', exchangeRate).replace('[foreigncurrency]', currency);

    content = content.replace('[price]', formattedPrice)
    $(e).attr('data-content',  content)

};

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('NoFlightPricing init', self);
    }
    self.el.css('opacity', 1);


    self.el.find('.see-in-aud').click(function(e, el){
      e.preventDefault();

      self.showAUDPricePopup($(this));

    })

    self.el.find('.see-in-aud').popup({
      on: 'click'
    });

    if (self.el.find('.select-date').length>0){
      self.parseData();
      self.handleDateChange();

      self.el.find('.dropdown-date select').dropdown({
        onChange: function(value) {
          self.handleDateChange();
        }
      });
      self.el.find('.select-date').dropdown({
        onChange: function(value) {
          self.handleDateChange();
        }
      });

    }else{
      self.el.find('.hidden-price').val(self.el.find('.price span').attr('data-price'));
    }


  }

}

function displayLowestPrice(self){
  var lowestPrice = getLowestPrice(self);
  $(self.el).find('.hidden-price').val(lowestPrice.value());

  displayPrice(lowestPrice, self.el);
}

function getLowestPrice(self) {

  self.priceData = _.cloneDeep(self.filteredData);
  var priceArray = _.sortBy(self.priceData, 'price');

  if(priceArray.length > 1) {
    // show "from"
    self.el.find('.from').show();
  } else {
    // hide "from"
    self.el.find('.from').hide();
  }

  return numeral(priceArray[0].price);

};

function GetAudPriceFromForeignPrice(foreignPrice, data){
  if (data == null || data.length ==0){
    return "";
  }

  for (var i=0; i<data.length;i++){
    if (data[i].price == foreignPrice){
      return data[i].priceInAUD;
    }
  }

  return "";
}

function GetDisplayPrice(el){
  return $(el).find('.price span').attr('data-price');
}

function displayPrice(price, el){
  var currencySymbol = $(el).find('.price').attr('data-currency-symbol')
  $(el).find('.price span').attr('data-price', price.value())
  $(el).find('.price span').html(currencySymbol + price.format('0,0'));
}


