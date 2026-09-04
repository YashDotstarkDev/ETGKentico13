import(/* webpackMode: "eager" */ './page-404.scss');

function Page404 (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('Page404 init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.page-404').each(function(i, el){
  $(el).data('widget', new Page404(el));
  $(el).data('widget').init();
});
