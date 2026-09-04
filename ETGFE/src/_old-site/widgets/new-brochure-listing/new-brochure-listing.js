import('./new-brochure-listing.scss');

function NewBrochureListing (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('NewBrochureListing init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.new-brochure-listing').each(function(i, el){
  $(el).data('widget', new NewBrochureListing(el));
  $(el).data('widget').init();
});