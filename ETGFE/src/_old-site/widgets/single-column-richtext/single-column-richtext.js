import(/* webpackMode: "eager" */ './single-column-richtext.scss');

function SingleColumnRichtext (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('SingleColumnRichtext init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.single-column-richtext').each(function(i, el){
  $(el).data('widget', new SingleColumnRichtext(el));
  $(el).data('widget').init();
});
