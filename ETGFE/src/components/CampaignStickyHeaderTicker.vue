<template>
  <div>
    <slot></slot>
  </div>
</template>

<script>
import EventBus from "./base/EventBus.ts";
import Swiper from 'swiper';
import {Autoplay} from 'swiper/modules';
// import Swiper and modules styles
import 'swiper/css';
export default {
  props: {
    spaceBetween: {
      type: Number,
      default: 0
    },
    centerInsufficientSlides: {
      type: Boolean,
      default: false
    }
  },
  mounted() {

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
  }
}
</script>
