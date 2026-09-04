import(/* webpackMode: "eager" */ './career-description.scss');

function CareerDescription (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('CareerDescription init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.career-description').each(function(i, el){
  $(el).data('widget', new CareerDescription(el));
  $(el).data('widget').init();
});
