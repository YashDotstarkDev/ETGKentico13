import('../package-jump-menu/package-jump-menu')
import('./new-destination-page.scss');

function NewDestinationPage (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('NewDestinationPage init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.new-destination-page').each(function(i, el){
  $(el).data('widget', new NewDestinationPage(el));
  $(el).data('widget').init();
});
