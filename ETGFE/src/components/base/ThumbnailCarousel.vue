<script lang="ts">
import {defineComponent} from 'vue'
import Swiper from 'swiper';
import {FreeMode, Scrollbar, Navigation, Thumbs} from 'swiper/modules';
// import Swiper and modules styles
import 'swiper/css';
import 'swiper/css/free-mode';
import 'swiper/css/scrollbar';
import 'swiper/css/scrollbar';
import EventBus from "./EventBus.ts";

export default defineComponent({
  name: "ThumbnailCarousel",
  props: {
    thumbs: {
      type: Array,
      required: true,
      default () {
        return []
      }
    },
    galleryId: {
      type: String,
      required: true
    }
  },
  mounted() {
    const thumbCarousel = new Swiper('.thumb-carousel', {
      modules: [Navigation],
      loop: false,
      slidesPerView: 2,
      spaceBetween: 10,
      breakpoints: {
        768: {
          slidesPerView: 4,
          spaceBetween: 20
        },
        1280: {
          slidesPerView: 5,
          spaceBetween: 20
        }
      },
      navigation: {
        nextEl: this.$el.querySelector(".button-next"),
        prevEl: this.$el.querySelector(".button-prev")
      }
    });
  },
  methods: {
    zoomImage(id) {
      EventBus.emit("image-gallery", {galleryId: this.galleryId, imageId: id});
    },
  }
})
</script>

<template>
  <div class="thumbnail-carousel relative z-10">
    <div class="relative mx-[70px]">

      <div class="thumb-carousel overflow-hidden user-select-none">
        <div class="swiper-wrapper">
          <div class="swiper-slide aspect-square relative overflow-hidden rounded-lg group cursor-pointer" v-for="(image, index) in thumbs" :key="`gallery_thumb_${index}`" @click="zoomImage(image.id)">
            <img :data-src="image.url" class="lazyload absolute left-0 top-0 w-full h-full object-cover" />

            <div class="icon shadow-md bg-white absolute top-2 right-2 w-6 h-6 lg:w-7 lg:h-7 flex items-center justify-center rounded-md transition-opacity lg:group-hover:opacity-100 lg:opacity-0">
              <svg width="15" height="16" viewBox="0 0 15 16" fill="none" xmlns="http://www.w3.org/2000/svg" class="scale-75 lg:scale-100">
                <path d="M8.04871 2.47009L14.2132 0.92898L12.672 7.09343L8.04871 2.47009Z" fill="#10A4B1"/>
                <path d="M1.61694 8.88745L0.0710448 15.071L6.25464 13.5251L1.61694 8.88745Z" fill="#10A4B1"/>
                <rect x="4.56995" y="11.858" width="1.81866" height="9.09328" transform="rotate(-135 4.56995 11.858)" fill="#10A4B1"/>
              </svg>
            </div>

          </div>
        </div>
      </div>

      <button class="button-next text-teal hover:text-primary transition-colors duration-300 absolute right-[-70px] top-1/2 mt-[-25px]">
        <svg width="50" height="50" viewBox="0 0 50 50" fill="none" xmlns="http://www.w3.org/2000/svg">
          <rect width="50" height="50" rx="25" fill="currentColor"/>
          <path d="M33.6094 26.3984L27.3594 32.6484C26.8906 33.1562 26.0703 33.1562 25.6016 32.6484C25.0938 32.1797 25.0938 31.3594 25.6016 30.8906L29.7031 26.75H17.75C17.0469 26.75 16.5 26.2031 16.5 25.5C16.5 24.8359 17.0469 24.25 17.75 24.25H29.7031L25.6016 20.1484C25.0938 19.6797 25.0938 18.8594 25.6016 18.3906C26.0703 17.8828 26.8906 17.8828 27.3594 18.3906L33.6094 24.6406C34.1172 25.1094 34.1172 25.9297 33.6094 26.3984Z" fill="white"/>
        </svg>
      </button>

      <button class="button-prev text-teal hover:text-primary transition-colors duration-300 absolute left-[-70px] top-1/2 mt-[-25px]">
        <svg width="50" height="50" viewBox="0 0 50 50" fill="none" xmlns="http://www.w3.org/2000/svg">
          <rect x="50" y="50" width="50" height="50" rx="25" transform="rotate(-180 50 50)" fill="currentColor"/>
          <path d="M16.3906 23.6016L22.6406 17.3516C23.1094 16.8437 23.9297 16.8437 24.3984 17.3516C24.9063 17.8203 24.9063 18.6406 24.3984 19.1094L20.2969 23.25L32.25 23.25C32.9531 23.25 33.5 23.7969 33.5 24.5C33.5 25.1641 32.9531 25.75 32.25 25.75L20.2969 25.75L24.3984 29.8516C24.9063 30.3203 24.9063 31.1406 24.3984 31.6094C23.9297 32.1172 23.1094 32.1172 22.6406 31.6094L16.3906 25.3594C15.8828 24.8906 15.8828 24.0703 16.3906 23.6016Z" fill="white"/>
        </svg>
      </button>
    </div>

  </div>
</template>