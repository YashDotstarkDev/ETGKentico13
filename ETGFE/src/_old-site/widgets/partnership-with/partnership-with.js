import(/* webpackMode: "eager" */ './partnership-with.scss');

function PartnershipWith (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PartnershipWith init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.partnership-with').each(function(i, el){
  $(el).data('widget', new PartnershipWith(el));
  $(el).data('widget').init();
});
