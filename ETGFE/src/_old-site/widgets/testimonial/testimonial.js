import(/* webpackMode: "eager" */ './testimonial.scss');

function Testimonial (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('Testimonial init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.testimonial').each(function(i, el){
  $(el).data('widget', new Testimonial(el));
  $(el).data('widget').init();
});
