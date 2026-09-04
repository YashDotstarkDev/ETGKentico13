import(/* webpackMode: "eager" */ './form.scss');

function Form (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('Form init', self);
    }
    self.el.css('opacity', 1);
  }
}

$('.widget.form').each(function(i, el){
  $(el).data('widget', new Form(el));
  $(el).data('widget').init();
});
