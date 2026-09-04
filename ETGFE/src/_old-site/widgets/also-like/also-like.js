import('../../plugins/swiper/swiper.js').then(({ default: Swiper }) => {
  Promise.all([
    import(/* webpackMode: "eager" */ './also-like.scss'),
    import('../../plugins/swiper/swiper.css'),
    import('../product-tile/product-tile.scss'),
    import('../product-tile/product-tile.js'),
  ]).then(() => {
    $('.widget.also-like').each(function (i, el) {
      $(el).data('widget', new AlsoLike(el, Swiper))
      $(el).data('widget').init()
    })
  })
})

function AlsoLike (el, Swiper) {
  const self = this
  self.el = $(el)

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('AlsoLike init', self)
    }
    self.el.css('opacity', 1)

    $(window).on('resize', function () {
      self.initCarousel()
    })

    self.initCarousel()
  }

  self.initCarousel = function () {
    let screenWidth = $(window).width()

    console.log('init carousel', screenWidth, self.swiper)

    if (screenWidth < 1024 && !self.swiper) {
      self.swiper = new Swiper(self.el.find('.swiper-container'), {
        slidesPerView: 'auto',
        freeMode: true,
        spaceBetween: 0,
        resistance: true,
        resistanceRatio: 0
        // on: {
        //   init: function () {
        //     self.el.addClass('beginning');
        //   },
        //   reachBeginning: function () {
        //     self.el.addClass('beginning');
        //   },
        //   reachEnd: function () {
        //     self.el.addClass('end');
        //   },
        //   fromEdge: function () {
        //     self.el.removeClass('beginning end');
        //   }
        // }
      })

    } else if (screenWidth > 1023 && self.swiper) {
      self.swiper.destroy()
      self.swiper = null
      self.el.find('.swiper-wrapper').removeAttr('style')
      self.el.find('.swiper-slide').removeAttr('style')
    }
  }
}


