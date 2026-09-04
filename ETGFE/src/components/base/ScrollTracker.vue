<template>
  <div id="scrollTracker"></div>
</template>

<script>
import EventBus from "./EventBus";
import throttle from "../../scripts/utils/throttle.js";

export default {
  name: "ScrollTracker",
  data() {
    return {
      scrolledThreshold: 20,
      scrolledMoreThreshold: 300,
      moreScrollDistanceThreshold: 50,
      currentScrollTop: 0,
      currentDirection: undefined,
      lastScrollTop: 0,
      lastScrollDistance: 0,
      lastDirectionChangePos: 0,
      lastDirection: undefined,
    };
  },
  mounted() {
    const scrollTop = window.pageYOffset || document.documentElement.scrollTop;

    // Check scroll position on page load
    if (scrollTop > this.scrolledThreshold) {
      document.body.classList.add("scrolled");
    } else {
      document.body.classList.remove("scrolled");
    }

    // Check scroll position on page load
    if (scrollTop > this.scrolledMoreThreshold) {
      document.body.classList.add("scrolled-more");
      document.body.classList.add("scroll-down-more");
    } else {
      document.body.classList.remove("scrolled-more");
    }

    window.addEventListener("scroll", throttle(this.handleScrollEvents, 10), false);


    // const lenis = new Lenis()
    //
    // function raf(time) {
    //   lenis.raf(time)
    //   requestAnimationFrame(raf)
    // }
    //
    // requestAnimationFrame(raf)

  },
  methods: {
    handleScrollEvents() {
      EventBus.emit("scroll");

      // Get the current scroll position
      let scrollTop = window.pageYOffset || document.documentElement.scrollTop;

      // Determine the direction of the scroll
      let newDirection = scrollTop > this.lastScrollTop ? "down" : "up";

      // Update the direction change position and last direction
      if (newDirection !== this.lastDirection) {
        this.lastDirection = newDirection;
        this.lastDirectionChangePos = scrollTop;
      }

      // Handle events based on direction of scroll
      if (newDirection === "down") {
        this.handleScrollingDown(scrollTop);
      } else {
        this.handleScrollingUp(scrollTop);
      }

      // Handle events based on distance from top of page
      if (scrollTop > this.scrolledThreshold) {
        document.body.classList.add("scrolled");
      } else {
        document.body.classList.remove("scrolled");
      }

      if (scrollTop > this.scrolledMoreThreshold) {
        document.body.classList.add("scrolled-more");
      } else {
        document.body.classList.remove("scrolled-more");
      }

      // Remember the last scroll position
      this.lastScrollTop = scrollTop <= 0 ? 0 : scrollTop; // For Mobile or negative scrolling
    },
    handleScrollingDown(scrollTop) {
      document.body.classList.remove("scroll-up");
      document.body.classList.remove("scroll-up-more");

      document.body.classList.add("scroll-down");
      EventBus.emit("scroll-down");

      const distanceScrolled = Math.abs(scrollTop - this.lastDirectionChangePos);
      if (distanceScrolled > this.moreScrollDistanceThreshold) {
        document.body.classList.add("scroll-down-more");
        EventBus.emit("scroll-down-more");
      }
    },
    handleScrollingUp(scrollTop) {
      document.body.classList.remove("scroll-down");
      document.body.classList.remove("scroll-down-more");

      document.body.classList.add("scroll-up");
      EventBus.emit("scroll-up");

      const distanceScrolled = Math.abs(scrollTop - this.lastDirectionChangePos);
      if (distanceScrolled > this.moreScrollDistanceThreshold) {
        document.body.classList.add("scroll-up-more");
        EventBus.emit("scroll-up-more");
      }
    },
  },
};
</script>
