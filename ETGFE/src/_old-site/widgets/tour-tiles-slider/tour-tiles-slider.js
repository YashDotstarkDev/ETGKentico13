import('../../plugins/swiper/swiper.js').then(({ default: Swiper }) => {
  Promise.all([
    import('../../plugins/swiper/swiper.css'),
    import(/* webpackMode: "eager" */ './tour-tiles-slider.scss'),
  ]).then(() => {
    $('.widget.tour-tiles-slider').each(function (i, el) {
      $(el).data('widget', new TourTilesSlider(el, Swiper))
      $(el).data('widget').init()
    })
  })
})

function TourTilesSlider (el, Swiper) {
  const self = this
  self.el = $(el)

  self.initTilesSlider = function () {
    let screenWidth = $(window).width()
    if (screenWidth < 1024 && !self.swiper) {
      self.swiper = new Swiper(self.el.find('.tour-tiles-swiper-container'), {
        slidesPerView: 'auto',
        freeMode: true,
        spaceBetween: 15,
        resistance: true,
        resistanceRatio: 0,
      })
    } else if (screenWidth >= 1024 && self.swiper) {
      self.swiper.destroy()
      self.swiper = null
      self.el.find('.tour-tiles-swiper-wrapper').removeAttr('style')
      self.el.find('.tour-tiles-swiper-slide').removeAttr('style')
    }
  }

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('TourTilesSlider init', self)
    }
    self.el.css('opacity', 1)

    // Init each tile image slider
    self.el.find('.tour-tiles-swiper-slide .tour .carousel').each(function (i, el) {
      const tile = $(el)
      tile.data(
        'swiper',
        new Swiper(tile.find('.swiper'), {
          loop: true,
          spaceBetween: 0,
          slidesPerView: 1,
        })
      )

      tile.on('click', '.next', function () {
        tile.data('swiper').slideNext()
      })

      tile.on('click', '.prev', function () {
        tile.data('swiper').slidePrev()
      })
    })

    self.initTilesSlider()
    window.addEventListener("resize", self.initTilesSlider);
  }
}
