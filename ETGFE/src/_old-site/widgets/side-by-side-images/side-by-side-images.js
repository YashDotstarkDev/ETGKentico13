import(/* webpackMode: "eager" */ './side-by-side-images.scss');

function SideBySideImages (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('SideBySideImages init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.side-by-side-images').each(function(i, el){
  $(el).data('widget', new SideBySideImages(el));
  $(el).data('widget').init();
});
