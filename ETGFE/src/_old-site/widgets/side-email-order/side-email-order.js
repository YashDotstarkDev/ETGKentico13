Promise.all([
  import(/* webpackMode: "eager" */ './side-email-order.scss'),
  import('../../dbs/scripts/form/dbs.semantic.form.js'),
  import('../../plugins/semantic/sidebar.js'),
]).then(() => {
  $('.widget.side-email-order').each(function (i, el) {
    $(el).data('widget', new SideEmailOrder(el))
    $(el).data('widget').init()
  })
})

function SideEmailOrder (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('SideEmailOrder init', self);
    }
    self.el.css('opacity', 1);

    self.el.sidebar({
      context: $('form'),
      transition: 'push',
      mobileTransition: 'push',
      silent: true,
      dimPage: false,
      closable: false,
      onVisible: function () {
        $('html').addClass('gdpr-visible');
        $(window).trigger('resize').trigger('scroll');
      },
      onHide: function () {
        $('html').removeClass('gdpr-visible');
        $(window).trigger('resize').trigger('scroll');
      },
      onHidden: function () {
        $(window).trigger('resize').trigger('scroll');

        setTimeout(function(){
          $(window).trigger('resize').trigger('scroll');
        }, 200);
      },
      onShow: function () {
        $(window).trigger('resize').trigger('scroll');
      }
    });
  }
}


