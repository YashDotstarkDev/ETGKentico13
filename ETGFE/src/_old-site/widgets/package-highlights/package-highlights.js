import(/* webpackMode: "eager" */ './package-highlights.scss');

function PackageHighlights (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PackageHighlights init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.package-highlights').each(function(i, el){
  $(el).data('widget', new PackageHighlights(el));
  $(el).data('widget').init();
});
