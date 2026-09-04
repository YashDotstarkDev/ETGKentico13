Promise.all([
  import(/* webpackMode: "eager" */ './brochure-listing.scss'),
  import('../../plugins/semantic/form.scss'),
  import('../../plugins/semantic/form.js'),
  import('../../dbs/scripts/form/dbs.semantic.form.js'),
]).then(() => {
  $('.widget.brochure-listing').each(function (i, el) {
    $(el).data('widget', new BrochureListing(el))
    $(el).data('widget').init()
  })
})

function BrochureListing (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('BrochureListing init', self);
    }
    self.el.css('opacity', 1);

    self.el.find('.toggle-add').click(function(event) {
      event.preventDefault();
      $(this).parents('.item').toggleClass('added');
      var numberAdded  = self.el.find('.item.added').length;

      self.el.find('.email-brochure .label span').html(numberAdded);

      self.brochureList = [];

      self.el.find('.added').each(function(i, el){
        self.brochureList.push($(el).attr('data-brochure'));
      });

      self.el.find('.added-brochures').val(self.brochureList.join(', '));
    });



  }
}

