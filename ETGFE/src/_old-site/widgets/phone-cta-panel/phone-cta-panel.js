import(/* webpackMode: "eager" */ './phone-cta-panel.scss');

function PhoneCtaPanel (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PhoneCtaPanel init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.phone-cta-panel').each(function(i, el){
  $(el).data('widget', new PhoneCtaPanel(el));
  $(el).data('widget').init();
});
