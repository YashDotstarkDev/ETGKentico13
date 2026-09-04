Promise.all([
  import(/* webpackMode: "eager" */ './information-accordion.scss'),
  import('../../plugins/semantic/accordion.css'),
  import('../../plugins/semantic/accordion.js'),
]).then(() => {
  $('.widget.information-accordion').each(function (i, el) {
    $(el).data('widget', new InformationAccordion(el))
    $(el).data('widget').init()
  })
})

function InformationAccordion (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('InformationAccordion init', self);
    }
    self.el.css('opacity', 1);

    self.el.find('.ui.accordion').accordion({
      exclusive: false
    });

    self.el.find('.expander').click(function() {
      $(this).toggleClass('expanded');
      console.log('clicked');

      if ($(this).hasClass('expanded')){
        $('.ui.accordion .title').each(function(i){
          $(this).parent().accordion('open',i);
        });
      }else{
        $('.ui.accordion .title').each(function(i){
          $(this).parent().accordion('close',i);
        });
      }
    });

  }
}


