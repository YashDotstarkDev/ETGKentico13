Promise.all([
  import(/* webpackMode: "eager" */ './package-hero.scss'),
  import('../../plugins/semantic/modal.css'),
  import('../../plugins/semantic/modal.js'),
  import('../../plugins/semantic/dimmer.css'),
  import('../../plugins/semantic/dimmer.js'),
  import('../photo-gallery/photo-gallery')
]).then(() => {
  $('.widget.package-hero').each(function (i, el) {
    $(el).data('widget', new PackageHero(el))
    $(el).data('widget').init()
  })
})

function copyUrl() {
  $('.url-holder').select();
  document.execCommand("copy");
}

function PackageHero (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PackageHero init', self);
    }
    self.el.css('opacity', 1);

    self.modal = self.el.find('.share-modal').modal();
    self.photoModal = self.el.find('.photo-modal').modal();


    self.el.find('.share-button').click(function(event) {
      event.preventDefault();
      self.modal.modal('show');
    });

    self.el.find('.carousel').slick({
      dots: true,
      infinite: true,
      speed: 300,
      arrows:true,
      slidesToShow: 1,
      slidesToScroll: 1,
      prevArrow:"<div class='slick-prev'><svg width=\"11\" height=\"11\" viewBox=\"0 0 11 11\" fill=\"none\" xmlns=\"http://www.w3.org/2000/svg\">\n" +
        "<path d=\"M6.02344 1.07031C6.16406 1.1875 6.21094 1.32812 6.21094 1.46875C6.21094 1.63281 6.14062 1.77344 6.02344 1.86719L3.21094 4.5625H9.9375C10.1016 4.5625 10.2422 4.63281 10.3359 4.72656C10.4531 4.84375 10.5 4.98438 10.5 5.125V5.875C10.5 6.03906 10.4531 6.17969 10.3359 6.27344C10.2422 6.39062 10.1016 6.4375 9.9375 6.4375H3.21094L6.02344 9.13281C6.14062 9.25 6.21094 9.39062 6.21094 9.53125C6.21094 9.69531 6.16406 9.83594 6.02344 9.92969L5.50781 10.4453C5.41406 10.5625 5.27344 10.6094 5.10938 10.6094C4.96875 10.6094 4.82812 10.5625 4.71094 10.4453L0.164062 5.89844C0.0703125 5.80469 0 5.66406 0 5.5C0 5.35938 0.0703125 5.21875 0.164062 5.10156L4.71094 0.554688C4.82812 0.460938 4.96875 0.390625 5.10938 0.390625C5.27344 0.390625 5.41406 0.460938 5.50781 0.554688L6.02344 1.07031Z\" fill=\"white\"/>\n" +
        "</svg></div>",
      nextArrow:"<div class='slick-next'><svg width=\"11\" height=\"11\" viewBox=\"0 0 11 11\" fill=\"none\" xmlns=\"http://www.w3.org/2000/svg\">\n" +
        "<path d=\"M4.97656 1.07031C4.83594 1.1875 4.78906 1.32812 4.78906 1.46875C4.78906 1.63281 4.85938 1.77344 4.97656 1.86719L7.78906 4.5625H1.0625C0.898438 4.5625 0.757812 4.63281 0.664062 4.72656C0.546875 4.84375 0.5 4.98438 0.5 5.125V5.875C0.5 6.03906 0.546875 6.17969 0.664062 6.27344C0.757812 6.39062 0.898438 6.4375 1.0625 6.4375H7.78906L4.97656 9.13281C4.85938 9.25 4.78906 9.39062 4.78906 9.53125C4.78906 9.69531 4.83594 9.83594 4.97656 9.92969L5.49219 10.4453C5.58594 10.5625 5.72656 10.6094 5.89062 10.6094C6.03125 10.6094 6.17188 10.5625 6.28906 10.4453L10.8359 5.89844C10.9297 5.80469 11 5.66406 11 5.5C11 5.35938 10.9297 5.21875 10.8359 5.10156L6.28906 0.554688C6.17188 0.460938 6.03125 0.390625 5.89062 0.390625C5.72656 0.390625 5.58594 0.460938 5.49219 0.554688L4.97656 1.07031Z\" fill=\"white\"/>\n" +
        "</svg></div>",
        responsive: [
          {
            breakpoint: 768,
            settings: {
              dots: false
            }
          }]
    });

    $('.copy-link').popup({
      on: 'click',
      position   : 'top right'
    });

    $('.copy-link').click(function(event) {
      event.preventDefault();
      copyUrl()
    });

    self.el.find('.photo-button').click(function(event) {
      event.preventDefault();
      self.photoModal.modal('show');
      $(window).trigger('resize');
    });
  }
}


