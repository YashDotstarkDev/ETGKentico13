<script lang="ts">
import {defineComponent} from 'vue'

export default defineComponent({
  name: "Loader",
  props: {
    active: {
      type: Boolean,
      default: false,
    },
  },
})
</script>

<template>
  <div class="loader-main absolute flex bg-white/70 inset-0 items-center justify-center transition-all duration-500 z-[100] rounded-lg" :class="[{'opacity-0 invisible': !active, 'opacity-100 visible': active}]">
    <div class="loader-container">
      <span class="loader"></span>
    </div>
  </div>
</template>

<style scoped lang="scss">
.loader {
  width: 48px;
  height: 48px;
  border: 5px solid #EDEDED;
  border-bottom-color: #091D3A;
  border-radius: 50%;
  display: inline-block;
  box-sizing: border-box;
  animation: rotation 1s linear infinite;
}

@keyframes rotation {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>
