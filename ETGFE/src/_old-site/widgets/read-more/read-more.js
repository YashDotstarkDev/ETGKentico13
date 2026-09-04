import(/* webpackMode: "eager" */ './read-more.scss');

function ReadMore (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('ReadMore init', self);
    }
    self.el.css('opacity', 1);

    self.el.find('.read-more-button').click(function(event) {
      event.preventDefault();

      $(this).parents('.copy').toggleClass('expanded');
    });
  }
}

$('.widget.read-more').each(function(i, el){
  $(el).data('widget', new ReadMore(el));
  $(el).data('widget').init();
});
