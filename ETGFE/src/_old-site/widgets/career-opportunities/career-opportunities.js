import(/* webpackMode: "eager" */ './career-opportunities.scss');

function CareerOpportunities (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('CareerOpportunities init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.career-opportunities').each(function(i, el){
  $(el).data('widget', new CareerOpportunities(el));
  $(el).data('widget').init();
});
