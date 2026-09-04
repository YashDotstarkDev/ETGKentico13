import(/* webpackMode: "eager" */ './simple-footer.scss');

function SimpleFooter (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('SimpleFooter init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.simple-footer').each(function(i, el){
  $(el).data('widget', new SimpleFooter(el));
  $(el).data('widget').init();
});
