import(/* webpackMode: "eager" */ './article-hero.scss');

function ArticleHero (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('ArticleHero init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.article-hero').each(function(i, el){
  $(el).data('widget', new ArticleHero(el));
  $(el).data('widget').init();
});
