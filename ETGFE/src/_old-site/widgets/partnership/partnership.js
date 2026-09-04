import(/* webpackMode: "eager" */ './partnership.scss');

function partnership (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('partnership init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.partnership').each(function(i, el){
  $(el).data('widget', new partnership(el));
  $(el).data('widget').init();
});
