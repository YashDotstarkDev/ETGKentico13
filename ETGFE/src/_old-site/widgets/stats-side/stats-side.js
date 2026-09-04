import(/* webpackMode: "eager" */ './stats-side.scss');

function StatsSide (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('StatsSide init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.stats-side').each(function(i, el){
  $(el).data('widget', new StatsSide(el));
  $(el).data('widget').init();
});
