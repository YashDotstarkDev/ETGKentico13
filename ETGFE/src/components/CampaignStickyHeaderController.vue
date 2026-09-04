<script>
import {defineComponent} from 'vue'
import Swiper from 'swiper';
import {Autoplay} from 'swiper/modules';
// import Swiper and modules styles
import 'swiper/css';

export default defineComponent({
  name: "CampaignStickyHeaderController",
  data () {
    return {
      end: null
    }
  },
  methods: {
    onScroll() {
      const self = this;
      if (document.documentElement.scrollTop > 200 || document.body.scrollTop > 200) {
        self.$el.classList.add('scrolled');
      } else {
        self.$el.classList.remove('scrolled');
      }
    },
    updateTimer() {
      let endTime = new Date(this.end);
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
  },
  mounted() {
    const self = this;
    document.addEventListener('scroll', self.onScroll);

    self.end = self.$el.querySelector('.js-discounts-offers-timer').dataset.end;

    // if there's a .anchor-navigation element - change the top to 170px
    const jumpMenu = document.querySelector('.anchor-navigation');
    if (jumpMenu) {
      jumpMenu.style.top = '170px';
    }

    // Update timer immediately on page load
    self.updateTimer();

    // Then update every second...
    setInterval(function() {
      self.updateTimer();
    }, 1000);

    const swiper = new Swiper(this.$el.querySelector(".swiper"), {
      modules: [Autoplay],
      loop: true,
      slidesPerView: 'auto',
      autoplay: {
        delay: 0,
        disableOnInteraction: false
      },
      speed: 20000,
      spaceBetween: 0,
      easing: 'linear',
      touchmove: false,
    });
  },
})
</script>

<template>
  <div>
    <slot></slot>
  </div>
</template>

<style scoped lang="scss">

</style>