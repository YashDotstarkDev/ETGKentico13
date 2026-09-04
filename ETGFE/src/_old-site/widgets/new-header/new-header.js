Promise.all([
  import(/* webpackMode: "eager" */ './new-header.scss'),
  import('../../styles/includes/buttons.scss'),
]).then(() => {
  $('.widget.new-header').each(function (i, el) {
    $(el).data('widget', new NewHeader(el))
    $(el).data('widget').init()
  })
})

function NewHeader (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('NewHeader init', self);
    }
    self.el.css('opacity', 1);

    $('.scrollto').click(function(e){
      e.preventDefault();
      if ($('#' + $(this).attr('data-scrollto')).length == 1){

        scrollTo(0,$('#' + $(this).attr('data-scrollto')).position().top - 70)
      }
    })

    self.el.find('.mobile-menu-trigger').click(function(event) {
      event.preventDefault();
      self.el.toggleClass('show-mobile-menu');
    });

    self.el.find('.close, .fader').click(function(event) {
      event.preventDefault();
      self.el.find('.secondary-container').removeClass('active');
      self.el.find('.primary-container').removeClass('active');
      self.el.removeClass('show-mobile-menu');
    });

    setTimeout(function(){
      self.el.find('.mobile-menu').show();
    }, 1000);


    self.el.find('.explore-dropdown .trigger').on('click', function () {
      self.el.find('.explore-dropdown').toggleClass('active');
      self.el.find('.currency-dropdown').removeClass('active');

      self.el.find('.explore-dropdown .submenu').css('min-height', self.el.find('.explore-dropdown > .menu').outerHeight() + 'px');
    })

    self.el.find('.explore-dropdown div.menu-item').on('click', function () {
      $(this).removeClass('inactive').toggleClass('active').siblings().removeClass('active');

      if($(this).hasClass('active')) {
        $(this).siblings().addClass('inactive');
        self.el.find('.explore-dropdown .menu').css('height', $(this).find('.submenu').outerHeight() + 'px');
      } else {
        $(this).siblings().removeClass('inactive');
        self.el.find('.explore-dropdown .menu').css('height', 'auto');
      }
    })

    self.el.find('.explore-dropdown .overlay').on('click', function () {
      self.el.find('.explore-dropdown, .menu-item').removeClass('active inactive');
      self.el.find('.explore-dropdown .menu').css('height', 'auto');
    })

    self.el.find('.currency-dropdown .overlay').on('click', function () {
      self.el.find('.currency-dropdown, .menu-item').removeClass('active inactive');
    })


    self.el.find('.currency-dropdown .trigger').on('click', function () {
      self.el.find('.currency-dropdown').toggleClass('active');
      self.el.find('.explore-dropdown').removeClass('active');
    })


    self.el.find('.currency-dropdown .menu a').on('click', function () {
      Cookies.set('CurrentCurrency', $(this).data('currency'), {path: '/', expires:365});
      location = "/change-currency"
    })

    self.el.find('.expand').click(function(event) {
      event.preventDefault();
      $(this).parents('li').find('.secondary-container').addClass('active');
      $(this).parents('.primary-container').addClass('active');
    });

    self.el.find('.back').click(function(event) {
      event.preventDefault();
      self.el.find('.secondary-container, .primary-container').removeClass('active');
    });

    self.el.find('.close, .fader').click(function(event) {
      event.preventDefault();
      self.el.find('.secondary-container').removeClass('active');
      self.el.find('.primary-container').removeClass('active');
      self.el.removeClass('show-mobile-menu');
    });

  }
}
