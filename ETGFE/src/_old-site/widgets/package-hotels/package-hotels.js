import('../../plugins/star-rating/star-rating.js').then(({ default: StarRating }) => {
  window.StarRating = StarRating
  Promise.all([
    import(/* webpackMode: "eager" */ './package-hotels.scss'),
    import('../../plugins/semantic/accordion.css'),
    import('../../plugins/semantic/accordion.js'),
    import('../../plugins/semantic/rating.css'),
    import('../../plugins/semantic/rating.js'),
    import('../../plugins/star-rating/star-rating.css'),
  ]).then(() => {
    $('.widget.package-hotels').each(function (i, el) {
      $(el).data('widget', new PackageHotels(el))
      $(el).data('widget').init()
    })
  })
})

function PackageHotels (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PackageHotels init', self);
    }
    self.el.css('opacity', 1);

    self.el.find('.stars').each(function(i, el){
      $(el).starRating({
        initialRating: parseFloat($(el).attr('data-rating')),
        // strokeWidth: 45,
        strokeColor: '#fff',
        useGradient: false,
        readOnly: true,
        activeColor: '#ca568e',
        starSize: 20,
        starShape: 'rounded',
        emptyColor: '#E0E0E0',
      });
    });



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


