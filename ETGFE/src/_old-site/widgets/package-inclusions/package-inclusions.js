import(/* webpackMode: "eager" */ './package-inclusions.scss');

function PackageInclusions (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PackageInclusions init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.package-inclusions').each(function(i, el){
  $(el).data('widget', new PackageInclusions(el));
  $(el).data('widget').init();
});
