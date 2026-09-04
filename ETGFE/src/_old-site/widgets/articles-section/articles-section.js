import(/* webpackMode: "eager" */ './articles-section.scss');

function ArticlesSection (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('ArticlesSection init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.articles-section').each(function(i, el){
  $(el).data('widget', new ArticlesSection(el));
  $(el).data('widget').init();
});
