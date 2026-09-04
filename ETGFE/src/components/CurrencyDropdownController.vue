<script lang="ts">
import {defineComponent} from 'vue'

export default defineComponent({
  name: "CurrencyDropdownController",
  mounted() {
    document.querySelectorAll('.currency-dropdown a').forEach(function (element) {
      element.addEventListener('click', function (event) {
        event.preventDefault(); // Prevent default anchor behavior
        document.cookie = "CurrentCurrency=" + this.dataset.currency + ";path=/;max-age=" + (365 * 24 * 60 * 60);
        window.location.href = "/change-currency";
      });
    });
  }
})
</script>

<template>
<div></div>
</template>

<style scoped lang="scss">

</style>