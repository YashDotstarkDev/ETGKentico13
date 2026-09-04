import(/* webpackMode: "eager" */ './package-overview.scss');

function PackageOverview (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PackageOverview init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.package-overview').each(function(i, el){
  $(el).data('widget', new PackageOverview(el));
  $(el).data('widget').init();
});
