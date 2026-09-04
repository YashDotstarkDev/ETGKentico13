import('../../plugins/swiper/swiper.js').then(({ default: Swiper }) => {
  Promise.all([
    import(/* webpackMode: "eager" */ './page-hero.scss'),
    import('../../plugins/semantic/modal.css'),
    import('../../plugins/semantic/modal.js'),
    import('../../plugins/semantic/dimmer.css'),
    import('../../plugins/semantic/dimmer.js'),
    import('../../plugins/semantic/popup.js'),
    import('../photo-gallery/photo-gallery')
  ]).then(() => {
    $('.widget.page-hero').each(function (i, el) {
      $(el).data('widget', new PageHero(el, Swiper))
      $(el).data('widget').init()
    })
  })
})

function copyUrl () {
  $('.url-holder').select()
  document.execCommand('copy')
}

function PageHero (el, Swiper) {
  const self = this
  self.el = $(el)

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('PageHero init', self)
    }
    self.el.css('opacity', 1)

    self.shareModal = self.el.find('.share-modal').modal()
    self.photoModal = self.el.find('.photo-modal').modal()

    self.el.find('.share-button').click(function (event) {
      event.preventDefault()
      self.shareModal.modal('show')
    })

    $('.copy-link').popup({
      on: 'click',
      position: 'top right'
    })

    $('.copy-link').click(function (event) {
      event.preventDefault()
      copyUrl()
    })

    self.el.find('.photo-button').click(function (event) {
      event.preventDefault()
      self.photoModal.modal('show')
      $(window).trigger('resize')
    })

  }
}


