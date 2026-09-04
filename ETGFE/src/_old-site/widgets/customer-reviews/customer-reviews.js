import { log } from '../../plugins/handlebars/handlebars-v4.0.11'

import('../../plugins/star-rating/star-rating.js').then(({ default: StarRating }) => {
  window.StarRating = StarRating

  import('dayjs').then(({ default: dayjs }) => {
    const relativeTime = require('dayjs/plugin/relativeTime')
    dayjs.extend(relativeTime) // See: https://day.js.org/docs/en/plugin/relative-time

    import('../../plugins/swiper/swiper.js').then(({ default: Swiper }) => {
      Promise.all([
        import(/* webpackMode: "eager" */ './customer-reviews.scss'),
        import('../../plugins/swiper/swiper.css'),
        import('../../plugins/semantic/rating.css'),
        import('../../plugins/semantic/rating.js'),
        import('../../plugins/star-rating/star-rating.css'),
      ]).then(() => {
        $('.widget.customer-reviews').each(function (i, el) {
          $(el).data('widget', new CustomerReviews(el, dayjs, Swiper))
          $(el).data('widget').init()
        })
      })
    })
  })
})

function CustomerReviews (el, dayjs, Swiper) {
  const self = this
  self.el = $(el)
  self.endpoint = $(self.el).attr('data-endpoint')
  self.endpointMethod = $(self.el).attr('data-endpoint-method')
  self.averageRatings = self.el.find('.average-ratings')
  self.reviewsList = self.el.find('.ratings-list')
  self.swiperContainer = self.el.find('.swiper-container')
  self.swiperContents = self.el.find('.swiper-wrapper')
  self.reviewSlideTemplate = require('./__review-slide.hbs')

  self.renderAverageTotalRatings = function (totals) {
    self.averageRatings.find('.total-reviews').html('').html(totals.review_count)
    self.averageRatings.find('.average-rating').html('').html(totals.average_rating)
    self.averageRatings.find('.stars').attr('data-rating', totals.average_rating)

    self.renderReviewStars(self.averageRatings)

    self.averageRatings.addClass('is-loaded')
  }

  self.initSwiper = function (container) {
    self.swiper = new Swiper(container, {
      autoHeight: true,
      slidesPerView: 3,
      slidesPerGroup: 3,
      breakpoints: {
        767: {
          // max-width: 767px
          slidesPerView: 1,
          slidesPerGroup: 1,
        },
        1023: {
          // max-width: 1023px
          slidesPerView: 2,
          slidesPerGroup: 2,
        },
      },
      on: {
        init: function () {
          self.reviewsList.addClass('beginning')
        },
        reachBeginning: function () {
          self.reviewsList.addClass('beginning')
        },
        reachEnd: function () {
          self.reviewsList.addClass('end')
        },
        fromEdge: function () {
          self.reviewsList.removeClass('beginning end')
        }
      }
    })

    self.el.on('click', '.next', function () {
      self.swiper.slideNext()
    })

    self.el.on('click', '.prev', function () {
      self.swiper.slidePrev()
    })

    self.truncateReviewText() // must run after slider init
  }

  self.truncateReviewText = function () {
    self.el.find('.review-text').each(function (i, el) {
      // NOTE height of the element is defined in CSS.
      if (el.scrollHeight > el.clientHeight) {
        const a = document.createElement('a')
        a.setAttribute('href', '#')
        a.classList.add('read-more')
        a.innerHTML += 'Read more'
        el.appendChild(a)

        el.classList.add('truncate')

        $(el).on('click', function (event) {
          event.preventDefault()

          if (!event.target.classList.contains('read-more')) return

          if (el.classList.contains('show')) {
            el.classList.remove('show')
            event.target.innerHTML = 'Read more'
          } else {
            el.classList.add('show')
            event.target.innerHTML = 'Read less'
          }

          // NOTE setTimeout delay should equal or greater that the CSS animation duration.
          setTimeout(_ => self.swiper.updateAutoHeight(300), 60)
        })
      }
    })
  }

  self.renderReviews = function (reviews) {
    self.swiperContents.html('')

    for (let entry = 0; entry < reviews.length; entry++) {
      let review = reviews[entry]
      // add formatted date
      review.dateFromNow = dayjs(review.dateTime).fromNow()

      let reviewSlide = self.reviewSlideTemplate(review)
      self.swiperContents.append(reviewSlide)
    }

    self.renderReviewStars(self.reviewsList)
    self.initSwiper(self.swiperContainer)

    self.reviewsList.addClass('is-loaded')
  }

  self.renderReviewStars = function (element) {
    $(element).find('.stars').each(function (i, el) {
      $(el).starRating({
        initialRating: parseFloat($(el).attr('data-rating')),
        readOnly: true,
        starShape: 'rounded',
        starSize: 17,
        strokeWidth: 0,
        useGradient: false,
        strokeColor: '#fff',
        activeColor: '#FDB101',
        emptyColor: '#E0E0E0',
      })
    })
  }

  self.getReviews = function () {
    return new Promise((resolve, reject) => {
      $.ajax({
        url: self.endpoint,
        type: self.endpointMethod,
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
          resolve(data)
        },
        error: function (error) {
          reject(error)
        },
      })
    })
  }

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('OurReviews init', self)
    }

    self.el.css('opacity', 1)

    self.getReviews().then(function (data) {
      self.renderReviews(data.reviews)
      self.renderAverageTotalRatings(data.totals)
    }).catch((error) => {
      console.log(error)
    })
  }
}
