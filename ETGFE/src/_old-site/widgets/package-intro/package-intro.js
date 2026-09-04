import(/* webpackMode: "eager" */ './package-intro.scss');

function PackageIntro (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PackageIntro init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.package-intro').each(function(i, el){
  $(el).data('widget', new PackageIntro(el));
  $(el).data('widget').init();
});
