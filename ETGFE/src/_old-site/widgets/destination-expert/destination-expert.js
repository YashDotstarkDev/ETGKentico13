import(/* webpackMode: "eager" */ './destination-expert.scss');

function DestinationExpert (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('DestinationExpert init', self);
    }
    self.el.css('opacity', 1);

    self.el.find('.read-more').click(function(event) {
      event.preventDefault();

      $(this).parents('.copy').toggleClass('expanded');
    });
  }
}

$('.widget.destination-expert').each(function(i, el){
  $(el).data('widget', new DestinationExpert(el));
  $(el).data('widget').init();
});
