Promise.all([
  import('../tour-tiles/tour-tiles'),
  import(/* webpackMode: "eager" */ './new-featured-packages.scss'),
]).then(() => {
  $('.widget.new-featured-packages').each(function (i, el) {
    $(el).data('widget', new NewFeaturedPackages(el))
    $(el).data('widget').init()
  })
})

function NewFeaturedPackages (el) {
  const self = this
  self.el = $(el)

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('NewFeaturedPackages init', self)
    }
    self.el.css('opacity', 1)
  }
}
