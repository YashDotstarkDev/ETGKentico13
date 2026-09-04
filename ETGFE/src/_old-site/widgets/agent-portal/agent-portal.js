import(/* webpackMode: "eager" */ './agent-portal.scss');

function AgentPortal (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('AgentPortal init', self);
    }
    self.el.css('opacity', 1);

    self.el.find('.mobile-menu-trigger').click(function(event) {
      event.preventDefault();
      self.el.addClass('show-mobile-subnav');
    });


    self.el.find('.nav a').click(function(event) {
      event.preventDefault();

      $(this).siblings().removeClass('active');
      $(this).addClass('active');
      self.el.find('.bottom .set').hide();
      self.el.find('.bottom .set[data-set="' + $(this).attr('data-set') + '"]').fadeIn();
    });

    self.el.find('.mobile-nav .links a').click(function(event) {
      event.preventDefault();
      self.el.removeClass('show-mobile-subnav');

      $(this).siblings().removeClass('active');
      $(this).addClass('active');

      self.el.find('.bottom .set').hide();
      self.el.find('.bottom .set[data-set="' + $(this).attr('data-set') + '"]').fadeIn();

    });

    self.el.find('.mobile-nav .close').click(function(event) {
      event.preventDefault();
      self.el.removeClass('show-mobile-subnav');
    });

    setTimeout(function(){
      self.el.find('.bottom .set[data-set="' + self.el.find('.nav .active').attr('data-set') + '"]').fadeIn();
    }, 500);

  }
}

$('.widget.agent-portal').each(function(i, el){
  $(el).data('widget', new AgentPortal(el));
  $(el).data('widget').init();
});
