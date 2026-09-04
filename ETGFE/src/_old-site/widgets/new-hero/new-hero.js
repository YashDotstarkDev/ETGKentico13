import('../../plugins/swiper/swiper.js').then(({ default: Swiper }) => {
  Promise.all([
    import('../../plugins/swiper/swiper.css'),
    import(/* webpackMode: "eager" */ './new-hero.scss'),
    import('../../plugins/semantic/modal.css'),
    import('../../plugins/semantic/modal.js'),
    import('../../plugins/semantic/dimmer.css'),
    import('../../plugins/semantic/dimmer.js'),
  ]).then(() => {
    $('.widget.new-hero').each(function (i, el) {
      $(el).data('widget', new NewHero(el, Swiper))
      $(el).data('widget').init()
    })
  })
})

function NewHero (el, Swiper) {
  const self = this
  self.el = $(el)

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('NewHero init', self)
    }
    self.el.css('opacity', 1)

    self.shareModal = self.el.find('.share-modal').modal()
    self.el.find('.js-open-share-modal').click(function (event) {
      event.preventDefault()
      self.shareModal.modal('show')
    })

    const swiperContainer = self.el.find('.swiper-container')
    const swiperSlides = swiperContainer.find('.swiper-slide')

    const swiperConfig = {
      autoplay: {
        delay: 3000,
        disableOnInteraction: false
      },
      effect: 'fade',
      speed: 1500,
      loop: true,
      spaceBetween: 0
    }

    if (swiperSlides.length > 1) {
      // init slider once first slide image to avoid swiper slide sizing issues.
      let firstSlideImg = $(swiperSlides[0]).find('img')
      firstSlideImg.one('load', function () {
        self.swiper = new Swiper(swiperContainer, swiperConfig)
      }).each(function () {
        if(this.complete) {
          self.swiper = new Swiper(swiperContainer, swiperConfig)
        }
      })
    }
  }
}
