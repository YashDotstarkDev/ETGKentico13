<script lang="ts">
import {defineComponent} from 'vue'
import { gsap } from "gsap";
import { ScrollTrigger } from "gsap/ScrollTrigger";
gsap.registerPlugin(ScrollTrigger);

export default defineComponent({
  name: "ParallaxPicture",
  props: {
    mobileImage: {
      type: String,
      required: true,
    },
    desktopImage: {
      type: String,
      required: true,
    },
    alt: {
      type: String,
    },
    breakpoint: {
      type: String,
      default: "550",
    },
    top: {
      type: String,
      default: "200",
    },
    move: {
      type: String,
      default: "200",
    },
    start: {
      type: String,
      default: "top bottom",
    },
    end: {
      type: String,
      default: "bottom top",
    }
  },
  mounted() {
    const img = this.$el.querySelector("img");

    gsap.to(img, {
      top: parseInt(this.move),
      ease: "none",
      scrollTrigger: {
        trigger: this.$el,
        markers: false,
        scrub: 0,
        start: this.start,
        end: this.end,
        pin: false
      }
    });
  },
})
</script>

<template>
  <div class="parallax-container absolute inset-0 " :style="`top:-${top}px`">
    <picture>
      <!-- Mobile image -->
      <source :data-src="mobileImage" :media="`(max-width: ${breakpoint}px)`" />
      <!-- Desktop image -->
      <source :data-src="desktopImage" />

      <!-- Desktop image -->
      <img class="lazyload absolute left-0 top-0 w-full h-full object-cover" data-sizes="auto" :data-src="desktopImage" :alt="alt" />
    </picture>
  </div>
</template>
