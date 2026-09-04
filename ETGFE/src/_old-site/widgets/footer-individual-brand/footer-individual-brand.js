import(/* webpackMode: "eager" */ './footer-individual-brand.scss');

function FooterIndividualBrand (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('FooterIndividualBrand init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.footer-individual-brand').each(function(i, el){
  $(el).data('widget', new FooterIndividualBrand(el));
  $(el).data('widget').init();
});
