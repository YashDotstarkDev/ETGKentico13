Promise.all([
  import(/* webpackMode: "eager" */ './generic-enquire-side.scss'),
  import('../../dbs/scripts/form/dbs.semantic.form.js'),
]).then(() => {
  $('.widget.generic-enquire-side').each(function (i, el) {
    $(el).data('widget', new GenericEnquireSide(el))
    $(el).data('widget').init()
  })
})

function GenericEnquireSide (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('GenericEnquireSide init', self);
    }
    self.el.css('opacity', 1);

    self.el.find('.make-dropdown').dropdown({
      placeholder: false,
      onChange: function(value) {
        self.el.find('.lines .line .number').html(self.el.find('.data-set[data-value="' + this.value + '"]').attr('data-phone'));
        self.el.find('.lines .line.meet-url').attr('href', self.el.find('.data-set[data-value="' + this.value + '"]').attr('data-url'));

      }
    });


  }
}


