import('../../plugins/jscookie/js.cookie.js').then(({ default: Cookies }) => {
  window.Cookies = Cookies
  Promise.all([
    import(/* webpackMode: "eager" */ './game-of-luck.scss'),
    import('../../plugins/semantic/modal.css'),
    import('../../plugins/semantic/modal.js'),
    import('../../plugins/semantic/dimmer.css'),
    import('../../plugins/semantic/dimmer.js'),
  ]).then(() => {
    $('.widget.game-of-luck').each(function (i, el) {
      $(el).data('widget', new GameOfLuck(el))
      $(el).data('widget').init()
    })
  })
})


function GameOfLuck (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('GameOfLuck init', self);
    }
    self.el.css('opacity', 1);

    self.gameOfLuck = self.el.modal();

    self.el.find('.game-close').click(function(event) {
      self.gameOfLuck.modal('hide');
      Cookies.set('gameofluck', 'true', {path: '/'});
    });

    if(!Cookies.get('gameofluck')) {
      setTimeout(function () {
        self.gameOfLuck.modal({
          onHide: function(){
            Cookies.set('gameofluck', 'true', {path: '/'});
          },
        }).modal('show');
      }, 5000);
    }
  }
}


