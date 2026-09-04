<script lang="ts">
import {defineComponent} from 'vue'
import EventBus from "./base/EventBus.ts";

export default defineComponent({
  name: "AnchorNavigationController",
  data () {
    return {
      anchors: null,
      sections: null,
      activeSection: null,
      activeIndex: null
    }
  },
  mounted() {
    // in one second
    setTimeout(() => {
      this.anchors = document.querySelectorAll('.anchor-navigation button')
      this.sections = document.querySelectorAll('.anchor-section')
      window.addEventListener('scroll', this.getActiveSection)
      this.getActiveSection()

      // handle anchor clicks
      this.anchors.forEach((anchor, index) => {
        anchor.addEventListener('click', () => {
          console.log('click', index)
          this.activeIndex = index + 1
          this.activeSection = this.sections[index + 1]
          // this.updateActiveSection()
          this.activeSection.scrollIntoView({ behavior: 'smooth' })
        })
      })

    }, 1000)
  },
  methods: {
    getActiveSection () {

      // log how far .anchor-navigation is from the top of the window
      let nav = document.querySelector('.anchor-navigation')
      let header = document.querySelector('header')
      let offset = nav.getBoundingClientRect().top

      if (offset <= 100) {
        nav.classList.add('stuck')
        document.body.classList.add('anchor-stuck')
        header.classList.add('[.scrolled_&]:shadow-transparent')
      } else {
        nav.classList.remove('stuck')
        document.body.classList.remove('anchor-stuck')
        header.classList.remove('[.scrolled_&]:shadow-transparent')
      }


      let newIndex = null
      let newSection = null
      this.sections.forEach((section, index) => {
        if (section.getBoundingClientRect().top <= 150) {
          newIndex = index
          newSection = section
        }
      })

      if (this.activeIndex !== newIndex) {
        this.activeIndex = newIndex
        this.activeSection = newSection
        this.updateActiveSection();
      }
    },
    updateActiveSection () {
      this.anchors.forEach((anchor, index) => {
        if (index === this.activeIndex - 1) {
          anchor.classList.add('active')
        } else {
          anchor.classList.remove('active')
        }
      })
      EventBus.emit('anchor-navigation', this.activeIndex);
    }
  }
})
</script>

<template>
  <div></div>
</template>

<style scoped lang="scss">

</style>