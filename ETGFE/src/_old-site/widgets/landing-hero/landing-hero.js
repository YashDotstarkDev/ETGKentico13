import('../../plugins/swiper/swiper.js').then(({ default: Swiper }) => {
  Promise.all([
    import('../../plugins/swiper/swiper.css'),
    import(/* webpackMode: "eager" */ './landing-hero.scss'),
    import('../../styles/includes/buttons.scss'),
    import('../../plugins/semantic/modal.css'),
    import('../../plugins/semantic/modal.js'),
    import('../../plugins/semantic/popup.css'),
    import('../../plugins/semantic/popup.js'),
    import('../../plugins/semantic/dimmer.css'),
    import('../../plugins/semantic/dimmer.js'),
    import('../../plugins/semantic/transition.css'),
    import('../../plugins/semantic/transition.js'),
  ]).then(() => {
    $('.widget.landing-hero').each(function (i, el) {
      $(el).data('widget', new LandingHero(el, Swiper))
      $(el).data('widget').init()
    })
  })
})

function copyUrl () {
  $('.url-holder').select()
  document.execCommand('copy')
}

function LandingHero (el, Swiper) {
  const self = this
  self.el = $(el)

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('LandingHero init', self)
    }
    self.el.css('opacity', 1)

    self.shareModal = self.el.find('.share-modal').modal()

    self.el.find('.share-button').click(function (event) {
      event.preventDefault()
      self.shareModal.modal('show')
    })

    $('.copy-link').popup({
      on: 'click',
      position: 'top right'
    })

    $('.copy-link').click(function (event) {
      event.preventDefault()
      copyUrl()
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

      }, 1000)
    }

  }
}


