import('./new-search-bar.scss');

function NewSearchBar (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('NewSearchBar init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.new-search-bar').each(function(i, el){
  $(el).data('widget', new NewSearchBar(el));
  $(el).data('widget').init();
});