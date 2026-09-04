Promise.all([
  import(/* webpackMode: "eager" */ './contact-us.scss'),
  import('../../plugins/semantic/form.scss'),
  import('../../plugins/semantic/form.js'),
  import('../../plugins/semantic/checkbox.css'),
  import('../../plugins/semantic/checkbox.js'),
  import('../../plugins/semantic/dropdown.scss'),
  import('../../plugins/semantic/dropdown.js'),
  import('../../dbs/scripts/form/dbs.semantic.form.js'),
]).then(() => {
  $('.widget.contact-us').each(function (i, el) {
    $(el).data('widget', new ContactUs(el))
    $(el).data('widget').init()
  })
})

function ContactUs (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('ContactUs init', self);
    }
    self.el.css('opacity', 1);
  }

  self.el.find('.ui.checkbox').checkbox();
}


