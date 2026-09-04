import(/* webpackMode: "eager" */ './package-enquiry.scss');

function PackageEnquiry (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PackageEnquiry init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.package-enquiry').each(function(i, el){
  $(el).data('widget', new PackageEnquiry(el));
  $(el).data('widget').init();
});
