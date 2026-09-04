import(/* webpackMode: "eager" */ './related-articles.scss');

function RelatedArticles (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('RelatedArticles init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.related-articles').each(function(i, el){
  $(el).data('widget', new RelatedArticles(el));
  $(el).data('widget').init();
});
