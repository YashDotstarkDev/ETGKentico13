import(/* webpackMode: "eager" */ './callouts.scss');

function Callouts (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('Callouts init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.callouts').each(function(i, el){
  $(el).data('widget', new Callouts(el));
  $(el).data('widget').init();
});
