<script lang="ts">
import {defineComponent} from 'vue'
import LoadingSpinner from "./LoadingSpinner.vue";

import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css'

export default defineComponent({
  name: "HomepageHeroSearch",
  components: {LoadingSpinner, VueDatePicker},
  props: {
    appId: {
      type: String,
      required: true
    },
    apiKey: {
      type: String,
      required: true
    },
    indexName: {
      type: String,
      required: true
    },
    searchUrl: {
      type: String,
      required: true
    }
  },
  data () {
    return {
      ready: false,
      overlayActive: false,
      hideOverflow: true,
      departureDate: null,
      destinations: [],
      tours: [],
      filters: {
        destination: '',
        query: '',
        departs: '',
        starts: '',
        ends: ''
      }
    }
  },
  methods: {
    getTours (filters) {
      fetch(`https://${this.appId}.algolia.net/1/indexes/${this.indexName}/query`, {
        method: 'POST',
        headers: {
          'X-Algolia-API-Key': this.apiKey,
          'X-Algolia-Application-Id': this.appId
        },
        body: JSON.stringify({
          attributesToRetrieve: 'code,departureDate,destination,duration,startCity,endCity,name',
          hitsPerPage: 1000,
          filters: filters
        })
      })
        .then(res => res.json())
        .then(data => {
          this.tours = data.hits
          this.handleTours(data.hits)

          setTimeout(() => {
            this.ready = true
          }, 500)
          setTimeout(() => {
            this.hideOverflow = false
          }, 1000)
        })
    },
    handleTours (tours) {

      // for each tour
      tours.forEach(tour => {
        // get distinct destination
        if (!this.destinations.includes(tour.destination) && tour.destination.length > 0) {
          this.destinations.push(tour.destination)
        }
      })

      // sort destinations
      this.destinations.sort()
    },
    hideOverlay () {
      this.overlayActive = false
      document.body.classList.remove('overlay-active')
    },
    showOverlay () {
      this.overlayActive = true
      document.body.classList.add('overlay-active')
    }
  },
  mounted() {
    this.getTours()
  },
  computed: {
    filterUrl () {

      let url = this.searchUrl

      // destination
      if (this.filters.destination && this.filters.destination.length > 0) {
        url += `&TourPricing-Live[refinementList][destination][0]=${this.filters.destination}`
      }

      // query
      if (this.filters.query && this.filters.query.length > 0) {
        url += `&TourPricing-Live[query]=${this.filters.query}`
      }

      // departs
      if (this.filters.departs && this.filters.departs.length > 0) {
        // create epoch timestamp for every day in range
        let start = new Date(this.filters.departs).getTime()
        // let end = new Date(this.filters.departs[1]).getTime()

        // convert start to gmt
        // start += new Date().getTimezoneOffset() * 60000
        // end += new Date().getTimezoneOffset() * 60000

        // convert start to gmt +10
        start -= 60000 * 60 * 10
        // end -= 60000 * 60 * 10

        // minus three days from start
        start = start - (86400000 * 3)

        // const days = (end - start) / 86400000
        const days = 7

        for (let i = 0; i <= days; i++) {
          let date = start
          date += (i * 86400000)
          url += `&TourPricing-Live[refinementList][departureDate][${i}]=${date / 1000}`
        }
      }

      // startCity
      // if (this.filters.starts && this.filters.starts.length > 0) {
      //   url += `&TourPricing-Live[refinementList][startCity][0]=${this.filters.starts.split('|')[0]}`
      //   url += `&TourPricing-Live[refinementList][endCity][0]=${this.filters.starts.split('|')[1]}`
      // }

      // startCity
      if (this.filters.starts && this.filters.starts.length > 0) {
        url += `&TourPricing-Live[refinementList][startCity][0]=${this.filters.starts}`
      }

      // endCity
      if (this.filters.ends && this.filters.ends.length > 0) {
        url += `&TourPricing-Live[refinementList][endCity][0]=${this.filters.ends}`
      }

      return url
    },
    toursForDestination () {
      if (!this.filters.destination || this.filters.destination.length === 0) {
        return this.tours
      }
      return this.tours.filter(tour => tour.destination === this.filters.destination)
    },
    departuresForDestination () {
      let departures = []

      this.toursForDestination.forEach(tour => {
        if (tour.departureDate && tour.departureDate.length > 0) {
          tour.departureDate.forEach(departure => {
            if (!departures.includes(departure)) {
              departures.push(departure)
            }
          })
        }
      })

      departures = departures.sort()

      return departures.map(departure => {
        return new Date(departure * 1000)
      })
    },
    startsForDestination () {
      let starts = []

      this.toursForDestination.forEach(tour => {
        if (!starts.includes(`${tour.startCity}|${tour.endCity}`)) {
          starts.push(`${tour.startCity}|${tour.endCity}`)
        }
      })

      return starts.sort()
    },
    startCitiesForDestination () {
      let starts = []

      this.toursForDestination.forEach(tour => {
        if (!starts.includes(`${tour.startCity}`) && tour.startCity.length) {
          starts.push(`${tour.startCity}`)
        }
      })

      return starts.sort()
    },
    toursForStartCity () {
      if (!this.filters.starts || this.filters.starts.length === 0) {
        return this.toursForDestination
      }
      return this.toursForDestination.filter(tour => tour.startCity === this.filters.starts)
    },
    endCitiesForDestination () {
      let ends = []

      this.toursForStartCity.forEach(tour => {
        if (!ends.includes(`${tour.endCity}`) && tour.endCity.length) {
          ends.push(`${tour.endCity}`)
        }
      })

      return ends.sort()
    }
  }
})
</script>

<template>
  <div class="homepage-hero-search w-full text-[16px]">
    <div class="max-w-xl lg:max-w-6xl w-full px-6 mx-auto flex flex-col items-center justify-center">

      <div class="bg-white rounded-2xl h-20 relative transition-all duration-500 ease-out w-full">
<!--      <div class="bg-white rounded-2xl h-20 relative transition-all duration-500 ease-out" :class="{'w-20': !ready, 'w-full': ready}">-->

<!--        <div class="absolute inset-0 flex items-center justify-center transition-all duration-500" :class="{'opacity-50': !ready, 'opacity-0 pointer-events-none': ready}">-->
<!--          <LoadingSpinner colour="#ca568e" />-->
<!--        </div>-->

        <div class="absolute inset-0 rounded-2xl transition-all duration-500 opacity-100 pointer-events-auto">
<!--        <div class="absolute inset-0 rounded-2xl transition-all duration-500" :class="{'opacity-0 pointer-events-none': !ready, 'opacity-100 pointer-events-auto': ready, 'overflow-hidden': hideOverflow}">-->

          <button class="w-10 h-10 flex items-center justify-center fixed right-0 top-0 text-white z-[2001] text-xl" @click="hideOverlay" v-show="overlayActive">
            <i class="fas fa-times"></i>
          </button>
          <button class="absolute inset-0 px-6 flex flex-col justify-center cursor-pointer whitespace-nowrap lg:hidden" @click="showOverlay">
            <span class="uppercase font-bold text-black">Where</span>
            <span class="text-black/50">Where would you like to go</span>
          </button>
          <div class="overlay fixed inset-0 z-[2000] lg:z-[500] flex flex-col justify-start lg:flex-row gap-8 px-6 md:px-48 lg:px-6 lg:pr-8 pb-6 pt-20 lg:py-0 lg:items-center bg-black/90 transition-opacity duration-300 ease-out lg:absolute lg:bg-transparent" :class="{'opacity-0 pointer-events-none lg:opacity-100 lg:pointer-events-auto': !overlayActive, 'opacity-100 pointer-events-auto': overlayActive}">
            <div class="col lg:flex-1 bg-white p-4 rounded-lg lg:p-0">
              <div class="label text-black uppercase font-bold text-[14px] whitespace-nowrap">Where</div>
              <div class="input">
                <select class="border-b rounded-none border-black/10 appearance-none py-1 px-1 !outline-none w-full" v-model="filters.destination" @change="filters.starts = ''; filters.departs = ''">
                  <option value="">Select a destination</option>
                  <option v-for="destination in destinations" :value="destination" :key="destination">{{ destination }}</option>
                </select>
              </div>
            </div>

            <div class="col lg:flex-1 bg-white p-4 rounded-lg lg:p-0">
              <div class="label text-black uppercase font-bold text-[14px] whitespace-nowrap">Keyword or Tour Code</div>
              <div class="input">
                <input type="text" class="border-b rounded-none border-black/10 appearance-none py-1 !outline-none w-full" v-model="filters.query">
              </div>
            </div>

            <div class="col lg:flex-1 bg-white p-4 rounded-lg lg:p-0">
              <div class="label text-black uppercase font-bold text-[14px] whitespace-nowrap">Departs</div>
              <VueDatePicker auto-apply :enable-time-picker="false" format="dd/MM/yy" hide-input-icon v-model="filters.departs" model-type="yyyy-MM-dd" :allowed-dates="departuresForDestination" :clearable="false">
                <template #dp-input="{ value }">
                  <input type="text" class="border-b rounded-none border-black/10 appearance-none py-1 !outline-none w-full pr-4 !placeholder-body !placeholder-opacity-100" :value="value" placeholder="Select" />
                </template>
              </VueDatePicker>
            </div>

<!--            <div class="col hidden lg:block lg:flex-1 bg-white p-4 rounded-lg lg:p-0">-->
<!--              <div class="label text-black uppercase font-bold text-[14px] whitespace-nowrap">Departs</div>-->
<!--              <VueDatePicker auto-apply :enable-time-picker="false" format="dd/MM/yy" hide-input-icon v-model="filters.departs" model-type="yyyy-MM-dd" :allowed-dates="departuresForDestination" :clearable="false">-->
<!--                <template #dp-input="{ value }">-->
<!--                  <input type="text" class="border-b rounded-none border-black/10 appearance-none py-1 !outline-none w-full pr-4 !placeholder-body !placeholder-opacity-100" :value="value" placeholder="Select" />-->
<!--                </template>-->
<!--              </VueDatePicker>-->
<!--            </div>-->

<!--            <div class="col lg:flex-1 bg-white p-4 rounded-lg lg:p-0">-->
<!--              <div class="label text-black uppercase font-bold text-[14px] whitespace-nowrap">Start / End City</div>-->
<!--              <div class="input">-->
<!--                <select class="border-b rounded-none border-black/10 appearance-none py-1 px-1 !outline-none w-full" v-model="filters.starts">-->
<!--                  <option value="">Select</option>-->
<!--                  <option v-for="start in startsForDestination" :value="start" :key="start">{{ start.split('|')[0] + ' / ' + start.split('|')[1] }}</option>-->
<!--                </select>-->
<!--              </div>-->
<!--            </div>-->

            <div class="col lg:flex-1 bg-white p-4 rounded-lg lg:p-0">
              <div class="label text-black uppercase font-bold text-[14px] whitespace-nowrap">Start City</div>
              <div class="input">
                <select class="border-b rounded-none border-black/10 appearance-none py-1 px-1 !outline-none w-full" v-model="filters.starts">
                  <option value="">Select</option>
                  <option v-for="start in startCitiesForDestination" :value="start" :key="start">{{ start }}</option>
                </select>
              </div>
            </div>

<!--            <div class="col lg:flex-1 bg-white p-4 rounded-lg lg:p-0">-->
<!--              <div class="label text-black uppercase font-bold text-[14px] whitespace-nowrap">End City</div>-->
<!--              <div class="input">-->
<!--                <select class="border-b rounded-none border-black/10 appearance-none py-1 px-1 !outline-none w-full" v-model="filters.ends">-->
<!--                  <option value="">Select</option>-->
<!--                  <option v-for="start in endCitiesForDestination" :value="start" :key="start">{{ start }}</option>-->
<!--                </select>-->
<!--              </div>-->
<!--            </div>-->


            <div class="col lg:hidden">
              <a :href="filterUrl" class="btn btn-primary w-full">
                Search
              </a>
            </div>

            <div class="col w-[40px] shrink-0 hidden lg:block">
              <a :href="filterUrl" class="rounded-full w-10 h-10 bg-primary flex items-center justify-center ml-3 hover:no-underline hover:bg-body">
                <svg width="16" height="16" viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg">
                  <path d="M14.6717 13.4907L11.3904 10.2095C12.1287 9.11572 12.5115 7.74854 12.3201 6.27197C11.9646 3.75635 9.91387 1.73291 7.42559 1.40479C3.70684 0.939941 0.562305 4.08447 1.02715 7.80322C1.35527 10.2915 3.37871 12.3423 5.89434 12.6978C7.3709 12.8892 8.73809 12.5063 9.85918 11.7681L13.1131 15.0493C13.5506 15.4595 14.2342 15.4595 14.6717 15.0493C15.0818 14.6118 15.0818 13.9282 14.6717 13.4907ZM3.15996 7.0376C3.15996 5.12354 4.71856 3.5376 6.65996 3.5376C8.57402 3.5376 10.16 5.12354 10.16 7.0376C10.16 8.979 8.57402 10.5376 6.65996 10.5376C4.71856 10.5376 3.15996 8.979 3.15996 7.0376Z" fill="white"></path>
                </svg>
              </a>
            </div>


          </div>
        </div>
      </div>

      <div class="cta mt-4 relative z-30 transition-all duration-500 ease-out text-white opacity-100 pointer-events-auto">
<!--      <div class="cta mt-4 relative z-30 transition-all duration-500 ease-out text-white" :class="{'opacity-0 pointer-events-none': !ready, 'opacity-100 pointer-events-auto': ready}">-->

        <a :href="searchUrl" class="text-white uppercase">Advanced Search</a>
      </div>
    </div>

  </div>
</template>

<style lang="scss">
:root {
  /*General*/
  --dp-font-family: "Lato", sans-serif;
  --dp-border-radius: 0; /*Configurable border-radius*/

  /*Sizing*/
  --dp-input-padding: 0; /*Padding in the input*/

  /*Font sizes*/
  --dp-font-size: 16px; /*Default font-size*/
}
</style>