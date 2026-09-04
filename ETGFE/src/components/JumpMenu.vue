<script lang="ts">
import {defineComponent} from 'vue'

export default defineComponent({
  name: "JumpMenu",
  props: {
    options: {
      type: Array,
      default: () => []
    }
  },
  methods: {
    handleChange(event) {
      if (event.target.value.length === 0) {
        return
      }

      window.location.href = event.target.value
    }
  }
})
</script>

<template>
  <select @change="handleChange" class="text-base text-white bg-[#333] ring-0 focus:ring-0 focus:outline-0 focus:outline-none border-b border-white/10 py-3 appearance-none bg-select-arrow bg-no-repeat bg-[right_0_center] pr-10 cursor-pointer pl-2">
    <option v-for="option in options" :key="option.url" :value="option.url">{{ option.label }}</option>
  </select>
  <nav class="absolute w-0 h-0 overflow-hidden">
    <ul>
      <li v-for="option in options" :key="option.url">
        <a :href="option.url" v-if="option.url.length">{{ option.label }}</a>
      </li>
    </ul>
  </nav>
</template>

<style scoped lang="scss">

</style>