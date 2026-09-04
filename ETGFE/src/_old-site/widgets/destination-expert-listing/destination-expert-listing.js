import(/* webpackMode: "eager" */ './destination-expert-listing.scss');

function DestinationExpertListing (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('DestinationExpertListing init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.destination-expert-listing').each(function(i, el){
  $(el).data('widget', new DestinationExpertListing(el));
  $(el).data('widget').init();
});
