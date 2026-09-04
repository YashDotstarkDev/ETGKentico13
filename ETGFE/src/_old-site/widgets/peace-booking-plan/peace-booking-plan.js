import(/* webpackMode: "eager" */ './peace-booking-plan.scss');

function PeaceBookingPlan (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PeaceBookingPlan init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.peace-booking-plan').each(function(i, el){
  $(el).data('widget', new PeaceBookingPlan(el));
  $(el).data('widget').init();
});
