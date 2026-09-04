Promise.all([
  import('../tour-tiles/tour-tiles'),
  import(/* webpackMode: "eager" */ './tour-tiles-collection.scss'),
]).then(() => {
  $('.widget.tour-tiles-collection').each(function (i, el) {
    $(el).data('widget', new TourTilesCollection(el))
    $(el).data('widget').init()
  })
})

function TourTilesCollection (el, Swiper) {
  const self = this
  self.el = $(el)

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('TourTilesCollection init', self)
    }
    self.el.css('opacity', 1)
  }
}
