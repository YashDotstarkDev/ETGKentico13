import('../../plugins/swiper/swiper.js').then(({ default: Swiper }) => {
  Promise.all([
    import('../../plugins/swiper/swiper.css'),
    import(/* webpackMode: "eager" */ './blog-carousel.scss'),
  ]).then(() => {
    $('.widget.blog-carousel').each(function (i, el) {
      $(el).data('widget', new BlogCarousel(el, Swiper))
      $(el).data('widget').init()
    })
  })
})

function BlogCarousel (el, Swiper) {
  const self = this
  self.el = $(el)

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('BlogCarousel init', self)
    }
    self.el.css('opacity', 1)

    self.swiper = new Swiper(self.el.find('.swiper-container'), {
      slidesPerView: 'auto',
      slidesPerGroup: 3,
      breakpoints: {
        768: {
          slidesPerGroup: 1,
        },
        1024: {
          slidesPerGroup: 2,
        },
      },
      on: {
        init: function () {
          self.el.find('.articles').addClass('beginning')
        },
        reachBeginning: function () {
          self.el.find('.articles').addClass('beginning')
        },
        reachEnd: function () {
          self.el.find('.articles').addClass('end')
        },
        fromEdge: function () {
          self.el.find('.articles').removeClass('beginning end')
        }
      }
    })

    $(el).on('click', '.next', function () {
      self.swiper.slideNext()
    })

    $(el).on('click', '.prev', function () {
      self.swiper.slidePrev()
    })
  }
}
