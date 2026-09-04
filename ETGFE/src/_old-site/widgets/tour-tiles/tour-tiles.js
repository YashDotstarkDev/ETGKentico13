import('../../plugins/swiper/swiper.js').then(({ default: Swiper }) => {
  import(/* webpackMode: "eager" */ './tour-tiles.scss').then(() => {
    import('../../plugins/swiper/swiper.css')
    $('.widget.tour-tiles').each(function (i, el) {
      $(el).data('widget', new TourTiles(el, Swiper))
      $(el).data('widget').init()
    })
  })
})

function TourTiles (el, Swiper) {
  const self = this
  self.el = $(el)

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('TourTiles init', self)
    }
    self.el.css('opacity', 1)

    self.el.find('.tour').each(function (i, el) {
      $(el).data('swiper',
        new Swiper($(el).find('.swiper'), {
          loop: true,
          spaceBetween: 0,
          slidesPerView: 1
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
