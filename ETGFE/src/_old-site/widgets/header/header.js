Promise.all([
  import(/* webpackMode: "eager" */ './header.scss'),
  import('../../plugins/semantic/popup.css'),
  import('../../plugins/semantic/popup'),
  import(/* webpackMode: "eager" */ '../../widgets/site-search/site-search'),
]).then(() => {
  $('.widget.header').each(function (i, el) {
    $(el).data('widget', new Header(el))
    $(el).data('widget').init()
  })
})


function Header (el) {
  const self = this;
  self.el = $(el);
  var currentScrollPosition = 0;

  self.handleScroll = function (){
    if ($(document).scrollTop() > 110){
      self.el.addClass('scrolled');
    }else{
      self.el.removeClass('scrolled');
    }

    var scrollDirection = 'down';

    if($(window).width() >= 829){
      if($(document).scrollTop() >= currentScrollPosition){
        scrollDirection = 'down';
        $(el).removeClass('show-menu');
      } else {
        scrollDirection = 'up';
        $(el).addClass('show-menu');
      }
    }



    currentScrollPosition = $(document).scrollTop();

    $('body').removeClass('up down').addClass(scrollDirection);
  }

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('Header init', self);
    }

    $('.scrollto').click(function(e){
      e.preventDefault();
      if ($('#' + $(this).attr('data-scrollto')).length == 1){

        scrollTo(0,$('#' + $(this).attr('data-scrollto')).position().top - 70)
      }
    })
    if(!$('.header-spacer').length){
      self.el.addClass('transparent');
    }

    setTimeout(function(){
      self.el.find('.mobile-menu').show();
    }, 1000);

    self.el.css('opacity', 1);

    self.el.find('.main-nav nav .item').mouseover(function(event) {
      $(this).siblings('.ui.menu').mouseover();
    });

    self.el.find('.main-nav nav .item').mouseout(function(event) {
      $(this).siblings('.ui.menu').mouseout();
    });

    self.el.find('.ui.menu').popup({
      inline: true,
      hoverable: true,
      position: 'bottom left',
      prefer: 'bottom right',
      lastResort: 'bottom right'
    });

    $(document).on('scroll', function(){
      self.handleScroll();
    });
    $(window).on('resize load', function(){
      self.handleScroll();
    });
    $('body').on('touchmove', function(){
      self.handleScroll();
    });

    self.el.find('.desktop-menu, .close-desktop-menu').click(function(event) {
      self.el.toggleClass('show-menu');
    })

    self.el.find('.mobile-menu-trigger').click(function(event) {
      event.preventDefault();
      self.el.toggleClass('show-mobile-menu');
    });

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

    /*self.el.find('.global-search-button').click(function(event) {
      event.preventDefault();

      self.el.addClass('show-search-overlay');

      self.el.find('.site-search .field input').focus();
    });*/

    self.el.find('.close-search-overlay-button').click(function(event) {
      event.preventDefault();
      self.el.removeClass('show-search-overlay');
    });

    self.el.find('.search-overlay').on('click', function(e) {
      if (e.target !== this)
        return;

      self.el.removeClass('show-search-overlay');
    });
  };
}
