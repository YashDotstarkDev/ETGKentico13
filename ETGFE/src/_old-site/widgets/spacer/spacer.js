import(/* webpackMode: "eager" */ './spacer.scss');

function Spacer (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('Spacer init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.spacer').each(function(i, el){
  $(el).data('widget', new Spacer(el));
  $(el).data('widget').init();
});
