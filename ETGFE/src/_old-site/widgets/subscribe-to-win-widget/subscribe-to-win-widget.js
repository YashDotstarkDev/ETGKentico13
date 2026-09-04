import(/* webpackMode: "eager" */ './subscribe-to-win-widget.scss');
import('../../plugins/semantic/form.scss');
import('../../plugins/semantic/form.js');
import('../../dbs/scripts/form/dbs.semantic.form.js');

function SubscribeToWinWidget (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('SubscribeToWinWidget init', self);
    }
    self.el.css('opacity', 1);

    console.log('MAKE DROPDOWN')
    self.el.find('.make-dropdown').dropdown({
      placeholder: false
    });
  }
}

$('.widget.subscribe-to-win-widget').each(function(i, el){
  $(el).data('widget', new SubscribeToWinWidget(el));
  $(el).data('widget').init();
});
