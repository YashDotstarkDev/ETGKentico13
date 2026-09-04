<script>
import { defineComponent } from "vue";

export default defineComponent({
  mounted() {
    document.querySelectorAll(".mobile-menu .menu-item").forEach((item) => {
      item.addEventListener("click", () => {
        item.parentNode.classList.toggle("active");
      });
    });
    document.querySelectorAll(".mobile-menu .back-button").forEach((item) => {
      item.addEventListener("click", (e) => {
        e.preventDefault();
        item.parentNode.parentNode.parentNode.parentNode.classList.remove("active");
      });
    });
    document.querySelectorAll(".mobile-menu .overlay").forEach((item) => {
      item.addEventListener("click", (e) => {
        e.preventDefault();
        document.body.classList.remove("show-mobile-menu");
      });
    });
  },
});
</script>

<template></template>
