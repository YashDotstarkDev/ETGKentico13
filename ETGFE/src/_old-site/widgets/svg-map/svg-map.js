import(/* webpackMode: "eager" */ './svg-map.scss');

function SvgMap (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('SvgMap init', self);
    }
    self.el.css('opacity', 1);

    self.el.find('#Regions g').click(function(e){
      e.preventDefault();
      window.location.href = self.el.find('.region[data-region-id="' + $(this).attr('id') + '"]').attr('href');
    });

    self.el.find('#Regions g').hover(function () {
      self.el.find('.list .region[data-region-id="' + $(this).attr('id') + '"]').addClass('active');
    }, function () {
      self.el.find('.list .region[data-region-id="' + $(this).attr('id') + '"]').removeClass('active');
    })

    self.el.find('.list .region').hover(function () {
      self.el.find('#Regions g[id="' + $(this).attr('data-region-id') + '"]').addClass('active');
    }, function () {
      self.el.find('#Regions g[id="' + $(this).attr('data-region-id') + '"]').removeClass('active');
    })
  }
}

$('.widget.svg-map').each(function(i, el){
  $(el).data('widget', new SvgMap(el));
  $(el).data('widget').init();
});
