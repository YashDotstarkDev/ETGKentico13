<script lang="ts">
import {defineComponent} from 'vue'
import {useVuelidate} from '@vuelidate/core'
import {required, email} from '@vuelidate/validators'

/*

{
    "objectSet": {
        "actions": [
            {
                "type": "message",
                "uri": null,
                "content": "<p>Thank you for subscribing to Entire Travel Group.</p>\r\n\r\n<p>We have just sent an email to the address you provided. If you do not receive it, check your junk email folder, and if the email arrives there please make sure you mark our email as a safe sender.</p>\r\n\r\n<p>The Team at Entire Travel Group</p>\r\n"
            }
        ]
    },
    "success": true,
    "message": null
}


 */

export default defineComponent({
  setup() {
    return {v$: useVuelidate()}
  },
  name: "SubscribeToWinForm",
  props: {
    endpoint: {
      type: String,
      default: null
    }
  },
  data() {
    return {
      loading: false,
      successMessage: null,
      form: {
        firstname: '',
        lastname: '',
        email: '',
        isAgent: '',
      }
    }
  },
  validations() {
    return {
      form: {
        firstname: {required},
        lastname: {required},
        email: {required, email},
        isAgent: {required}
      }
    }
  },
  methods: {
    async getRecaptchaToken() {
      try {
        const token = await this.$recaptcha('login')
        if (!token) {
          console.error('There was no Google reCAPTCHA token returned.')
        }
        return token
      } catch (err) {
        console.error(err)
        return ''
      }
    },
    async doSubmitForm() {
      if (this.loading) return
      const self = this
      const isFormCorrect = await this.v$.$validate()
      if (!isFormCorrect) return

      this.loading = true

      // const recaptchaToken = await this.getRecaptchaToken()
      // const data = {
      //   'g_recaptcha_response': recaptchaToken,
      //   ...self.form
      // }

      const data = self.form

      fetch(self.endpoint, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(data),
      }).then((res) => {
        return res.json();
      }).then((data) => {

        if (data.success) {
          self.successMessage = data.message
        }
      }).finally(() => {
        self.loading = false
      })
    }
  },
})
</script>

<template>
  <div class="form mx-auto max-w-5xl">
    <div v-if="successMessage" class="text-white text-lg text-center">
      {{ successMessage }}
    </div>
    <form v-if="!successMessage" @submit.prevent="doSubmitForm" class="md:-mx-3 md:flex md:flex-wrap md:justify-center">
      <div class="field mt-6 md:w-1/2 md:px-3">
        <label class="text-white">First name</label>
        <div class="control mt-2">
          <input class="form-item" type="text" v-model="form.firstname">
        </div>
        <div v-if="v$.form.firstname.$errors.length" class="text-white text-sm mt-2">Please enter your first name</div>
      </div>
      <div class="field mt-6 md:w-1/2 md:px-3">
        <label class="text-white">Last name</label>
        <div class="control mt-2">
          <input class="form-item" type="text" v-model="form.lastname">
        </div>
        <div v-if="v$.form.lastname.$errors.length" class="text-white text-sm mt-2">Please enter your last name</div>
      </div>
      <div class="field mt-6 md:w-1/2 md:px-3">
        <label class="text-white">Email</label>
        <div class="control mt-2">
          <input class="form-item" type="email" v-model="form.email">
        </div>
        <div v-if="v$.form.email.$errors.length" class="text-white text-sm mt-2">Please enter a valid email</div>
      </div>
      <div class="field mt-6 md:w-1/2 md:px-3">
        <label class="text-white">Are you a travel agent?</label>
        <div class="control mt-2">
          <select class="form-item" v-model="form.isagent">
            <option value="">Please select</option>
            <option value="yes">Yes</option>
            <option value="no">No</option>
          </select>
        </div>
        <div v-if="v$.form.isAgent.$errors.length" class="text-white text-sm mt-2">Please select</div>
      </div>

<!--      <div class="google-notice mt-8 md:px-3 text-xs text-red-200">-->
<!--        This site is protected by reCAPTCHA and the Google <a href="https://policies.google.com/privacy" target="_blank" class="text-red-200 underline underline-offset-2 hover:text-red-100">Privacy Policy</a> and <a href="https://policies.google.com/terms" target="_blank"  class="text-red-200 underline underline-offset-2 hover:text-red-100">Terms of Service</a> apply.-->
<!--      </div>-->

      <div class="field mt-8 flex justify-center w-full">
        <button type="submit"
                :class="{'disabled opacity-50 pointer-events-none': loading}"
                class="btn btn-primary">
          Subscribe
        </button>
      </div>
    </form>
  </div>
</template>

<style scoped lang="scss">

</style>