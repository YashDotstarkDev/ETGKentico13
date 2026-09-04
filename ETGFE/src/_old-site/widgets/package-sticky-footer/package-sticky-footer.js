import(/* webpackMode: "eager" */ './package-sticky-footer.scss');

function PackageStickyFooter(el) {
  const self = this;
  self.el = $(el);

  self.handleScroll = function () {
    if ($(document).scrollTop() > 200) {
      self.el.addClass('scrolled');
    } else {
      self.el.removeClass('scrolled');
    }
  }

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('PackageStickyFooter init', self);
    }
    self.el.css('opacity', 1);

    $(document).on('scroll', function () {
      self.handleScroll();
    });
    $(window).on('resize load', function () {
      self.handleScroll();
    });
    $('body').on('touchmove', function () {
      self.handleScroll();
    });

    if ($(window).width() <= 480) {
      self.el.find('.bar').click(function () {
        console.log('clicked');
        $(this).toggleClass('expand-sticky')
      });
    }

    $(self.el).find('.book-now-trigger').click(function (e) {
      e.preventDefault();

      window.bookNowClient()

      // let scollToTarget = $(this).attr('data-scrollto-target')
      // let firstStepNumber = $(scollToTarget).find('.step-number').first()

      // if (firstStepNumber.hasClass('animate')) {
      //   firstStepNumber.removeClass('animate')
      // }

      // $("html, body").stop().animate({
      //   scrollTop: $(scollToTarget).position().top - 120
      // }, 700, 'swing', function () {
      //   firstStepNumber.addClass('animate')
      // });
    })

    $(self.el).find('.ta-trigger').click(function (e) {
      e.preventDefault();
      window.bookNowTravelAgent()
    })

  }
}

$('.widget.package-sticky-footer').each(function (i, el) {
  $(el).data('widget', new PackageStickyFooter(el));
  $(el).data('widget').init();
});
