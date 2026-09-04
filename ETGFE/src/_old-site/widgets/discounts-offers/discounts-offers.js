Promise.all([
  import(/* webpackMode: "eager" */ './discounts-offers.scss'),
  import('../../plugins/timeto/timeto.js'),
  import('../../plugins/timeto/timeto.css'),
]).then(() => {
  $('.widget.discounts-offers').each(function (i, el) {
    $(el).data('widget', new DiscountsOffers(el))
    $(el).data('widget').init()
  })
})

function DiscountsOffers(el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('DiscountsOffers init', self);
    }
    self.el.css('opacity', 1);

    function updateTimer() {
      const end = self.el.find('.js-discounts-offers-timer').attr('data-end');

      let endTime = new Date(end);
      let endTimeInSeconds = (Date.parse(endTime) / 1000);

      let nowTime = new Date();
      let nowTimeInSeconds = (Date.parse(nowTime) / 1000);

      let timeLeft = endTimeInSeconds - nowTimeInSeconds;

      if (timeLeft < 0) {
        timeLeft = 0;
      }

      // Calculate
      let timer = {}
      timer.days = Math.floor(timeLeft / 86400);
      timer.hours = Math.floor((timeLeft - (timer.days * 86400)) / 3600);
      timer.minutes = Math.floor((timeLeft - (timer.days * 86400) - (timer.hours * 3600)) / 60);
      timer.seconds = Math.floor((timeLeft - (timer.days * 86400) - (timer.hours * 3600) - (timer.minutes * 60)));


      let units = document.querySelectorAll('.js-discounts-offers-timer [data-timer-unit]');
      units.forEach((unit) => {
        unit.textContent = timer[unit.dataset.timerUnit] < "10" ? "0" + timer[unit.dataset.timerUnit] : timer[unit.dataset.timerUnit]
      })
    }

    // Update timer immediately on page load
    updateTimer();

    // Then update every second...
    setInterval(function() {
      updateTimer();
    }, 1000);
  }
}


