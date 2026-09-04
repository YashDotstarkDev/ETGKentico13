<script lang="ts">
import {defineComponent} from 'vue'
import LoadingSpinner from "./LoadingSpinner.vue";

export default defineComponent({
  name: "PackageTileThumbnail",
  components: {LoadingSpinner},
  props: {
    images: {
      type: Array,
      required: true,
      default: () => []
    }
  },
  data () {
    return {
      currentImageIndex: 0,
      loading: false,
      imageContainer: null
    }
  },
  methods: {
    loadFirstImage () {
      const img = document.createElement('img')
      img.setAttribute('data-src', this.images[0] + '?auto=format&w={width}')
      img.setAttribute('data-lowsrc', this.images[0] + '?auto=format&w=3')
      img.setAttribute('data-sizes', 'auto')
      img.classList.add('image0')
      img.classList.add('lazyload', 'absolute', 'z-0', 'inset-0', 'w-full', 'h-full', 'object-cover')
      this.imageContainer.appendChild(img)
    },
    createHighImage (index) {
      // add the high quality image
      const highImg = document.createElement('img')
      highImg.setAttribute('src', this.images[index] + '?auto=format&w=700')
      highImg.classList.add('image' + index)
      highImg.classList.add('absolute', 'z-10', 'inset-0', 'w-full', 'h-full', 'object-cover', 'opacity-0', 'transition-opacity', 'duration-500')
      return highImg
    },
    loadNextImage () {
      if (this.loading) return
      this.loading = true
      this.currentImageIndex = (this.currentImageIndex + 1) % this.images.length

      let highImg = this.createHighImage(this.currentImageIndex)
      this.imageContainer.appendChild(highImg)

      highImg.onload = () => {
        console.log('high image loaded')
        this.loading = false
        highImg.classList.remove('opacity-0')

        // previous index
        let previousIndex = this.currentImageIndex - 1
        if (previousIndex < 0) {
          previousIndex = this.images.length - 1
        }

        // remove all previous images
        let previousImages = this.imageContainer.querySelectorAll('.image' + previousIndex)

        setTimeout(() => {
          previousImages.forEach((img) => {
            img.remove()
          })
        }, 500)
      }
    },
    loadPreviousImage () {
      if (this.loading) return
      this.loading = true
      this.currentImageIndex = (this.currentImageIndex - 1 + this.images.length) % this.images.length

      let highImg = this.createHighImage(this.currentImageIndex)
      this.imageContainer.appendChild(highImg)

      highImg.onload = () => {
        console.log('high image loaded')
        this.loading = false
        highImg.classList.remove('opacity-0')

        // next index
        let nextIndex = (this.currentImageIndex + 1) % this.images.length

        // remove all next images
        let nextImages = this.imageContainer.querySelectorAll('.image' + nextIndex)
        setTimeout(() => {
          nextImages.forEach((img) => {
            img.remove()
          })
        }, 500)
      }
    }

  },
  mounted() {
    this.imageContainer = this.$el.querySelector('.images')
    this.loadFirstImage()
  }
})
</script>

<template>
  <div class="package-tile-thumbnail absolute inset-0 group">
    <div class="images absolute inset-0 z-10 transition-all duration-300"></div>
    <button @click="loadPreviousImage" v-if="images.length > 1" type="button" class="rounded-full w-10 h-10 text-primary bg-white absolute top-1/2 -translate-y-1/2 z-20 left-0 opacity-0 pointer-events-none group-hover:left-4 group-hover:opacity-100 group-hover:pointer-events-auto transition-all duration-300 hover:bg-body hover:text-white">
      <i class="fas fa-chevron-left"></i>
    </button>
    <button @click="loadNextImage" v-if="images.length > 1" type="button" class="rounded-full w-10 h-10 text-primary bg-white absolute top-1/2 -translate-y-1/2 z-20 right-0 opacity-0 pointer-events-none group-hover:right-4 group-hover:opacity-100 group-hover:pointer-events-auto transition-all duration-300 hover:bg-body hover:text-white">
      <i class="fas fa-chevron-right"></i>
    </button>
    <div class="loading absolute inset-0 z-30 flex items-center justify-center pointer-events-none bg-black/30 transition-all duration-500" :class="{'opacity-0': !loading, 'opacity-100 delay-200': loading}">
      <LoadingSpinner />
    </div>
  </div>
</template>

<style scoped lang="scss">

</style>