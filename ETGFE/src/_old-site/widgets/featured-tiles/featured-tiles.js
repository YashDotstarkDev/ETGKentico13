import('../../plugins/swiper/swiper.js').then(({ default: Swiper }) => {
  Promise.all([
    import(/* webpackMode: "eager" */ './featured-tiles.scss'),
    import('../../plugins/swiper/swiper.css'),
  ]).then(() => {
    $('.widget.featured-tiles').each(function (i, el) {
      $(el).data('widget', new FeaturedTiles(el, Swiper))
      $(el).data('widget').init()
    })
  })
})

function FeaturedTiles (el, Swiper) {
  const self = this
  self.el = $(el)

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('FeaturedTiles init', self)
    }
    self.el.css('opacity', 1)

    if ($(window).width() > 1024) {
      var numberSlides = self.el.find('.swiper-slide').length

      if (numberSlides <= 5) {
        self.el.find('.carousel').addClass('center')
      } else {
        setTimeout(function () {
          self.swiper = new Swiper(self.el.find('.swiper-container'), {
            slidesPerView: 'auto',
            freeMode: false,
            loop: true,
            spaceBetween: 0,
            autoplay: {
              delay: 5000
            },
            navigation: {
              nextEl: self.el.find('.swiper-button-next'),
              prevEl: self.el.find('.swiper-button-prev')
            }
          })

        }, 1000)
      }
    }

    if ($(window).width() <= 1024) {
      var numberSlides = self.el.find('.swiper-slide').length

      if (numberSlides <= 2) {
        self.el.find('.carousel').addClass('center')
      } else {
        setTimeout(function () {
          self.swiper = new Swiper(self.el.find('.swiper-container'), {
            slidesPerView: 'auto',
            freeMode: false,
            loop: true,
            spaceBetween: 0,
            autoplay: {
              delay: 5000
            },
            navigation: {
              nextEl: self.el.find('.swiper-button-next'),
              prevEl: self.el.find('.swiper-button-prev')
            }
          })

        }, 1000)
      }
    }

    if ($(window).width() <= 414) {
      var numberSlides = self.el.find('.swiper-slide').length

      if (numberSlides == 1) {
        self.el.find('.carousel').addClass('center')
      } else {
        self.el.find('.carousel').removeClass('center')
        setTimeout(function () {
          self.swiper = new Swiper(self.el.find('.swiper-container'), {
            slidesPerView: 'auto',
            freeMode: false,
            loop: true,
            spaceBetween: 0,
            autoplay: {
              delay: 5000
            },
            navigation: {
              nextEl: self.el.find('.swiper-button-next'),
              prevEl: self.el.find('.swiper-button-prev')
            }
          })

        }, 1000)
      }
    }

    self.el.find('.swiper-container').mouseenter(function () {
      self.swiper.autoplay.stop()

    })

    self.el.find('.swiper-container').mouseleave(function () {
      self.swiper.autoplay.start()
    })
  }
}


