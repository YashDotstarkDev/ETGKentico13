Promise.all([
  import(/* webpackMode: "eager" */ './faqs.scss'),
  import('../../plugins/semantic/accordion.css'),
  import('../../plugins/semantic/accordion.js'),
]).then(() => {
  $('.widget.faqs').each(function (i, el) {
    $(el).data('widget', new Faqs(el))
    $(el).data('widget').init()
  })
})

function Faqs (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('Faqs init', self);
    }
    self.el.css('opacity', 1);

    self.el.find('.ui.accordion').accordion({
      exclusive: false
    });

    self.el.find('.expander').click(function(e) {
      e.preventDefault();
      $(this).toggleClass('expanded');

      if ($(this).hasClass('expanded')){
        $(this).text("Collapse All")
        self.el.find('.ui.accordion .title').each(function(i){
          $(this).parent().accordion('open',i);
        });
      }else{
        $(this).text("Expand All")
        self.el.find('.ui.accordion .title').each(function(i){
          $(this).parent().accordion('close',i);
        });
      }
    });

  }
}


