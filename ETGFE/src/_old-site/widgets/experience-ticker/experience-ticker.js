import(/* webpackMode: "eager" */ './experience-ticker.scss');

function ExperienceTicker (el, Swiper) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('ExperienceTicker init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.experience-ticker').each(function(i, el){
  $(el).data('widget', new ExperienceTicker(el));
  $(el).data('widget').init();
});