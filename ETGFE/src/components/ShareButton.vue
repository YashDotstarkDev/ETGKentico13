<script lang="ts">
import {defineComponent} from 'vue'

export default defineComponent({
  name: "ShareButton",
  props: {
    url: {
      type: String,
      required: true
    },
    title: {
      type: String,
      required: true
    },
    text: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      shareData: {
        title: this.title,
        text: this.text,
        url: this.url,
      }
    }
  },
  methods: {
    share() {
      if (navigator.share) {
        try {
          navigator.share(this.shareData);
        } catch (err) {
          console.log('Error sharing', err)
        }
      }
    }
  },
  computed: {
    shareApiAvailable () {
      return !!navigator.share
    }
  }
})
</script>

<template>
  <div class="relative" v-if="shareApiAvailable">
    <button rel="button" class="btn btn-primary-outline bg-white" @click="share" tabindex="0">
      <svg width="13" height="11" viewBox="0 0 13 11" fill="none" xmlns="http://www.w3.org/2000/svg">
        <path d="M11.7891 4.82031L7.66406 8.35938C7.3125 8.66406 6.75 8.40625 6.75 7.9375V5.89844C3.09375 5.94531 1.54688 6.83594 2.60156 10.2344C2.71875 10.6094 2.25 10.9141 1.94531 10.6797C0.914062 9.92969 0 8.5 0 7.07031C0 3.50781 2.97656 2.73438 6.75 2.6875V0.835938C6.75 0.34375 7.3125 0.0859375 7.66406 0.390625L11.7891 3.92969C12.0469 4.1875 12.0469 4.58594 11.7891 4.82031Z" fill="currentColor"></path>
      </svg>
      Share
    </button>
  </div>
</template>

<style scoped lang="scss">

</style>