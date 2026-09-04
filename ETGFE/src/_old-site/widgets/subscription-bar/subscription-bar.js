Promise.all([
  import(/* webpackMode: "eager" */ './subscription-bar.scss'),
  import('../../plugins/semantic/form.scss'),
  import('../../plugins/semantic/form.js'),
  import('../../plugins/semantic/checkbox.css'),
  import('../../plugins/semantic/checkbox.js'),
  import('../../dbs/scripts/form/dbs.semantic.form.js'),
]).then(() => {
  $('.widget.subscription-bar').each(function (i, el) {
    $(el).data('widget', new SubscriptionBar(el))
    $(el).data('widget').init()
  })
})

function SubscriptionBar (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('SubscriptionBar init', self);
    }
    self.el.css('opacity', 1);

    $(el).find('.ui.checkbox').checkbox();
  }
}

