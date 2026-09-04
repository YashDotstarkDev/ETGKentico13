import(/* webpackMode: "eager" */ './career-callout.scss');

function CareerCallout (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('CareerCallout init', self);
    }
    self.el.css('opacity', 1);

    self.el.find('.vacancies-trigger').click(function(event) {
      event.preventDefault();

      $('html, body').animate({scrollTop: $('.career-opportunities').offset().top - 60}, 800);

    });
  }
}

$('.widget.career-callout').each(function(i, el){
  $(el).data('widget', new CareerCallout(el));
  $(el).data('widget').init();
});
