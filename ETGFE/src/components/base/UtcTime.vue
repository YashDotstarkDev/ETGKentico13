<script lang="ts">
import {defineComponent} from 'vue'
import moment from 'moment-timezone'

export default defineComponent({
  name: "UtcTime",
  methods: {
    UpdateUtcTime(el) {
      let utc = el.getAttribute('data-utc');
      if (utc.length) {
        let d = moment(utc);
        let format = el.getAttribute('data-format');
        if (!format) {
          format = 'DD/MM/YYYY';
        }
        let timezone = el.getAttribute('data-timezone') || moment.tz.guess();
        d = d.tz(timezone);
        el.setAttribute('title', 'Timezone: ' + timezone);
        el.innerHTML = d.format(format);
        el.classList.add('updated');
      }
    }
  },
  mounted() {
    document.querySelectorAll('.utc-time').forEach((el) => {
      if (el.classList.contains('updated')) {
        return;
      }
      this.UpdateUtcTime(el);
    });
  },
})
</script>

<template>
<div></div>
</template>

<style scoped lang="scss">

</style>
