import('../../plugins/jscookie/js.cookie.js').then(({ default: Cookies }) => {
  window.Cookies = Cookies
  Promise.all([
    import(/* webpackMode: "eager" */ './subscribe-to-win-modal.scss'),
    import('../../plugins/semantic/modal.css'),
    import('../../plugins/semantic/modal.js'),
    import('../../plugins/semantic/dimmer.css'),
    import('../../plugins/semantic/dimmer.js'),
  ]).then(() => {
    $('.widget.subscribe-to-win-modal').each(function (i, el) {
      $(el).data('widget', new SubscribeToWinModal(el))
      $(el).data('widget').init()
    })
  })
})

function SubscribeToWinModal (el) {
  const self = this;
  self.el = $(el);
  self.loaded = false;
  self.events = new dbs.events();
  self.setSessionUrl = self.el.attr('data-session-url');
  self.cookieName = self.el.attr('data-cookie') + 'stop'
  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('SubscribeToWin init', self);
    }
    self.el.css('opacity', 1);

    self.subscribeToWin = self.el.modal();

    window.isSearchInteracted = window.isSearchInteracted || false


    $(document).on('scroll', function(){
      self.handleScroll();
    });

    $('body').on('touchmove', function(){
      self.handleScroll();
    });

    if (self.el.find('.close-popup').length){
      self.el.find('.close-popup').on('click', function (e){
        self.subscribeToWin.modal('hide');
      });

    }

  }

  self.handleScroll = function(){

    if(!Cookies.get(self.cookieName) && !self.loaded) {
      if ($(document).scrollTop() > 400 && !window.isSearchInteracted && !window.isNzRedirectModalOpen){
        setTimeout(function () {
          self.subscribeToWin.modal({
            onHide: function(){
              if ($('input[name="nevershow"]').is(':checked')){
                Cookies.set(self.cookieName, 'true', {path: '/', expires:365});
              }
            }
          }).modal('show');

          self.loaded = true;
          $.ajax({
            url: self.setSessionUrl,
            contentType: 'application/json'
          })
            .done(function(response) {
            })

        }, 1500);
      }
    }
  }
}


