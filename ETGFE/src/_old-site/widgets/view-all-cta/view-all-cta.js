import(/* webpackMode: "eager" */ './view-all-cta.scss');

function ViewAllCta (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('ViewAllCta init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.view-all-cta').each(function(i, el){
  $(el).data('widget', new ViewAllCta(el));
  $(el).data('widget').init();
});
