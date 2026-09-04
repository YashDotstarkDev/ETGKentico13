import('../../plugins/swiper/swiper.js').then(({ default: Swiper }) => {
  Promise.all([
    import('../../plugins/swiper/swiper.css'),
    import(/* webpackMode: "eager" */ './experience-list.scss')
  ]).then(() => {
    $('.widget.experience-list').each(function (i, el) {
      $(el).data('widget', new ExperienceList(el, Swiper))
      $(el).data('widget').init()
    })
  })
})

function ExperienceList (el, Swiper) {
  const self = this
  self.el = $(el)

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('ExperienceList init', self)
    }
    self.el.css('opacity', 1)

    var slideWidth = 0
    var windowWidth = $(window).outerWidth() - 260

    self.el.find('.swiper-slide').each(function (i, el) {
      slideWidth += $(el).outerWidth()
    })

    self.el.find('.list').css('width', slideWidth)

    if (slideWidth > windowWidth) {
      setTimeout(function () {
        self.swiperList = new Swiper(self.el.find('.swiper-container'), {
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
}
