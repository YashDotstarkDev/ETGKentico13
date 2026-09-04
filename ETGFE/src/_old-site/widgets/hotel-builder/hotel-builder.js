Promise.all([
  import(/* webpackMode: "eager" */ './hotel-builder.scss'),
  import('../../dbs/scripts/form/dbs.semantic.form.js'),
  import('../../plugins/semantic/form.scss'),
  import('../../plugins/semantic/form.js'),
  import('../../plugins/semantic/checkbox.css'),
  import('../../plugins/semantic/checkbox.js'),
  import('../../plugins/semantic/dropdown.scss'),
  import('../../plugins/semantic/dropdown.js'),
  import('../../plugins/semantic/popup.css'),
  import('../../plugins/semantic/popup.js'),
  import('../../plugins/jquery-ui/custom')
]).then(() => {
  $('.hotel-builder').each(function (i, el) {
    $(el).data('widget', new HotelBuilder(el))
    $(el).data('widget').init(true)
  })
})

function HotelBuilder (el) {
  const self = this;
  self.el = $(el);
  self.initialLoad = true;
  self.hotels = [];
  self.results = '';
  self.selectedHotelList  = [];

  self.getHotels = function () {
    $.ajax({
      url: self.el.attr('data-endpoint'),
      type: self.el.attr('data-method'),
      contentType: 'application/json'
    })
      .done(function(response) {

        self.hotels = response.hotels

        self.fuse = new Fuse(self.hotels, {
          threshold: 0.2,
          keys: ['title', 'location']
        })

        if (self.el.find('.search-field').val() == ''){
          self.el.find('.checkboxes .inner').empty();
          return;
        }
        var checkboxes = '';
        $.each(self.hotels, function(index, value) {
          checkboxes += '<div class="item">';
          checkboxes += '<label for="' + index + '"><span>' + value.title + '</span><span>' + value.location + '</span></label>';
          // checkboxes += '<input id="' + index + '" type="checkbox" value="' + value.title + '"/>';
          checkboxes += '<a href="#" class="ui button primary add small" data-title="' + value.title + '">Add</a>';
          checkboxes += '</div>';
        });
        self.el.find('.checkboxes .inner').empty();
        self.el.find('.checkboxes .inner').html(checkboxes)

      })
      .fail(function() {
        console.log('fail');
      })
      .always(function(){
      });
  }

  self.showClear = function (){
    if (self.el.find('.selected-hotels .inner-container').html().length){
      self.el.find('.clear').show();
    }else{
      self.el.find('.clear').hide();
    }
  }

  self.displayHotels = function (searchText){
    if (searchText == ''){
      self.getHotels();
    }else{
      self.fuse.search(searchText);
      self.results = self.fuse.search(searchText);

      self.el.find('.checkboxes').removeClass('show');
      var hotels = '';
      $.each(self.results, function(index, value) {
        hotels += '<div class="item">';
        hotels += '<label for="' + index + '"><span>' + value.item.title + '</span><span>' + value.item.location + '</span></label>';
        hotels += '<a href="#" class="ui button primary add small" data-id="' + value.item.id + '" data-title="' + value.item.title + '"><span class="label-add">Add</span><span class="label-added">Added</span></a>';
        hotels += '</div>';
      });
      self.el.find('.checkboxes .inner').empty();

      if (hotels != ''){
        self.el.find('.checkboxes').addClass('show');
        self.el.find('.checkboxes .inner').html(hotels)
      }
    }
  }

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('HotelBuilder init', self);
    }
    self.el.css('opacity', 1);

    self.getHotels();

    self.showClear();


    self.el.find('.search-field').on('keyup', function(){
      self.displayHotels($(this).val())

    });



    $('body').on('click', '.hotel-builder .add', function(event){
      event.preventDefault();

      if ($(this).hasClass('added')){
        return;
      }
      $(this).addClass("added");
      var hotel = {
        id:$(this).attr('data-id'),
        title:$(this).attr('data-title')
      }

      self.selectedHotelList.push(hotel);

      function noDupliateList(list) {
        var result = [];
        $.each(list, function(i, e) {
          if ($.inArray(e, result) == -1) result.push(e);
        });
        return result;
      }

      self.selectedHotelList = noDupliateList(self.selectedHotelList);

      self.visibleList = self.selectedHotelList.map(function(value) {
        return value.title;
      }).join(', ')

      self.el.find('.selected-hotels .inner-container').html(self.visibleList);

      self.showClear();

    });

    $('body').on('click', '.hotel-builder .clear', function(event){
      event.preventDefault();

      self.el.find('.added').removeClass('added')
      self.visibleList = [];
      self.selectedHotelList = [];

      self.el.find('.selected-hotels .inner-container').html('');

      self.showClear();

    });


    $('body').on('click', '.hotel-builder .show-dropdowns', function(event){
      event.preventDefault();

      // var checkedHotels = $('.hotel-builder .checkboxes input:checked').map(function(){
      //   return this.value;
      // }).toArray();

      self.el.find('.step-one').hide();
      var dropdowns = '<div class="item item-header"><span class="title-hotel">Hotel</span><span class="title-nights">No. of nights</span></div>';
      for (let i = 1; i <= self.selectedHotelList.length + 2; i++) {
        dropdowns += '<div class="item">';
        dropdowns += '<label>Hotel ' + i + '</label>';
        dropdowns += '<select name="hotel' + i + '">';
        dropdowns += '<option value="">Please select</option>';
        $.each(self.selectedHotelList, function (index, value) {
          dropdowns += '<option value="' + value.id + '">' + value.title + '</option>';
        })
        dropdowns += '</select>';
        dropdowns += '<input type="text" name="night' + i + '" class="nights"/>';
        dropdowns += '</div>';
      }
      self.el.find('.step-two').show();
      self.el.find('.dropdowns').empty();
      self.el.find('.dropdowns').html(dropdowns)

    });

    $('body').on('change', '.hotel-builder .step-two select', function(event){

      var val = '';
      self.el.find('select').each(function(index, e){

        if (index == 0){
          val = $(this).val();
        }else{
          val+= ',' + $(this).val();

        }
      })
      self.el.find('#hidHotelIds').val(val);
    });

    $('body').on('keyup', '.hotel-builder .step-two .nights', function(event){

      val = '';
      self.el.find('.nights').each(function(index, e){
        if (index == 0){
          val = $(this).val();
        }else{
          val+= ',' + $(this).val();

        }
      })
      self.el.find('#hidNights').val(val);
    });
  }
}
