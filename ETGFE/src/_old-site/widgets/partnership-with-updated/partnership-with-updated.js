import('./partnership-with-updated.scss');

function PartnershipWithUpdated (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PartnershipWithUpdated init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.partnership-with-updated').each(function(i, el){
  $(el).data('widget', new PartnershipWithUpdated(el));
  $(el).data('widget').init();
});