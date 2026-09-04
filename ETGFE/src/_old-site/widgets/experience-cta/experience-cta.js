import(/* webpackMode: "eager" */ './experience-cta.scss');

function ExperienceCta (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('ExperienceCta init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.experience-cta').each(function(i, el){
  $(el).data('widget', new ExperienceCta(el));
  $(el).data('widget').init();
});
