import(/* webpackMode: "eager" */ './why-we-love-it.scss');

function WhyWeLoveIt (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('WhyWeLoveIt init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.why-we-love-it').each(function(i, el){
  $(el).data('widget', new WhyWeLoveIt(el));
  $(el).data('widget').init();
});
