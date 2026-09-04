import(/* webpackMode: "eager" */ './package-brochure.scss');

function PackageBrochure (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PackageBrochure init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.package-brochure').each(function(i, el){
  $(el).data('widget', new PackageBrochure(el));
  $(el).data('widget').init();
});
