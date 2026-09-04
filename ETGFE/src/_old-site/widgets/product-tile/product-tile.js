import(/* webpackMode: "eager" */ './product-tile.scss');

function ProductTile (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      // console.log('ProductTile init', self);
    }
    self.el.css('opacity', 1);

  }
}

$('.widget.product-tile').each(function(i, el){
  $(el).data('widget', new ProductTile(el));
  $(el).data('widget').init();
});
