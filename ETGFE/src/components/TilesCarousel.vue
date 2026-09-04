<template>
  <div>
    <slot></slot>
  </div>
</template>

<script>
import EventBus from "./base/EventBus.ts";
import Swiper from 'swiper';
import {FreeMode, Scrollbar, Navigation} from 'swiper/modules';
// import Swiper and modules styles
import 'swiper/css';
import 'swiper/css/free-mode';
import 'swiper/css/scrollbar';
import 'swiper/css/navigation';
export default {
  props: {
    spaceBetween: {
      type: Number,
      default: 20
    },
    centerInsufficientSlides: {
      type: Boolean,
      default: false
    }
  },
  mounted() {

    let scrollbar = false;
    if (this.$el.querySelector(".swiper-scrollbar")) {
      scrollbar = {
        el: this.$el.querySelector(".swiper-scrollbar"),
        hide: false,
        draggable: true,
      };
    }

    let navigation = false;
    if (this.$el.querySelector(".button-next")) {
      navigation = {
        nextEl: this.$el.querySelector(".button-next"),
        prevEl: this.$el.querySelector(".button-prev")
      }
    }

    const swiper = new Swiper(this.$el.querySelector(".swiper"), {
      modules: [ FreeMode, Scrollbar, Navigation ],
      loop: false,
      freeMode: {
        enabled: true,
        sticky: true,
        momentum: true,
      },
      slidesPerView: 1,
      spaceBetween: this.spaceBetween,
      scrollbar: scrollbar,
      navigation: navigation,
      centerInsufficientSlides: this.centerInsufficientSlides,
      breakpoints: {
        640: {
          slidesPerView: 2
        },
        1024: {
          slidesPerView: 3
        },
        1280: {
          slidesPerView: 4
        }
      }
    });
  }
}
</script>
