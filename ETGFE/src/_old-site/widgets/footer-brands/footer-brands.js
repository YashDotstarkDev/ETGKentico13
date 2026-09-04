import('../../plugins/swiper/swiper.js').then(({ default: Swiper }) => {
  Promise.all([
    import('../../plugins/swiper/swiper.css'),
    import(/* webpackMode: "eager" */ './footer-brands.scss')
  ]).then(() => {
    $('.widget.footer-brands').each(function (i, el) {
      $(el).data('widget', new FooterBrands(el, Swiper))
      $(el).data('widget').init()
    })
  })
})

function FooterBrands (el, Swiper) {
  const self = this
  self.el = $(el)

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('FooterBrands init', self)
    }
    self.el.css('opacity', 1)

    // var owl = self.el.find('.brands');
    // var slides = self.el.find('img').length;
    // owl.owlCarousel({
    //   items: slides,
    //   loop: true,
    //   margin: 20,
    //   autoplay: true,
    //   slideTransition: 'linear',
    //   autoplayTimeout: 0,
    //   autoplaySpeed: 6000,
    //   autoplayHoverPause: false
    // });

    setTimeout(function () {
      self.swiper = new Swiper(self.el.find('.brands .swiper-container'), {
        autoplay: {
          delay: 2000,
          disableOnInteraction: false
        },
        speed: 1500,
        loop: true,
        slidesPerView: 'auto',
        spaceBetween: 0
      })
    }, 1000)

  }

}


