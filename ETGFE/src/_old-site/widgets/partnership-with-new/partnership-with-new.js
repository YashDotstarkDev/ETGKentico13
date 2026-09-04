import(/* webpackMode: "eager" */ './partnership-with-new.scss');

function PartnershipWithNew (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PartnershipWith init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.partnership-with-new').each(function(i, el){
  $(el).data('widget', new PartnershipWithNew(el));
  $(el).data('widget').init();
});
