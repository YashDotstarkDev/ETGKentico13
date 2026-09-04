import { dom } from '@fortawesome/fontawesome-svg-core'

Promise.all([
  import(/* webpackMode: "eager" */ './photo-slider.scss'),
  import('../../plugins/slick/slick.css'),
  import('../../plugins/slick/slick'),
  import('../../plugins/slick/slick-theme.css'),
]).then(() => {
  $('.widget.photo-slider').each(function (i, el) {
    $(el).data('widget', new PhotoSlider(el))
    $(el).data('widget').init()
  })
})

function PhotoSlider (el) {
  const self = this
  self.el = $(el)

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('PhotoSlider init', self)
    }
    self.el.css('opacity', 1)

    self.el.find('.slider').slick({
      dots: false,
      speed: 500,
      arrows: true,
      infinite: false,
      slidesToShow: 1,
      slidesToScroll: 1,
      centerMode: false,
      adaptiveHeight: true,
      prevArrow: '<div class="prev"><i class="fa-light fa-arrow-left"></i></div>',
      nextArrow: '<div class="next"><i class="fa-light fa-arrow-right"></i></div>'
    })

    dom.i2svg()
  }
}


