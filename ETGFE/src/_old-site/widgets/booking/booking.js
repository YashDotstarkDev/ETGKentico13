Promise.all([
    import(/* webpackMode: "eager" */ './booking.scss'),
    import('../../plugins/jquery-ui/custom'),
    import('../../plugins/semantic/form.scss'),
    import('../../plugins/semantic/form.js'),
    import('../../plugins/semantic/dimmer.css'),
    import('../../plugins/semantic/dimmer.js'),
    import('../../plugins/semantic/modal.css'),
    import('../../plugins/semantic/modal.js'),
    import('../../plugins/semantic/checkbox.css'),
    import('../../plugins/semantic/checkbox.js'),
    import('../../plugins/semantic/popup.css'),
    import('../../plugins/semantic/popup.js'),
    import('../../plugins/semantic/dropdown.scss'),
    import('../../plugins/semantic/dropdown.js'),
    import('../../dbs/scripts/form/dbs.semantic.form.js')
]).then(() => {
    $('.widget.booking').each(function (i, el) {
        $(el).data('widget', new Booking(el))
        $(el).data('widget').init()
    })
})

function Booking(el) {

    const self = this
    self.el = $(el)

    self.commissionContainer = self.el.find('.item-commission .commission')
    self.originalCommissionDisplay = self.el.find('.item-commission .commission').text()
    self.netPrice = Number(self.el.find('.item-net-price .total-price').data('net'))

    self.paymentOptionsModal = self.el.find('.payment-options-modal').modal({
        closable: false
    })

    self.paymentOptionsModalSubmit = self.paymentOptionsModal.find('.button.proceed')
    self.paymentOptionsModalSubmit.click(function () {
        // Update the form endpoint, then trigger a form submit
        self.form.endpoint = self.form.el.attr('data-endpoint-pay-later')
        self.form.submitButton.click() // Using .click() so all other on click functions are fired, too.
    })

    self.paymentOptionsModalCancel = self.paymentOptionsModal.find('.button.cancel')
    console.log(self.paymentOptionsModalCancel)
    self.paymentOptionsModalCancel.click(function () {
        self.paymentOptionsModal.modal('hide')
    })

    self.bindForm = function () {
        console.log('bindForm')
        self.quotesuccessurl = self.el.attr('data-quote-successurl')
        self.form = new dbs.form.genericForm()
        self.form.init(self.el.find('.ui.form'))

        // Be default, on form validation fail, the page does not scroll to if there is a modal visible.
        // See `dbs.semantic.form.js`
        // So, we handle hiding the modal and scrolling to first error. This is required when the form is
        // submitted from the button in the modal.
        self.form.events.subscribe('Form:validation_fail', function () {
            self.paymentOptionsModal.modal('hide')

            setTimeout(function () {
                if (self.el.find('.field.error:first').length) {
                    $('html, body').animate({ scrollTop: self.el.find('.field.error:first').offset().top - 120 })
                }
            }, 100)

            // The payment options modal changes the form endpoint, so we want to reset the endpoint
            // in case user decides to submit form by 'pay by credit card' button.
            self.form.endpoint = self.form.el.attr('data-endpoint')
        })

        let submitOther = self.el.find('.submit-other')
        console.log('submitOther', submitOther)
        submitOther.click(function () {
            console.log('submitOther clicked')
            self.paymentOptionsModal.modal('show')
        })

        self.form.events.subscribe('Form:send', function (data) {
            console.log('Form:send', data)
        })

        self.form.events.subscribe('Form:send_success', function (data) {
            console.log('success', data)
            if (!data.response.success) {
                return
            }

            if (data.response.travelpay == undefined || data.response.travelpay == null) {
                if (data.response.redirecturl) {
                    window.location = data.response.redirecturl
                }
                else {
                    window.location = self.quotesuccessurl + '?qid=' + data.response.quoteid
                }
            } else {
                self.el.find('input[name="orderid"]').val(data.response.travelpay.orderId);
                setTimeout(() => {
                }, 1000);
                var payment = $.zpPayment({
                    url: data.response.travelpay.apiUrl,
                    apiKey: data.response.travelpay.apiKey,
                    fingerprint: data.response.travelpay.fingerprint,
                    merchantCode: data.response.travelpay.merchantCode,
                    redirectUrl: $('.booking').attr('data-payment-result-page'),
                    mode: 0,
                    customerName: data.response.travelpay.customerName,
                    customerReference: data.response.travelpay.orderId,
                    merchantUniquePaymentId: data.response.travelpay.orderId,
                    paymentAmount: data.response.travelpay.paymentAmount,
                    timeStamp: data.response.travelpay.timestamp,
                    title: 'Book Now',
                    hideTermsAndConditions: true,
                    customerEmail: data.response.travelpay.customerEmail,
                    allowBankAcOneOffPayment: false,
                    overrideFeePayer: 0,
                    sendConfirmationEmailToMerchant: true
                })

                payment.init();
            }
        })
    }


    self.init = function () {
        if (process.env.NODE_ENV === 'development') {
            console.log('Booking init', self)
        }

        self.el.css('opacity', 1)

        window.onpageshow = function (event) {
            if (event.persisted) {
                location = '/'
            }

        }


        self.el.find('.promo-form button').click(function (e) {
            e.preventDefault()

            self.el.find('.summary.promo-code').removeClass('invalid')

            // basic validation
            if (self.el.find('.promo-form input').val() === '') {
                self.el.find('.summary.promo-code .error').text('Please enter a promo code.')
                self.el.find('.summary.promo-code').addClass('invalid')
                return
            }

            $.ajax({
                url: self.el.find('.promo-form').attr('data-endpoint'),
                method: self.el.find('.promo-form').attr('data-method'),
                data: {
                    code: self.el.find('.promo-form input').val()
                },
                success: function (data) {
                    if (data.success) {
                        console.log('success')
                        window.location.reload()
                    } else {
                        self.el.find('.summary.promo-code').addClass('invalid')
                        self.el.find('.summary.promo-code .error').text(data.message)
                    }
                }
            })
        })


        if (self.el.hasClass('change-cookie')) {
            Cookies.set('CurrentCurrency', self.el.data('currency'), { path: '/', expires: 365 });
            location = "/change-currency"
        }
        self.el.find('.datepicker').datepicker({
            dateFormat: 'dd/mm/yy',
            changeMonth: true,
            changeYear: true,
            yearRange: '1920:' + new Date().getFullYear().toString()
        })

        try {
            if (window.performance.getEntriesByType('navigation')[0].type === 'back_forward') {
                location = '/'
            }

        } catch (err) {
            console.log(err)
        }


        $('.ui.tool-tip').each(function (i, el) {
            $(el).popup({
                on: 'click',

                position: 'top right'
            })

        })


        self.bindForm()

        self.el.find('.change-price').on('click', function (e) {
            e.preventDefault()

            self.el.find('.display-price').hide()
            self.el.find('.input-price').show()
            self.el.find('input[name=agentprice]').focus()
            $(this).hide()
            self.el.find('.cancel-price').show()
        })

        self.el.find('.cancel-price').on('click', function (e) {
            e.preventDefault()

            self.el.find('.display-price').show()

            self.el.find('input[name=agentprice]').val('')
            self.el.find('.input-price').hide()
            $(this).hide()
            self.el.find('.change-price').show()
            self.commissionContainer.text(self.originalCommissionDisplay)
        })

        $('body').on('click', '.confirm-agent-price .no', function (e) {
            e.preventDefault()
            self.el.find('confirm-agent-price').modal('hide')
        })

        self.el.find('.your-price input[name=agentprice]').on('keyup', function (e) {
            e.preventDefault()

            if (isNaN($(this).val())) {
                self.commissionContainer.text(self.originalCommissionDisplay)
                return
            }
            let newCommission = Number($(this).val()) - self.netPrice

            if (newCommission < 0) {
                newCommission = 0
            }

            self.commissionContainer.text(newCommission.toLocaleString('en-US'))
        })


        self.el.find('.ui.checkbox').checkbox()

        self.el.find('.make-dropdown').dropdown({
            placeholder: false

        })
    }
}


