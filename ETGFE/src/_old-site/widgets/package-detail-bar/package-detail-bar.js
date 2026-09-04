import(/* webpackMode: "eager" */ './package-detail-bar.scss');

function PackageDetailBar (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PackageDetailBar init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.package-detail-bar').each(function(i, el){
  $(el).data('widget', new PackageDetailBar(el));
  $(el).data('widget').init();
});
