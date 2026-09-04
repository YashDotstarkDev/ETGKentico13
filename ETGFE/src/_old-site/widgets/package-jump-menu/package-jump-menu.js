import('./package-jump-menu.scss')

Promise.all([
  import('../../plugins/semantic/dropdown.scss'),
  import('../../plugins/semantic/dropdown.js'),
]).then(() => {
  $('.widget.package-jump-menu').each(function (i, el) {
    $(el).data('widget', new PackageJumpMenu(el))
    $(el).data('widget').init()
  })
})

function PackageJumpMenu (el) {
  const self = this;
  self.el = $(el);
  self.currentSection = null

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PackageJumpMenu init', self);
    }
    self.el.css('opacity', 1);

    self.el.find('select').dropdown({
      onChange: function (value, text, $selectedItem) {
        self.scrollToSection(value)
      }
    })

    self.getCurrentSection()

    $(window).on('scroll', function () {
      self.getCurrentSection()
    })

    self.el.find('.menu-links a').on('click', function (e) {
      e.preventDefault()
      const href = $(this).attr('href')
      self.scrollToSection(href)
    })
  }

  self.getCurrentSection = function () {
    self.currentSection = null

    $('.scroll-section').each(function (index, element) {
      if ($(window).scrollTop() + 250 > $(element).offset().top) {
        self.currentSection = '#' + $(element).attr('id');
      }
    })
    if (!self.currentSection) {
      self.currentSection = '#' + $('.scroll-section').first().attr('id');
    }

    self.el.find('.menu-select select').dropdown('set text', self.el.find('.menu-links a[href="' + self.currentSection + '"]').text());
    self.el.find('.menu-links a').removeClass('active');
    self.el.find('.menu-links a[href="' + self.currentSection + '"]').addClass('active');
  }

  self.scrollToSection = function(sectionId) {
    const section = $(sectionId);
    const sectionOffset = section.offset().top - 200;
    $('html, body').animate({
      scrollTop: sectionOffset
    }, 500);
  }
}
