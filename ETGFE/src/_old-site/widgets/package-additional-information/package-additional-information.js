Promise.all([
  import(/* webpackMode: "eager" */ './package-additional-information.scss'),
  import('../../plugins/semantic/dropdown.scss'),
  import('../../plugins/semantic/dropdown.js'),
]).then(() => {
  $('.widget.package-additional-information').each(function (i, el) {
    $(el).data('widget', new PackageAdditionalInformation(el))
    $(el).data('widget').init()
  })
})

function PackageAdditionalInformation (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PackageAdditionalInformation init', self);
    }
    self.el.css('opacity', 1);
  }
}


