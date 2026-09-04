Promise.all([
  import(/* webpackMode: "eager" */ './video.scss'),
  import('../../plugins/magnific-popup/jquery.magnific-popup.js'),
  import('../../plugins/magnific-popup/magnific-popup.css'),
]).then(() => {
  $('.widget.video').each(function (i, el) {
    $(el).data('widget', new Video(el))
    $(el).data('widget').init()
  })
})

function Video (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('Video init', self);
    }
    self.el.css('opacity', 1);

    self.el.find('.video-button').magnificPopup({
      type: 'iframe'
    });
  }
}


