import('dayjs').then(({ default: dayjs }) => {
  import('../../plugins/swiper/swiper.js').then(({ default: Swiper }) => {
    Promise.all([
      import('../../plugins/swiper/swiper.css'),
      import(/* webpackMode: "eager" */ './homepage-hero-search.scss'),
      import('../../plugins/jquery-ui/custom'),
      import('../../plugins/semantic/search'),
      import('../../plugins/semantic/search.css'),
      import('../../plugins/semantic/dropdown'),
      import('../../plugins/semantic/dropdown.css'),
      import('../../plugins/semantic/label.css')
    ]).then(() => {
      $('.widget.homepage-hero-search').each(function (i, el) {
        $(el).data('widget', new HomepageHeroSearch(el, dayjs, Swiper))
        $(el).data('widget').init()
      })
    })
  })
})

function HomepageHeroSearch (el, dayjs, Swiper) {
  const self = this
  self.el = $(el)

  self.endpoints = {
    destinations: $(self.el).find('.destinations-endpoint').attr('data-endpoint'),
    experiences: $(self.el).find('.experiences-endpoint').attr('data-endpoint'),
    travelStyles: $(self.el).find('.travel-styles-endpoint').attr('data-endpoint'),
    startLocations: $(self.el).find('.start-locations-endpoint').attr('data-endpoint'),
    endLocations: $(self.el).find('.end-locations-endpoint').attr('data-endpoint')
  }

  self.selectedValues = {
    destinations: null,
    experiences: null,
    travelStyles: null,
    startDate: null,
    endDate: null,
    startLocations: null,
    endLocations: null
  }

  self.getDestinations = function () {
    $(self.el).find('.destination-dropdown').addClass('loading disabled')
    $.ajax({
      url: self.endpoints.destinations,
      type: 'POST',
      data: JSON.stringify({
        experiences: self.selectedValues.experiences,
        travelStyles: self.selectedValues.travelStyles,
        startLocations: self.selectedValues.startLocations,
        endLocations: self.selectedValues.endLocations,
      }),
      dataType: 'json',
      contentType: 'application/json'
    })
      .done(function (response) {
        $(self.el).find('.destination-dropdown .menu').html('<div class="item" data-value="">All destinations</div>')
        for (var i = 0; i < response.length; i++) {
          var item = response[i]
          $(self.el).find('.destination-dropdown .menu').append('<div class="item" data-value="' + item.value + '">' + item.label + '</div>')
        }
        $(self.el).find('.destination-dropdown').dropdown('refresh')
      })
      .fail(function () {
        console.log('fail')
      })
      .always(function () {
        $(self.el).find('.destination-dropdown').removeClass('loading disabled')
      })
  }

  self.getExperiences = function () {
    $(self.el).find('.experience-dropdown').addClass('loading disabled')
    $.ajax({
      url: self.endpoints.experiences,
      type: 'POST',
      data: JSON.stringify({
        destinations: self.selectedValues.destinations,
        travelStyles: self.selectedValues.travelStyles,
        startLocations: self.selectedValues.startLocations,
        endLocations: self.selectedValues.endLocations,
      }),
      dataType: 'json',
      contentType: 'application/json'
    })
      .done(function (response) {
        $(self.el).find('.experience-dropdown .menu').html('<div class="item" data-value="">All experiences</div>')
        for (var i = 0; i < response.length; i++) {
          var item = response[i]
          $(self.el).find('.experience-dropdown .menu').append('<div class="item" data-value="' + item.value + '"><span class="icon icon-' + item.icon + '"></span>' + item.label + '</div>')
        }
        $(self.el).find('.experience-dropdown').dropdown('refresh')
      })
      .fail(function () {
        console.log('fail')
      })
      .always(function () {
        $(self.el).find('.experience-dropdown').removeClass('loading disabled')
      })
  }

  self.getTravelStyles = function () {
    $(self.el).find('.travel-styles-dropdown').addClass('loading disabled')
    $.ajax({
      url: self.endpoints.travelStyles,
      type: 'POST',
      data: JSON.stringify({
        destinations: self.selectedValues.destinations,
        experiences: self.selectedValues.experiences,
        startLocations: self.selectedValues.startLocations,
        endLocations: self.selectedValues.endLocations,
      }),
      dataType: 'json',
      contentType: 'application/json'
    })
      .done(function (response) {
        $(self.el).find('.travel-styles-dropdown .menu').html('<div class="item" data-value="">All travel styles</div>')
        for (var i = 0; i < response.length; i++) {
          var item = response[i]
          $(self.el).find('.travel-styles-dropdown .menu').append('<div class="item" data-value="' + item.value + '">' + item.label + '</div>')
        }
        $(self.el).find('.travel-styles-dropdown').dropdown('refresh')
      })
      .fail(function () {
        console.log('fail')
      })
      .always(function () {
        $(self.el).find('.travel-styles-dropdown').removeClass('loading disabled')
      })
  }

  self.getStartLocations = function () {
    $(self.el).find('.start-location-dropdown').addClass('loading disabled')
    $.ajax({
      url: self.endpoints.startLocations,
      type: 'POST',
      data: JSON.stringify({
        destinations: self.selectedValues.destinations,
        experiences: self.selectedValues.experiences,
        travelStyles: self.selectedValues.travelStyles,
        endLocations: self.selectedValues.endLocations,
      }),
      dataType: 'json',
      contentType: 'application/json'
    })
      .done(function (response) {
        $(self.el).find('.start-location-dropdown .menu').html('<div class="item" data-value="">Select desired start location</div>')
        for (var i = 0; i < response.length; i++) {
          var item = response[i]
          $(self.el).find('.start-location-dropdown .menu').append('<div class="item" data-value="' + item.value + '">' + item.label + '</div>')
        }
        $(self.el).find('.start-location-dropdown').dropdown('refresh')
      })
      .fail(function () {
        console.log('fail')
      })
      .always(function () {
        $(self.el).find('.start-location-dropdown').removeClass('loading disabled')
      })
  }

  self.getEndLocations = function () {
    $(self.el).find('.end-location-dropdown').addClass('loading disabled')
    $.ajax({
      url: self.endpoints.endLocations,
      type: 'POST',
      data: JSON.stringify({
        destinations: self.selectedValues.destinations,
        experiences: self.selectedValues.experiences,
        travelStyles: self.selectedValues.travelStyles,
        startLocations: self.selectedValues.startLocations,
      }),
      dataType: 'json',
      contentType: 'application/json'
    })
      .done(function (response) {
        $(self.el).find('.end-location-dropdown .menu').html('<div class="item" data-value="">Select desired end location</div>')
        for (var i = 0; i < response.length; i++) {
          var item = response[i]
          $(self.el).find('.end-location-dropdown .menu').append('<div class="item" data-value="' + item.value + '">' + item.label + '</div>')
        }
        $(self.el).find('.end-location-dropdown').dropdown('refresh')
      })
      .fail(function () {
        console.log('fail')
      })
      .always(function () {
        $(self.el).find('.end-location-dropdown').removeClass('loading disabled')
      })
  }

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('HomepageHeroSearch init', self)
    }
    self.el.css('opacity', 1)

    self.getDestinations()
    self.getExperiences()
    self.getTravelStyles()
    self.getStartLocations()
    self.getEndLocations()

    self.el.find('.ui.destination-dropdown').dropdown({
      forceSelection: false,
      clearable: true,
      onChange: function (value) {
        self.el.find('.ui.destination-dropdown input').blur()
        self.selectedValues.destinations = value
        self.getExperiences()
        self.getTravelStyles()
        self.getStartLocations()
        self.getEndLocations()
      }
    })
    self.el.find('.ui.experience-dropdown').dropdown({
      forceSelection: false,
      clearable: true,
      onChange: function (value) {
        self.el.find('.ui.experience-dropdown input').blur()
        self.selectedValues.experiences = value
        self.getDestinations()
        self.getTravelStyles()
        self.getStartLocations()
        self.getEndLocations()
      }
    })
    self.el.find('.ui.travel-styles-dropdown').dropdown({
      forceSelection: false,
      clearable: true,
      onChange: function (value) {
        self.el.find('.ui.travel-styles-dropdown input').blur()
        self.selectedValues.travelStyles = value
        self.getDestinations()
        self.getExperiences()
        self.getStartLocations()
        self.getEndLocations()
      }
    })
    self.el.find('.ui.start-location-dropdown').dropdown({
      forceSelection: false,
      clearable: true,
      onChange: function (value) {
        self.el.find('.ui.start-location-dropdown input').blur()
        self.selectedValues.startLocations = value
        self.getDestinations()
        self.getExperiences()
        self.getTravelStyles()
        self.getEndLocations()
      }
    })
    self.el.find('.ui.end-location-dropdown').dropdown({
      forceSelection: false,
      clearable: true,
      onChange: function (value) {
        self.el.find('.ui.end-location-dropdown input').blur()
        self.selectedValues.endLocations = value
        self.getDestinations()
        self.getExperiences()
        self.getTravelStyles()
        self.getStartLocations()
      }
    })
    self.el.find('.calendar-start').datepicker({
      minDate: new Date(),
      dateFormat: 'dd/mm/yy',
      showOtherMonths: true,
      selectOtherMonths: true,
      firstDay: 1
    })

    self.el.find('.calendar-end').datepicker({
      minDate: dayjs().add(3, 'week').toDate(),
      dateFormat: 'dd/mm/yy',
      showOtherMonths: true,
      selectOtherMonths: true,
      firstDay: 1
    })
    self.el.find('.calendar-start').change(function () {
      self.selectedValues.startDate = self.el.find('.calendar-start').datepicker('getDate')
      self.el.find('.calendar-end').datepicker('option', 'minDate', dayjs(self.el.find('.calendar-start').datepicker('getDate')).add(3, 'week').toDate())
    })
    self.el.find('.calendar-end').change(function () {
      self.selectedValues.endDate = self.el.find('.calendar-end').datepicker('getDate')
    })

    self.el.find('.toggle-advanced-search').click(function (e) {
      e.preventDefault()
      if (self.el.find('.search-fields').hasClass('active')) {
        self.el.find('.advanced-search').slideUp(250, function () {
          self.el.find('.search-fields').removeClass('active')

          // clear advanced fields
          self.el.find('.calendar-end').datepicker('setDate', null)
          self.el.find('.calendar-start').datepicker('setDate', null)
          self.el.find('.ui.start-location-dropdown').dropdown('clear')
          self.el.find('.ui.end-location-dropdown').dropdown('clear')
        })
      } else {
        self.el.find('.search-fields').addClass('active')
        self.el.find('.advanced-search').slideDown(250)
      }
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
