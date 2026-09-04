import(/* webpackMode: "eager" */ './keep-reading.scss');

function KeepReading (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('KeepReading init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.keep-reading').each(function(i, el){
  $(el).data('widget', new KeepReading(el));
  $(el).data('widget').init();
});
