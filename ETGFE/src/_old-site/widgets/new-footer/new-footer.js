import('../../plugins/swiper/swiper.js').then(({ default: Swiper }) => {
  Promise.all([
    import('../../plugins/swiper/swiper.css'),
    import(/* webpackMode: "eager" */ './new-footer.scss')
  ]).then(() => {
    $('.widget.new-footer').each(function (i, el) {
      $(el).data('widget', new NewFooter(el, Swiper))
      $(el).data('widget').init()
    })
  })
})

function NewFooter (el, Swiper) {
  const self = this
  self.el = $(el)

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('NewFooter init', self)
    }
    self.el.css('opacity', 1)

    self.el.find('.categories .inner').click(function (event) {
      event.preventDefault()
      self.el.find('.links').slideToggle()
      $(this).parents('.categories').toggleClass('expanded')
    })

    self.swiper = new Swiper(self.el.find('.brands .swiper-container'), {
      autoplay: {
        delay: 2000,
        disableOnInteraction: false
      },
      speed: 1500,
      loop: true,
      slidesPerView: 'auto',
      spaceBetween: 0
    })
  }
}

