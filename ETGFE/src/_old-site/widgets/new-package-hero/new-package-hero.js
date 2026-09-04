Promise.all([
  import(/* webpackMode: "eager" */ './new-package-hero.scss'),
  import('../../plugins/semantic/modal.css'),
  import('../../plugins/semantic/modal.js'),
  import('../../plugins/semantic/dimmer.css'),
  import('../../plugins/semantic/dimmer.js'),
]).then(() => {
  $('.widget.new-package-hero').each(function (i, el) {
    $(el).data('widget', new NewPackageHero(el))
    $(el).data('widget').init()
  })
})

function copyUrl() {
  $('.url-holder').select();
  document.execCommand("copy");
}

function NewPackageHero(el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('NewPackageHero init', self);
    }
    self.el.css('opacity', 1);

    // self.photoModal = self.el.find('.photo-modal').modal();
    self.shareModal = self.el.find('.share-modal').modal();

    self.el.find('.js-open-photo-modal').click(function (event) {
      event.preventDefault();
      // self.photoModal.modal('show');
      // setTimeout(() => {
      //   $(window).trigger('resize');
      // }, 500);

      self.el.find('.new-photo-modal').addClass('active');
    });

    self.el.find('.js-open-share-modal').click(function (event) {
      event.preventDefault();
      self.shareModal.modal('show');
    });

    self.el.find('.new-photo-modal .back-button').click(function (event) {
      event.preventDefault();
      self.el.find('.new-photo-modal').removeClass('active');
      self.el.find('.new-photo-modal .photo-overlay').scrollTop(0);
      self.el.find('.new-photo-modal .thumbnail-overlay').scrollTop(0);
    });

    self.el.find('.new-photo-modal .thumbs-button').click(function (event) {
      event.preventDefault();
      self.el.find('.new-photo-modal .thumbnail-overlay').toggleClass('active');

      if (self.el.find('.new-photo-modal .thumbnail-overlay').hasClass('active')) {
        $(this).addClass('active')
        $(this).find('.label').text('Full size');
        // self.el.find('.new-photo-modal .photo-overlay').scrollTop(0);
      } else {
        $(this).removeClass('active')
        $(this).find('.label').text('Thumbnails');
      }
    });

    self.el.find('.new-photo-modal .thumbnail-list .thumbnail').click(function (event) {
      event.preventDefault();
      var id = $(this).attr('data-id');
      var top = self.el.find('.new-photo-modal .photo-overlay').scrollTop() + self.el.find('.photo[data-id="' + id + '"]').offset().top;

      self.el.find('.new-photo-modal .photo-overlay').scrollTop(top - 120);
      self.el.find('.new-photo-modal .thumbnail-overlay').removeClass('active');
      self.el.find('.new-photo-modal .thumbs-button .label').text('Thumbnails');
    });

    $('.copy-link').popup({
      on: 'click',
      position: 'top right'
    });

    $('.copy-link').click(function (event) {
      event.preventDefault();
      copyUrl()
    });
  }
}


