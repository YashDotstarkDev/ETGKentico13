import(/* webpackMode: "eager" */ './breadcrumbs.scss');

function Breadcrumbs (el) {
  const self = this;
  self.el = $(el);
  self.handleScroll = function (){
    if ($(document).scrollTop() > 110){
      self.el.addClass('scrolled');
    }else{
      self.el.removeClass('scrolled');
    }
  }
  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('Breadcrumbs init', self);
    }
    self.el.css('opacity', 1);

    $(document).on('scroll', function(){
      self.handleScroll();
    });
    $(window).on('resize load', function(){
      self.handleScroll();
    });
    $('body').on('touchmove', function(){
      self.handleScroll();
    });
  }
}

$('.widget.breadcrumbs').each(function(i, el){
  $(el).data('widget', new Breadcrumbs(el));
  $(el).data('widget').init();
});
