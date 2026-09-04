Promise.all([
  import(/* webpackMode: "eager" */ './gdpr-consent.scss'),
  import('../../plugins/semantic/sidebar.scss'),
  import('../../plugins/semantic/sidebar.js'),
]).then(() => {
  $('.widget.gdpr-consent').each(function (i, el) {
    $(el).data('widget', new GdprConsent(el))
    $(el).data('widget').init()
  })
})

function GdprConsent (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('GdprConsent init', self);
    }
    self.el.css('opacity', 1);

    setTimeout(function(){
      // console.log('timeout');
      // self.el.sidebar({
      //   transition: 'push',
      //   mobileTransition: 'push',
      //   silent: true,
      //   dimPage: false,
      //   closable: false,
      //   onVisible: function () {
      //     $('html').addClass('gdpr-visible');
      //     $(window).trigger('resize').trigger('scroll');
      //   },
      //   onHide: function () {
      //     $('html').removeClass('gdpr-visible');
      //     $(window).trigger('resize').trigger('scroll');
      //   },
      //   onHidden: function () {
      //     $(window).trigger('resize').trigger('scroll');
      //
      //     setTimeout(function(){
      //       $(window).trigger('resize').trigger('scroll');
      //     }, 200);
      //   },
      //   onShow: function () {
      //     $(window).trigger('resize').trigger('scroll');
      //   }
      // });
      // self.el.sidebar('show');

      $('html').addClass('gdpr-visible');
    }, 1000);
/*
    self.el.find('.accept, .decline').click(function(event) {
      event.preventDefault();
      self.el.sidebar('hide');

    });*/
  }
}


