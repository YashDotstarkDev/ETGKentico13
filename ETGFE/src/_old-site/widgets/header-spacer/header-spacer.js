import(/* webpackMode: "eager" */ './header-spacer.scss');

function HeaderSpacer (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('HeaderSpacer init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.header-spacer').each(function(i, el){
  $(el).data('widget', new HeaderSpacer(el));
  $(el).data('widget').init();
});
