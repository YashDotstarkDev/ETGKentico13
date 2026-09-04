import(/* webpackMode: "eager" */ './footer.scss');

function Footer (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('Footer init', self);
    }
    self.el.css('opacity', 1);

    self.el.find('.categories .inner').click(function(event) {
      event.preventDefault();
      self.el.find('.links').slideToggle();
      $(this).parents('.categories').toggleClass('expanded');
    });
  }
}

$('.widget.footer').each(function(i, el){
  $(el).data('widget', new Footer(el));
  $(el).data('widget').init();
});
