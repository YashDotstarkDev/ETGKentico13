Promise.all([
  import(/* webpackMode: "eager" */ './filepicker.scss'),
  import(/* webpackMode: "eager" */ '../../dbs/scripts/form/dbs.form.filePicker.js')
]).then(() => {
  $('.widget.filepicker').each(function (i, el) {
    $(el).data('widget', new Filepicker(el))
    $(el).data('widget').init()
  })
})

function Filepicker (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('Filepicker init', self);
    }
    self.el.css('opacity', 1);
    self.filePicker = new dbs.form.filePicker(self.el);
  }
}


