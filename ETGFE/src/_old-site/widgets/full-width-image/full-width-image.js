import(/* webpackMode: "eager" */ './full-width-image.scss');

function FullWidthImage (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('FullWidthImage init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.full-width-image').each(function(i, el){
  $(el).data('widget', new FullWidthImage(el));
  $(el).data('widget').init();
});
