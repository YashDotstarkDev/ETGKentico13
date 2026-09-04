import(/* webpackMode: "eager" */ './package-detail-title.scss');

function PackageDetailTitle (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PackageDetailTitle init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.package-detail-title').each(function(i, el){
  $(el).data('widget', new PackageDetailTitle(el));
  $(el).data('widget').init();
});
