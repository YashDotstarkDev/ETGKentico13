import(/* webpackMode: "eager" */ './package-optional-extras.scss');

$('.widget.package-optional-extras').each(function(i, el) {
  $(el).data('widget', new PackageOptionalExtras(el));
  $(el).data('widget').init();
});

function PackageOptionalExtras(el) {
  const self = this;
  self.el = $(el);

  self.init = function() {
    if (process.env.NODE_ENV === 'development') {
      console.log('PackageOptionalExtras init', self);
    }
    self.el.css('opacity', 1);
  }
}


