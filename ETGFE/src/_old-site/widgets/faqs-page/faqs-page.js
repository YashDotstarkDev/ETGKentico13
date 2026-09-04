import('./faqs-page.scss');

function FaqsPage (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('FaqsPage init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.faqs-page').each(function(i, el){
  $(el).data('widget', new FaqsPage(el));
  $(el).data('widget').init();
});