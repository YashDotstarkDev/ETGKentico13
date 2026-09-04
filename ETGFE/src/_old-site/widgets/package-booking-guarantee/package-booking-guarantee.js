import(/* webpackMode: "eager" */ './package-booking-guarantee.scss');

function PackageBookingGuarantee (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PackageBookingGuarantee init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.package-booking-guarantee').each(function(i, el){
  $(el).data('widget', new PackageBookingGuarantee(el));
  $(el).data('widget').init();
});
