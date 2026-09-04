import { dom } from '@fortawesome/fontawesome-svg-core'

Promise.all([
  import(/* webpackMode: "eager" */ './photo-gallery.scss'),
  import('../../plugins/slick/slick.css'),
  import('../../plugins/slick/slick'),
  import('../../plugins/slick/slick-theme.css'),
]).then(() => {
  $('.widget.photo-gallery').each(function (i, el) {
    $(el).data('widget', new PhotoGallery(el))
    $(el).data('widget').init()
  })
})

function PhotoGallery (el) {
  const self = this;
  self.el = $(el);

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('PhotoGallery init', self);
    }
    self.el.css('opacity', 1);

    self.el.find('.main-carousel').slick({
      dots: false,
      speed: 500,
      arrows: true,
      infinite: false,
      slidesToShow: 1,
      slidesToScroll: 1,
      centerMode: false,
      adaptiveHeight: true,
      prevArrow: '<div class="prev"><span class="icon fas fa-chevron-left"></span></div>',
      nextArrow: '<div class="next"><span class="icon fas fa-chevron-right"></span></div>'
    });

    self.el.find('.thumbnail-carousel').slick({
      dots: false,
      speed: 500,
      arrows: true,
      infinite: false,
      slidesToShow: 5,
      slidesToScroll: 5,
      centerMode: false,
      prevArrow: '<div class="prev"><span class="icon fas fa-chevron-left"></span></div>',
      nextArrow: '<div class="next"><span class="icon fas fa-chevron-right"></span></div>',
      responsive: [
        {
          breakpoint: 1220,
          settings: {
            slidesToShow: 4,
            slidesToScroll: 4
          }
        },
        {
          breakpoint: 1024,
          settings: {
            slidesToShow: 3,
            slidesToScroll: 3
          }
        },
        {
          breakpoint: 600,
          settings: {
            slidesToShow: 2,
            slidesToScroll: 2
          }
        }
      ]
    });

    self.el.find('.main-carousel').on('beforeChange', function(event, slick, currentSlide, nextSlide){
      console.log(nextSlide);
      $(el).find('.thumbnail-carousel').slick('slickGoTo', nextSlide);
      $(el).find('.thumbnail-carousel .item').removeClass('active');
      $(el).find('.thumbnail-carousel .item').eq(nextSlide).addClass('active');
      $('video').trigger('pause');
      setTimeout(function(){
        $('.main-carousel .item video').css('width', $('.main-carousel .item').width());
      }, 500);
    });

    $('body').on('click', '.thumbnail-carousel .slick-slide', function(e){
      console.log('thumbnail-carousel click', $(this).index());
      e.preventDefault();
      self.el.find('.main-carousel').slick('slickGoTo', $(this).index())
    });

    dom.i2svg();

  }
}


