import('../../plugins/swiper/swiper.js').then(({ default: Swiper }) => {
  Promise.all([
    import('../../plugins/swiper/swiper.css'),
    import(/* webpackMode: "eager" */ './homepage-hero-al-search.scss'),
    import('../../plugins/jquery-ui/custom'),
    import('../../plugins/semantic/search'),
    import('../../plugins/semantic/search.css'),
    import('../../plugins/semantic/dropdown'),
    import('../../plugins/semantic/dropdown.css'),
    import('../../plugins/semantic/label.css')
  ]).then(() => {
    $('.widget.homepage-hero-search').each(function (i, el) {
      $(el).data('widget', new HomepageHeroSearch(el, Swiper))
      $(el).data('widget').init()
    })
  })
})

import { menuSelect } from 'instantsearch.js/es/widgets'

function HomepageHeroSearch (el, Swiper) {
  const self = this
  self.el = $(el)

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('HomepageHeroSearch init', self)
    }
    self.el.css('opacity', 1)

    self.searchClient = algoliasearch('SK049MNMX7', '79d4022f9d2199e9dcc43221a5171c43')

    self.tourSearch = instantsearch({
      indexName: $(el).attr('data-tour-indexname'),
      searchClient: self.searchClient,
      searchFunction: function (helper) {
        helper.search()

      }
    })

// 3. Instantiate
    self.tourSearch.addWidgets([
      menuSelect({
        container: '#destinationMenu',
        attribute: 'DestinationNameList',
        limit: 100,
        showMoreLimit: 500,
        templates: {
          defaultOption: 'All Destinations',
        },
      }),
      menuSelect({
        container: '#experienceMenu',
        attribute: 'ExperienceNameList',
        limit: 100,
        showMoreLimit: 500,
        templates: {
          defaultOption: 'All Experiences',
        },
      }),
      menuSelect({
        container: '#travelstyleMenu',
        attribute: 'TravelStyles',
        limit: 100,
        showMoreLimit: 500,
        templates: {
          defaultOption: 'All Travel Styles',
        },
      }),

    ])
    self.tourSearch.start()
    setTimeout(function () {
      self.el.find('.search-fields select').addClass('ui fluid dropdown').dropdown({
        placeholder: false,
        direction: 'downwards'
      })

    }, 1000)
    self.getSearchQuery = function (queryName, queryValue) {
      if (queryValue == null || queryValue == '') {
        return ''
      }

      return `&${queryName}=${encodeURI(queryValue).replace(/%/g, '%25')}`
    }
    self.el.find('.search-fields select').addClass('make-dropdown')
    self.el.find('.search-button').click(function (e) {
      e.preventDefault()
      var searchUrl = self.el.attr('data-search-url')

      var destination = self.el.find('#destinationMenu select').val()
      var experience = self.el.find('#experienceMenu select').val()
      var travelstyle = self.el.find('#travelstyleMenu select').val()

      if (destination == '' && experience == '' && travelstyle == '') {
        location = searchUrl
        return
      }
      searchUrl += '?'
      searchUrl += self.getSearchQuery('destination', destination)
      searchUrl += self.getSearchQuery('experiences', experience)
      searchUrl += self.getSearchQuery('travelstyles', travelstyle)

      location = searchUrl

    })

    self.swiper = new Swiper(self.el.find('.bg .swiper-container'), {
      autoplay: {
        delay: 3000,
        disableOnInteraction: false
      },
      effect: 'fade',
      speed: 1500,
      loop: true,
      spaceBetween: 0
    })

    self.el.find('.arrow').click(function (event) {
      event.preventDefault()

      $('html, body').animate({
        scrollTop: $(this).parents('.homepage-hero').find('.bottom').offset().top - 80
      }, 1000)
    })

    var slideWidth = 0
    var windowWidth = $(window).outerWidth() - 260

    self.el.find('.experience-list .swiper-slide').each(function (i, el) {
      slideWidth += $(el).outerWidth()
    })

    self.el.find('.list').css('width', slideWidth)

    if (slideWidth > windowWidth) {
      setTimeout(function () {
        self.swiperList = new Swiper(self.el.find('.experience-list .swiper-container'), {
          autoplay: {
            delay: 2000,
            disableOnInteraction: false
          },
          speed: 1500,
          loop: true,
          slidesPerView: 'auto',
          spaceBetween: 0
        })

      }, 1000)
    }

  }
}
