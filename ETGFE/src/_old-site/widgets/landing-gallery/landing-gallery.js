import { dom } from '@fortawesome/fontawesome-svg-core'

Promise.all([
  import(/* webpackMode: "eager" */ './landing-gallery.scss'),
  import('../../plugins/semantic/modal.css'),
  import('../../plugins/semantic/modal.js'),
  import('../../plugins/semantic/dimmer.css'),
  import('../../plugins/semantic/dimmer.js'),
  import('../../plugins/slick/slick.css'),
  import('../../plugins/slick/slick.js'),
  import('../../plugins/semantic/transition.css'),
  import('../../plugins/semantic/transition.js'),
  import('../../plugins/slick/slick-theme.css'),
]).then(() => {
  $('.widget.landing-gallery').each(function (i, el) {
    $(el).data('widget', new LandingGallery(el))
    $(el).data('widget').init()
  })
})

function LandingGallery (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('LandingGallery init', self);
    }
    self.el.css('opacity', 1);

    self.galleryModal = self.el.find('.gallery-modal').modal();

    self.el.find('.gallery-items .item').click(function(event) {
      event.preventDefault();

      self.galleryModal.modal('show');

      if (!$('.gallery-modal .images').hasClass('slick-initialized')){
        $('.gallery-modal .images').slick({
          dots: true,
          infinite: false,
          speed: 300,
          slidesToShow: 1,
          slidesToScroll: 1,
          prevArrow: '<div class="prev"><span class="icon fas fa-chevron-left"></span></div>',
          nextArrow: '<div class="next"><span class="icon fas fa-chevron-right"></span></div>'
        });

        dom.i2svg();

      }

      $('.gallery-modal .images').slick('slickGoTo', $(this).index());

    });



  }
}


