import(/* webpackMode: "eager" */ './package-offer.scss');

function PackageOffer (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PackageOffer init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.package-offer').each(function(i, el){
  $(el).data('widget', new PackageOffer(el));
  $(el).data('widget').init();
});
