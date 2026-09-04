<script lang="ts">
import {defineComponent} from 'vue'
import { gsap } from "gsap";
import { ScrollTrigger } from "gsap/ScrollTrigger";
gsap.registerPlugin(ScrollTrigger);

export default defineComponent({
  name: "ParallaxImage",
  props: {
    image: {
      type: String,
      required: true,
    },
    alt: {
      type: String,
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
  <div class="parallax-container absolute inset-0" :style="`top:-${top}px`">
    <img class="lazyload absolute !m-0 block h-full w-full object-cover" :data-src="image" data-sizes="auto" :alt="alt">
  </div>
</template>
