<script lang="ts">
import {defineComponent} from 'vue'

export default defineComponent({
  name: "FAQs",
  mounted () {
    const itemDivs = this.$el.querySelectorAll('.question')

    itemDivs.forEach(function(elem) {
      elem.addEventListener('click', function (event) {
        event.preventDefault();
        event.target.closest('.item').classList.toggle('active')
      });
    });
  }
})
</script>

<template>
  <div>
    <slot></slot>
  </div>
</template>
