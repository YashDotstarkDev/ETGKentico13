import('./new-covered.scss');

function NewCovered (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('NewCovered init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.new-covered').each(function(i, el){
  $(el).data('widget', new NewCovered(el));
  $(el).data('widget').init();
});