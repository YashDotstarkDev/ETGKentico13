<script lang="ts">
import {defineComponent} from 'vue'

export default defineComponent({
  name: "ReviewBlurButton",
  data() {
    return {
      active: false
    }
  },
  methods: {
    toggle() {
      document.activeElement?.blur()
    }
  }
})
</script>

<template>
  <button type="button" class="text-primary mt-2 hidden group-focus-within:inline" @click="toggle">Read less</button>
</template>

<style scoped lang="scss">

</style>