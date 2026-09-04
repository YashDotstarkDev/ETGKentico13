Promise.all([
  import(/* webpackMode: "eager" */ './booking-thankyou.scss'),
]).then(() => {
  // then
});

function BookingThankYou (el) {
  const self = this;
  self.el = $(el);
  self.init = function () {
    // init
  }
}


