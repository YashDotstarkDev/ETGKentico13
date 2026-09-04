import(/* webpackMode: "eager" */ './landing-sticky-footer.scss');

function LandingStickyFooter (el) {
  const self = this;
  self.el = $(el);

  self.handleScroll = function (){
    if ($(document).scrollTop() > 200){
      self.el.addClass('scrolled');
    }else{
      self.el.removeClass('scrolled');
    }
  }

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('LandingStickyFooter init', self);
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

    if($(window).width() <= 480){
      self.el.find('.bar').click(function() {
        console.log('clicked');
        $(this).toggleClass('expand-sticky')
      });
    }


  }
}

$('.widget.landing-sticky-footer').each(function(i, el){
  $(el).data('widget', new LandingStickyFooter(el));
  $(el).data('widget').init();
});
