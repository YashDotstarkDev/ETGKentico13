import('../../plugins/swiper/swiper.js').then(({ default: Swiper }) => {
  import(/* webpackMode: "eager" */ './new-experience-bar.scss').then(() => {
    import('../../plugins/swiper/swiper.css')
    $('.widget.new-experience-bar').each(function (i, el) {
      $(el).data('widget', new NewExperienceBar(el, Swiper))
      $(el).data('widget').init()
    })
  })
})

function NewExperienceBar (el, Swiper) {
  const self = this
  self.el = $(el)

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('NewExperienceBar init', self)
    }
    self.el.css('opacity', 1)

    self.el.find('.carousel').each(function (i, el) {
      $(el).data(
        'swiper',
        new Swiper($(el).find('.swiper'), {
          loop: false,
          spaceBetween: 0,
          slidesPerView: 'auto',
          slidesPerGroup: 1, // todo make this responsive
          centeredSlides: false,
          initialSlide: 1,
          breakpoints: {
            640: {
              slidesPerGroup: 1,
              centeredSlides: true,
            }
          },
          on: {
            reachEnd: function () {
              $(el).addClass('end')
            },
            reachBeginning: function () {
              $(el).addClass('beginning')
            },
            fromEdge: function () {
              $(el).removeClass('end beginning')
            }
          }
        })
      )

      $(el).on('click', '.next', function () {
        $(el).data('swiper').slideNext()
      })

      $(el).on('click', '.prev', function () {
        $(el).data('swiper').slidePrev()
      })
    })
  }
}
