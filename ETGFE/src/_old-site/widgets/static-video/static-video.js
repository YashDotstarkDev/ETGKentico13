import(/* webpackMode: "eager" */ './static-video.scss');

function StaticVideo (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('StaticVideo init', self);
    }
    self.el.css('opacity', 1);

    self.el.find('.video-button').click(function(event) {
      event.preventDefault();

      $(this).parents('.video-container').addClass('play-video');
      self.el.find('iframe')[0].src += "?autoplay=1";
    });
  }
}

$('.widget.static-video').each(function(i, el){
  $(el).data('widget', new StaticVideo(el));
  $(el).data('widget').init();
});
