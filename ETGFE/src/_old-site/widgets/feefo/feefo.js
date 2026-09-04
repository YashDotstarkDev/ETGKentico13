import(/* webpackMode: "eager" */ './feefo.scss');

function Feefo (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('Feefo init', self);
    }
    self.el.css('opacity', 1);

    setTimeout(function () {
      //https://api.feefo.com/api/javascript/entire-travel
      var script = document.createElement('script');
      script.type = 'text/javascript';
      script.src = 'https://api.feefo.com/api/javascript/entire-travel';

      document.getElementsByTagName('head')[0].appendChild(script);
    }, 5000);
  }
}

$('.widget.feefo').each(function(i, el){
  $(el).data('widget', new Feefo(el));
  $(el).data('widget').init();
});
