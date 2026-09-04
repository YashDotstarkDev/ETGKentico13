<script lang="ts">
import {defineComponent} from 'vue'

export default defineComponent({
  name: "MenuButton",
  mounted() {
    this.$el.querySelector("button").addEventListener("click", () => {
      document.body.classList.toggle("show-mobile-menu");
    })
  }
})
</script>

<template>
  <div class="menu-button">
    <slot></slot>
  </div>
</template>

<style scoped lang="scss">

</style>
