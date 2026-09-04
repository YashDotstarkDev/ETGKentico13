import(/* webpackMode: "eager" */ './table.scss');

function Table (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('Table init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.table').each(function(i, el){
  $(el).data('widget', new Table(el));
  $(el).data('widget').init();
});
