import('./feature-box.scss');

function FeatureBox (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('FeatureBox init', self);
    }
    self.el.css('opacity', 1);

    self.el.find('.feature-accordion .item .header').each(function(i, el){
      $(el).click(function(e){
        e.preventDefault();
        $(this).parents('.item').toggleClass('active')

        if ($(this).parents('.item').hasClass('active')) {
          $(this).siblings('.content').slideDown(200)
        } else {
          $(this).siblings('.content').slideUp(200)
        }
      });
    });

    self.el.find('.calendry-button').click(function(e){
      e.preventDefault();
      try{

        Calendly.showPopupWidget($(this).attr('data-link'));
      }catch(ex){

      }
    })
  }
}

$('.widget.feature-box').each(function(i, el){
  $(el).data('widget', new FeatureBox(el));
  $(el).data('widget').init();
});
