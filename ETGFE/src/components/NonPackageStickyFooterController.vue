<script>
import {defineComponent} from 'vue'

export default defineComponent({
  name: "NonPackageStickyFooterController",
  data () {
    return {
      end: null
    }
  },
  methods: {
    onScroll() {
      const self = this;
      if (document.documentElement.scrollTop > 200 || document.body.scrollTop > 200) {
        self.$el.classList.add('scrolled');
      } else {
        self.$el.classList.remove('scrolled');
      }
    },
  },
  mounted() {
    const self = this;
    document.addEventListener('scroll', self.onScroll);

    // any time this element is loaded, add 90px bottom padding to the footer
    document.body.classList.add('has-non-package-sticky-footer');
  },
})
</script>

<template>
  <div>
    <slot></slot>
  </div>
</template>

<style scoped lang="scss">

</style>