<script>
import { defineComponent } from "vue";

import { gsap } from "gsap";
import { ScrollTrigger } from "gsap/ScrollTrigger";
gsap.registerPlugin(ScrollTrigger);

export default defineComponent({
  name: "Parallax",
  mounted() {
    const self = this;
    self.init();
  },
  methods: {
    init() {
      const self = this;

      console.log("Parallax init");

      const parallaxElements = document.querySelectorAll(".parallax");

      if ("undefined" !== parallaxElements && parallaxElements.length > 0) {
        parallaxElements.forEach((element) => {
          const tl = gsap.timeline({
            defaults: {
              ease: "none",
            },
            scrollTrigger: {
              trigger: element.dataset.trigger === 'body' ? document.body : element.parentElement,
              start: element.dataset.start || "top bottom",
              end: element.dataset.end || "bottom top",
              pin: false,
              invalidateOnRefresh: true,
              markers: false,
              scrub: true,
            },
          });

          tl.to(element, {
            y: element.dataset.y || "0%",
            x: element.dataset.x || "0%",
          });
        });
      }
    },
  },
});
</script>

<template></template>
