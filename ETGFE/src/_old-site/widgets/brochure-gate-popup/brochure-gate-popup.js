import('../../plugins/jscookie/js.cookie.js').then(({ default: Cookies }) => {
  window.Cookies = Cookies
  Promise.all([
    import(/* webpackMode: "eager" */ './brochure-gate-popup.scss'),
    import('../../plugins/semantic/modal.css'),
    import('../../plugins/semantic/modal.js'),
    import('../../plugins/semantic/dimmer.css'),
    import('../../plugins/semantic/dimmer.js'),
    import('../../plugins/semantic/transition.css'),
    import('../../plugins/semantic/transition.js'),
    import('../../plugins/semantic/form.scss'),
    import('../../plugins/semantic/form.js'),
    import('../../dbs/scripts/form/dbs.semantic.form.js'),
  ]).then(() => {
    $('.widget.brochure-gate-popup').each(function (i, el) {
      $(el).data('widget', new BrochureGatePopup(el))
      $(el).data('widget').init()
    })
  })
})

function BrochureGatePopup (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log(window.dbs)
      console.log('BrochureGatePopup init', self);
    }
    self.el.css('opacity', 1);

    self.brochureGate = $('.brochure-gate-popup').modal();

    setTimeout(function(){
      self.gateForm = new dbs.form.genericForm();
      self.gateForm.init(self.el.find('.gate-form'));
      self.gateForm.events.subscribe('Form:validation_success', function (data) {
        var endpoint = self.el.find('.form').attr('data-endpoint-url')

        if (endpoint !== null && endpoint !== ""){

          $.ajax({
            url: endpoint,
            type: self.el.find('.form').attr('data-method'),
            data: JSON.stringify(data.values),
            dataType: 'json',
            contentType: 'application/json'
          })
          .done(function(response) {

            Cookies.set('brochureGate', 'true', {path: '/'});
           location = self.brochureUrl;
            //self.brochureGate.modal('hide');
          })
          .always(function(){
            // HIDE BLOCKER
          });

        }else
        {
          Cookies.set('brochureGate', 'true', {path: '/'});
          location = self.brochureUrl;
          self.brochureGate.modal('hide');
        }
      });
    }, 500);


    $('.brochure-gate-trigger').click(function(event) {
      self.brochureUrl = $(this).attr('href');

      if(!Cookies.get('brochureGate')) {
        event.preventDefault();
        self.brochureGate.modal('show');
      }
    });
  }
}


