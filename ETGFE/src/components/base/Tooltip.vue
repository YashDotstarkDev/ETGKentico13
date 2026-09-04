<script lang="ts">
import {defineComponent} from 'vue'

export default defineComponent({
  name: "Tooltip"
})
</script>

<template>
  <div tabindex="1" class="relative w-[15px] h-[15px] group">
    <i class="fa-solid fa-info-circle text-teal text-[15px]"></i>

    <div class="tooltip-content w-[230px] text-center rounded-full absolute bottom-[20px] left-1/2 -translate-x-1/2 bg-primary uppercase text-white px-4 py-2 text-xs opacity-0 pointer-events-none group-focus:opacity-100 group-focus:pointer-events-auto group-hover:opacity-100 group-hover:pointer-events-auto">
      <slot></slot>
      <div class="triangle bg-primary rotate-45 absolute left-1/2 -translate-x-1/2 bottom-[-2px] w-3 h-3"></div>
    </div>
  </div>
</template>

<style scoped lang="scss">

</style>