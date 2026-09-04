import(/* webpackMode: "eager" */ './content-500.scss');

function Content500 (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('Content500 init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.content-500').each(function(i, el){
  $(el).data('widget', new Content500(el));
  $(el).data('widget').init();
});
