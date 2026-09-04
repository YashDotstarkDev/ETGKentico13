import('../../plugins/swiper/swiper.js').then(({ default: Swiper }) => {
  Promise.all([
    import('../../plugins/swiper/swiper.css'),
    import(/* webpackMode: "eager" */ './homepage-hero.scss')
  ]).then(() => {
    $('.widget.homepage-hero').each(function (i, el) {
      $(el).data('widget', new HomepageHero(el, Swiper))
      $(el).data('widget').init()
    })
  })
})

function HomepageHero (el, Swiper) {
  const self = this
  self.el = $(el)

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('HomepageHero init', self)
    }
    self.el.css('opacity', 1)

    self.swiper = new Swiper(self.el.find('.bg .swiper-container'), {
      autoplay: {
        delay: 3000,
        disableOnInteraction: false
      },
      effect: 'fade',
      speed: 1500,
      loop: true,
      spaceBetween: 0
    })

    self.el.find('.arrow').click(function (event) {
      event.preventDefault()

      $('html, body').animate({
        scrollTop: $(this).parents('.homepage-hero').find('.bottom').offset().top - 80
      }, 1000)
    })

    var slideWidth = 0
    var windowWidth = $(window).outerWidth() - 260

    self.el.find('.experience-list .swiper-slide').each(function (i, el) {
      slideWidth += $(el).outerWidth()
    })

    self.el.find('.list').css('width', slideWidth)

    if (slideWidth > windowWidth) {
      setTimeout(function () {
        self.swiperList = new Swiper(self.el.find('.experience-list .swiper-container'), {
          autoplay: {
            delay: 2000,
            disableOnInteraction: false
          },
          speed: 1500,
          loop: true,
          slidesPerView: 'auto',
          spaceBetween: 0,
          on: {
            init: function () {

              console.log('inited')
              // if (self.swiperList.params.loopedSlides < self.swiperList.params.slidesPerView) {
              //   self.swiperList.params.slidesPerView = self.swiperList.loopedSlides;
              //   self.swiperList.destroy(false, false);
              //   self.swiperList.init();
              // }
            },
          },
        })
        console.log(self.swiperList.params)

        console.log(self.swiperList.params.loopedSlides)

      }, 1000)
    }

    // self.el.find('.experience-list')

    // var owl = self.el.find('.list');
    // var slides = self.el.find('img').length;
    // owl.owlCarousel({
    //   items: slides,
    //   loop: true,
    //   margin: 20,
    //   autoplay: true,
    //   slideTransition: 'linear',
    //   autoplayTimeout: 0,
    //   autoplaySpeed: 50000,
    //   autoplayHoverPause: false
    //
    // });

  }
}


