Promise.all([
    import(/* webpackMode: "eager" */ './pay-now.scss'),
    import('../../plugins/semantic/form.scss'),
    import('../../plugins/semantic/form.js'),
    import('../../plugins/semantic/checkbox.css'),
    import('../../plugins/semantic/checkbox.js'),
    import('../../plugins/semantic/popup.css'),
    import('../../plugins/semantic/popup.js'),
    import('../../dbs/scripts/form/dbs.semantic.form.js'),
]).then(() => {
    $('.widget.pay-now').each(function (i, el) {
        $(el).data('widget', new PayNow(el))
        $(el).data('widget').init()
    })
})

function PayNow(el) {
    const self = this;
    self.el = $(el);

    self.init = function () {
        if (process.env.NODE_ENV === 'development') {
            console.log('PayNow init', self);
        }
        self.el.css('opacity', 1);

        self.el.find('.ui.checkbox').checkbox();

        self.el.find('.ui.tool-tip').each(function (i, el) {
            $(el).popup({
                on: 'click',
                position: 'top right'
            })
        });

        self.form = new dbs.form.genericForm();
        self.form.init(self.el.find('.ui.form'));
        self.form.events.subscribe('Form:send', function (data) {
            console.log('Form:send', data);
        })
        self.form.events.subscribe('Form:send_success', function (data) {

            console.log('success', data);
            if (!data.response.success) {
                $('.loading').removeClass('loading')
                return
            }
            self.el.find('input[name="paymentid"]').val(data.response.travelpay.orderId);
            setTimeout(() => {
            }, 1000);
            var payment = $.zpPayment({
                url: data.response.travelpay.apiUrl,
                apiKey: data.response.travelpay.apiKey,
                fingerprint: data.response.travelpay.fingerprint,
                merchantCode: data.response.travelpay.merchantCode,
                redirectUrl: self.el.find('.ui.form').attr('data-payment-result-page'),
                mode: 0,
                customerName: data.response.travelpay.customerName,
                customerReference: data.response.travelpay.customerReference,
                merchantUniquePaymentId: data.response.travelpay.customerReference,
                paymentAmount: data.response.travelpay.paymentAmount,
                timeStamp: data.response.travelpay.timestamp,
                title: 'Pay Now',
                hideTermsAndConditions: true,
                customerEmail: data.response.travelpay.customerEmail,
                allowBankAcOneOffPayment: false,
                overrideFeePayer: 0,
                sendConfirmationEmailToMerchant: true
            });
            payment.init();
        });
    }

}
