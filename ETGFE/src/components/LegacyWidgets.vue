<script lang="ts">
import {defineComponent} from 'vue'

export default defineComponent({
  name: "LegacyWidgets",
  mounted() {
    console.log('LegacyWidgets mounted')

    // insert the runtime script
    console.log('inserting runtime script')
    const runtimeScript = document.createElement('script')
    runtimeScript.src = '/legacy/scripts/runtime.js'
    document.head.appendChild(runtimeScript)

    // for every data-widget
    console.log('inserting widget scripts')
    document.querySelectorAll('[data-widget]').forEach((el) => {
      const widgetName = el.getAttribute('data-widget')
      console.log('inserting script for widget:', widgetName)
      // insert script into head
      const script = document.createElement('script')
      script.src = `/legacy/scripts/${widgetName}.js`
      document.head.appendChild(script)
    })
  }
})
</script>

<template>
<div></div>
</template>

<style scoped lang="scss">

</style>