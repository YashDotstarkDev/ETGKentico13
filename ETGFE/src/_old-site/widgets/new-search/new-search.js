import {liteClient} from 'algoliasearch/lite'
import instantsearch from 'instantsearch.js'

import historyRouter from 'instantsearch.js/es/lib/routers/history'

import('dayjs').then(({ default: dayjs }) => {
  import('../../plugins/numeraljs/numeral.min.js').then(({ default: numeral }) => {
    window.numeral = numeral
    import('../../plugins/swiper/swiper.js').then(({ default: Swiper }) => {
      Promise.all([
        import(/* webpackMode: "eager" */ './new-search.scss'),
        import('../../plugins/swiper/swiper.css'),
        import('../../plugins/semantic/icon.css'),
        import('../../plugins/semantic/transition.css'),
        import('../../plugins/semantic/transition.js'),
        import('../../plugins/jquery-ui/custom'),
      ]).then(() => {
        $('.widget.new-search').each(function (i, el) {
          $(el).data('widget', new NewSearch(el, instantsearch, dayjs, Swiper))
          $(el).data('widget').init()
        })
      })
    })
  })
})

import { hits, rangeSlider, rangeInput, refinementList, toggleRefinement, configure, hitsPerPage, sortBy, stats, pagination } from 'instantsearch.js/es/widgets'
import { connectRange, connectHits, connectRefinementList, connectSearchBox } from 'instantsearch.js/es/connectors'

function NewSearch (el, instantsearch, dayjs, Swiper) {
  const self = this
  self.el = $(el)
  self.initialLoad = true
  self.searchClient = liteClient('SK049MNMX7', '79d4022f9d2199e9dcc43221a5171c43')
  self.search = null
  self.searchIndex = self.el.attr('data-index')
  self.dateRange = null
  self.departureItems = []
  self.timerId = null
  self.currencyCode = "AUD";
  self.currencySymbol = "$";
  self.conversionRate = 1;
  self.currencyDiscountApplicable = true;
  const filterButton = $('.new-search .filter-button')
  const filterPanel = $('.new-search .filter-panel')

  function showFilters() {
    filterButton.addClass('active')
    filterPanel.addClass('active')
  }

  function hideFilters() {
    filterButton.removeClass('active')
    filterPanel.removeClass('active')
  }

  self.keywordRender = (renderOptions, isFirstRender) => {
    const { query, refine } = renderOptions
    const container = document.querySelector('#searchbox')
    if (isFirstRender) {
      const input = document.createElement('input')
      input.placeholder = 'Start typing'
      input.addEventListener('input', event => {
        refine(event.target.value)
      })
      container.appendChild(input)
    }
    container.querySelector('input').value = query
  }

  self.keywordSearch = connectSearchBox(
    self.keywordRender
  )

  self.destinationRefinementListRender = (renderOptions, isFirstRender) => {
    const { items } = renderOptions

    self.el.find('#destination .ui.dropdown').dropdown({
      placeholder: 'Destination',
      fullTextSearch: true,
      sortSelect: true,
      values: items.map(function (item) {
        return {
          name: item.label,
          value: item.label,
          selected: item.isRefined
        }
      })
    })

    var minChars = 1
    if ($(window).width() < 1024) {
      minChars = 0
    }

    self.el.find('#destination .ui.dropdown').dropdown({
      minCharacters: minChars,
      placeholder: 'Destination',
      fullTextSearch: true,
      sortSelect: true,
      onShow: function () {
        $('.trigger.destination').removeClass('active')
      },
      onChange: function (value, text, $selectedItem) {

        self.setInstantSearchUiState({
          refinementList: {
            ...self.getInstantSearchUiState().refinementList,
            destination: value ? value.split(',') : []
          }
        })
      },
    })

  }

  self.destinationRefinementList = connectRefinementList(
    self.destinationRefinementListRender
  )

  self.departureRefinementListRender = (renderOptions, isFirstRender) => {
    const { items, refine } = renderOptions
    self.departureItems = items

    self.handleRender()
  }

  self.departureRefinementList = connectRefinementList(
    self.departureRefinementListRender
  )

  self.virtualRange = connectRange(function (renderOptions, isFirstRender) {

  })

  self.virtualDepartureRefinement = connectRefinementList(function (renderOptions, isFirstRender) {

  })

  self.handleRender = function (helper) {

    var priceMax = self.el.find('#priceSlider .rheostat-handle-upper .rheostat-tooltip').html()
    var priceMin = self.el.find('#priceSlider .rheostat-handle-lower .rheostat-tooltip').html()
    self.el.find('.price-range .from-value').html(numeral(priceMin).format('$0,0'))
    self.el.find('.price-range .to-value').html(numeral(priceMax).format('$0,0'))

    var durationMax = self.el.find('#durationSlider .rheostat-handle-upper .rheostat-tooltip').html()
    var durationMin = self.el.find('#durationSlider .rheostat-handle-lower .rheostat-tooltip').html()
    self.el.find('.duration-range .from-value').html(durationMin + ' days')
    self.el.find('.duration-range .to-value').html(durationMax + ' days')

    self.el.find('.tour-tiles .tour').each(function (i, el) {
      $(el).data(
        'swiper',
        new Swiper($(el).find('.swiper'), {
          loop: true,
          spaceBetween: 0,
          slidesPerView: 1
        })
      )

      $(el).on('click', '.next', function () {
        $(el).data('swiper').slideNext()
      })

      $(el).on('click', '.prev', function () {
        $(el).data('swiper').slidePrev()
      })
    })

    setTimeout(function () {
      self.searchPageState = self.getInstantSearchUiState()

      let filterCount = 0

      filterCount += self.searchPageState.range && self.searchPageState.range.price ? 1 : 0
      filterCount += self.searchPageState.range && self.searchPageState.range.duration ? 1 : 0

      filterCount += self.searchPageState.refinementList && self.searchPageState.refinementList.travelStyles ? self.searchPageState.refinementList.travelStyles.length : 0
      filterCount += self.searchPageState.refinementList && self.searchPageState.refinementList.experiences ? self.searchPageState.refinementList.experiences.length : 0

      filterCount += self.searchPageState.toggle && self.searchPageState.toggle['options.bookNow'] ? 1 : 0
      filterCount += self.searchPageState.toggle && self.searchPageState.toggle['options.freedomOfChoice'] ? 1 : 0
      filterCount += self.searchPageState.toggle && self.searchPageState.toggle['options.onSale'] ? 1 : 0
      filterCount += self.searchPageState.toggle && self.searchPageState.toggle['options.peaceOfMind'] ? 1 : 0
      filterCount += self.searchPageState.toggle && self.searchPageState.toggle['options.safeTravels'] ? 1 : 0

      $('.filter-button .count').html(filterCount.toString())

      if (filterCount > 0) {
        $('.filter-button .count').addClass('active')
      } else {
        $('.filter-button .count').removeClass('active')
      }
    }, 200)

    var numberOfMonths = 2
    if ($(window).width() < 1024) {
      numberOfMonths = 1
    }

    try {
      $('#departureDate').datepicker('destroy')
    } catch (error) {
      // nothing
    }

    $('#departureDate').datepicker({
      numberOfMonths: numberOfMonths,
      beforeShowDay: function (date) {

        let dayAvailable = false
        for (let index = 0; index < self.departureItems.length; index++) {
          const item = self.departureItems[index]
          let dateUnix = parseInt(item.value)
          if (dateUnix === dayjs(date).unix()) {
            dayAvailable = true

            break
          }
        }

        if (self.selectedDate && dayAvailable) {
          if (self.dateRange > 0) {
            if (dayjs(date).isSame(self.selectedDate, 'day')) {
              return [dayAvailable, 'selected-date']
            }
            if (dayjs(date).isBefore(self.selectedDate.add(self.dateRange, 'days'), 'day') && dayjs(date).isAfter(self.selectedDate, 'day')) {
              return [dayAvailable, 'adjacent-date']
            }
            if (dayjs(date).isSame(self.selectedDate.add(self.dateRange, 'days'), 'day')) {
              return [dayAvailable, 'adjacent-date']
            }
            if (dayjs(date).isAfter(self.selectedDate.subtract(self.dateRange, 'days'), 'day') && dayjs(date).isBefore(self.selectedDate, 'day')) {
              return [dayAvailable, 'adjacent-date']
            }
            if (dayjs(date).isSame(self.selectedDate.subtract(self.dateRange, 'days'), 'day')) {
              return [dayAvailable, 'adjacent-date']
            }
          }

          if (dayjs(date).isSame(self.selectedDate, 'day')) {
            return [dayAvailable, 'selected-date']
          }

          return [dayAvailable, '', null]
        } else {
          return [dayAvailable, '', null]
        }
      },
      onSelect: function (dateText, inst) {
        self.selectedDate = dayjs(dateText)
        var departureRefinement = []
        var startDate = dayjs(dateText).startOf('day').subtract(self.dateRange, 'days').startOf('day')
        var endDate = dayjs(dateText).startOf('day').add(self.dateRange, 'days').startOf('day')
        departureRefinement.push(startDate.unix())
        for (let index = startDate.unix() + 86400; index <= endDate.unix(); index += 86400) {
          departureRefinement.push(index)
        }
        self.setInstantSearchUiState({
          refinementList: {
            ...self.getInstantSearchUiState().refinementList,
            departureDate: departureRefinement
          }
        })

        $('.trigger.date .value').html(self.selectedDate.format('MMM D, YYYY')).removeClass('default')
        if (self.dateRange > 0) {
          $('.trigger.date .value').append(`
            <div class="plusminus">
              <svg width="9" height="13" viewBox="0 0 9 13" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path d="M8.25 11H0.75C0.328125 11 0 11.3516 0 11.75C0 12.1719 0.328125 12.5 0.75 12.5H8.25C8.64844 12.5 9 12.1484 9 11.75C9 11.375 8.64844 11 8.25 11ZM1.125 5.375H3.75V8C3.75 8.39844 4.07812 8.72656 4.5 8.72656C4.89844 8.72656 5.25 8.375 5.25 8V5.375H7.875C8.27344 5.375 8.625 5.04688 8.625 4.625C8.625 4.22656 8.27344 3.875 7.875 3.875H5.25V1.25C5.25 0.851562 4.89844 0.5 4.5 0.5C4.07812 0.5 3.75 0.851562 3.75 1.27344V3.89844H1.125C0.703125 3.89844 0.375 4.22656 0.375 4.625C0.375 5.04688 0.703125 5.375 1.125 5.375Z" fill="currentColor"/>
              </svg>
              ${self.dateRange}
            </div>
          `)
        }

        if ($(window).width() < 1024) {
          $('.trigger').removeClass('active')
        }

      },
      minDate: dayjs().add(1, 'days').toDate(),
    })

    if (self.selectedDate) {
      $('#departureDate').datepicker('setDate', self.selectedDate.toDate())
    }
  }

  self.initFilterBox = function () {

  }

  self.departureCalendarRendered = false
  self.departureCalendarSubmitted = false

  self.renderHits = function (renderOptions, isFirstRender) {
    const { hits } = renderOptions

    if (hits.length) {
      self.el.removeClass('no-results')
    } else {
      self.el.addClass('no-results')
    }

    document.querySelector('#hits').innerHTML = `
      <div class="tour-tiles">
        ${hits
      .map(function (item) {
        
        const priceType = item.priceType ? item.priceType : 'Per Person twin share';
        const buttonMarkup = item.thumbnails.length <= 1 ? `` : `
          <button type="button" class="prev">
            <svg width="8" height="14" viewBox="0 0 8 14" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M6.75008 0.685057C6.99617 0.685057 7.21492 0.767089 7.37899 0.931151C7.73445 1.25928 7.73445 1.8335 7.37899 2.16162L2.75789 6.81006L7.37898 11.4312C7.73445 11.7593 7.73445 12.3335 7.37898 12.6616C7.05086 13.0171 6.47664 13.0171 6.14852 12.6616L0.898516 7.41162C0.543047 7.08349 0.543047 6.50928 0.898516 6.18115L6.14852 0.931151C6.31258 0.767089 6.53133 0.685057 6.75008 0.685057Z" fill="#CA568E"/>
            </svg>
          </button>
          <button type="button" class="next">
            <svg width="8" height="14" viewBox="0 0 8 14" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M1.24992 13.3149C1.00383 13.3149 0.785079 13.2329 0.621017 13.0688C0.265548 12.7407 0.265548 12.1665 0.621017 11.8384L5.24211 7.18994L0.621017 2.56885C0.265548 2.24072 0.265548 1.6665 0.621017 1.33838C0.949142 0.98291 1.52336 0.98291 1.85149 1.33838L7.10149 6.58838C7.45695 6.9165 7.45695 7.49072 7.10149 7.81885L1.85149 13.0688C1.68742 13.2329 1.46867 13.3149 1.24992 13.3149Z" fill="#CA568E"/>
            </svg>
          </button>
        `
        
        return `

          <div class="tour">
            <div class="inner">
              ${(self.currencyDiscountApplicable || item.fromPrice === item.price  ) && item.promotionTitle && item.promotionTitle.length ? `<div class="promotion">${item.promotionTitle}</div>` : ``}
              <div class="image">
                <div class="carousel">
                  <div class="swiper">
                    <div class="swiper-wrapper">
                      ${item.thumbnails
          .map(function (thumbnail) {
            return `<div class="swiper-slide"><img class="lazyload" data-sizes="auto" data-src="${thumbnail}" /></div>`
          })
          .join('')}
                    </div>
                  </div>
                  ${buttonMarkup}
                </div>
              </div>

              <a class="copy" href="${item.url}">
                <div class="badges">
                  ${
          item.options.peaceOfMind
            ? `
                  <div class="badge">
                    <div class="icon">
                      <svg width="21" height="21" viewBox="0 0 21 21" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path d="M12.25 7.0625V5.96875C11.1172 4.52344 10.375 2.84375 10.1016 1.04688C10.0234 0.539062 9.35938 0.304688 9.04688 0.734375C8.1875 1.71094 7.5625 2.84375 7.13281 4.13281C8.46094 5.61719 10.2578 6.63281 12.25 7.0625ZM16.625 3C14.8672 3 13.5 4.40625 13.5 6.16406V8.46875C9.4375 8.23438 5.96094 5.73438 4.39844 2.14062C4.16406 1.67188 3.46094 1.63281 3.26562 2.10156C2.60156 3.46875 2.25 5.03125 2.25 6.63281C2.25 9.40625 3.57812 11.9844 5.57031 13.8984C6.07812 14.4062 6.58594 14.7969 7.09375 15.1875L1.46875 16.5938C1.03906 16.6719 0.84375 17.1797 1.07812 17.5312C1.78125 18.5859 3.46094 20.3828 7.05469 20.5C7.36719 20.5391 7.67969 20.4219 7.91406 20.2266L10.4922 18L13.5 18.0391C16.9375 18.0391 19.75 15.2266 19.75 11.7891V5.5L20.9609 3H16.625ZM16.625 6.78906C16.2734 6.78906 15.9609 6.47656 15.9609 6.16406C15.9609 5.8125 16.2734 5.53906 16.625 5.53906C16.9375 5.53906 17.2109 5.8125 17.2109 6.16406C17.25 6.47656 16.9375 6.78906 16.625 6.78906Z" fill="#929497" />
                      </svg>
                    </div>
                    <div class="text">Peace of Mind</div>
                  </div>
                  `
            : ``
        }

                  ${
          item.options.freedomOfChoice
            ? `
                  <div class="badge">
                    <div class="icon">
                      <svg width="18" height="19" viewBox="0 0 18 19" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path d="M2.125 0.75H7.94531C8.60938 0.75 9.23438 1.02344 9.70312 1.49219L16.5781 8.36719C17.5547 9.34375 17.5547 10.9453 16.5781 11.9219L11.3828 17.1172C10.4062 18.0938 8.80469 18.0938 7.82812 17.1172L0.953125 10.2422C0.484375 9.77344 0.25 9.14844 0.25 8.48438V2.625C0.25 1.60938 1.07031 0.75 2.125 0.75ZM4.625 6.375C5.28906 6.375 5.875 5.82812 5.875 5.125C5.875 4.46094 5.28906 3.875 4.625 3.875C3.92188 3.875 3.375 4.46094 3.375 5.125C3.375 5.82812 3.92188 6.375 4.625 6.375Z" fill="#929497" />
                      </svg>
                    </div>
                    <div class="text">Freedom of Choice</div>
                  </div>
                  `
            : ``
        }

                  ${
          item.options.exclusivePackages
            ? `
                  <div class="badge">
                    <div class="icon">
                      <svg width="22" height="20" viewBox="0 0 22 20" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path d="M16.2422 0.125H5.71484L11 6.61328L16.2422 0.125ZM22 7L17.3594 0.941406L12.418 7H22ZM4.59766 0.941406L0 7H9.53906L4.59766 0.941406ZM10.4844 19.1602C10.6133 19.332 10.7852 19.375 11 19.375C11.1719 19.375 11.3438 19.332 11.4727 19.1602L21.9141 8.375H0.0429688L10.4844 19.1602Z" fill="#929497" />
                      </svg>
                    </div>
                    <div class="text">Exclusive Packages</div>
                  </div>
                  `
            : ``
        }

                  ${
          item.options.safeTravels
            ? `
                  <div class="badge">
                    <div class="icon">
                      <svg width="10" height="23" viewBox="0 0 10 23" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path d="M6.95553 6.67116C6.92369 4.46295 5.55426 3.10609 3.40139 3.15265C1.27401 3.19921 -0.0444663 4.64253 0.000119689 6.8707C0.0447057 8.99244 1.54789 10.4823 3.57974 10.4225C5.65617 10.3626 6.98738 8.87937 6.94916 6.67116H6.95553Z" fill="#8CC53F" />
                        <path d="M0.930176 19.6079L1.53527 18.5437C3.08941 15.83 4.88559 13.2759 6.56712 10.642C8.31234 7.90835 8.49705 5.76665 7.18495 3.91095C5.87285 2.04195 4.25502 1.64953 0.981131 2.46763C0.974762 1.98209 0.974762 0.385791 0.974762 0.385791C0.968392 0.385791 1.42699 0.1663 2.38877 0.0665317C5.08941 -0.252728 7.33782 0.525468 8.87922 2.92657C10.4079 5.30771 10.2678 7.81523 9.08304 10.3094C8.87922 10.7418 8.5926 11.1342 8.33145 11.5399C5.99387 15.1715 3.30597 19.3618 0.968392 22.9934L0.930176 19.6012V19.6079Z" fill="#929497" />
                        <path d="M6.95553 6.67116C6.98738 8.87937 5.66254 10.3626 3.58611 10.4225C1.54789 10.4823 0.0447057 8.99244 0.000119717 6.8707C-0.0444663 4.64253 1.27401 3.19921 3.39502 3.15265C5.54789 3.10609 6.91732 4.46295 6.94916 6.67116H6.95553Z" fill="#929497" />
                      </svg>
                    </div>
                    <div class="text">Sustainable Travel</div>
                  </div>
                  `
            : ``
        }


                </div>

                <div class="destination">${item.destination}</div>
                <div class="title">${item.name}</div>
                ${item.startCity && item.endCity ? `<div class="cities">${item.startCity === item.endCity ? `Starts and ends in <span>${item.startCity}</span>` : `<span>${item.startCity}</span> to <span>${item.endCity}</span>`}</div>` : ``}
                <div class="description">${item.description}</div>


                <div class="bottom">
                <div class="stats">
                ${item.duration > 0 ? `
                  <div class="duration">${item.duration} days</div>
                ` : ``}

                  <div class="inclusions">
                  
                    ${
          item.inclusions.flights
            ? `
                    <div class="inclusion">
                    <svg width="23" height="21" viewBox="0 0 23 21" fill="none" xmlns="http://www.w3.org/2000/svg">
                                                <path d="M22.2031 9.17188C22.5156 9.52344 22.75 9.99219 22.75 10.5C22.75 11.5547 21.8906 12.375 21.1094 12.8438C20.2891 13.3125 19.2344 13.625 18.4531 13.625H14.5078L11.1484 19.5625C10.7969 20.1484 10.1719 20.5 9.50781 20.5H7.32031C6.46094 20.5 5.875 19.7188 6.10938 18.9375L7.63281 13.625H5.5625L4.03906 15.6562C3.80469 15.9688 3.45312 16.125 3.0625 16.125H1.42188C0.757812 16.125 0.25 15.6172 0.25 14.9531C0.25 14.875 0.25 14.7578 0.289062 14.6406L1.46094 10.5L0.289062 6.39844C0.25 6.28125 0.25 6.16406 0.25 6.04688C0.25 5.42188 0.757812 4.875 1.42188 4.875H3.0625C3.45312 4.875 3.80469 5.07031 4.03906 5.38281L5.5625 7.375H7.63281L6.10938 2.10156C5.875 1.32031 6.46094 0.5 7.32031 0.5H9.50781C10.1719 0.5 10.7969 0.890625 11.1484 1.47656L14.5078 7.375H18.4531C19.2344 7.375 20.2891 7.72656 21.1094 8.23438C21.5 8.46875 21.9297 8.78125 22.2031 9.17188ZM20.4453 9.28906C19.7812 8.89844 18.9609 8.625 18.4531 8.625H14.1562C13.9219 8.625 13.7266 8.50781 13.6094 8.3125L10.0547 2.10156C9.9375 1.90625 9.74219 1.75 9.50781 1.75H7.32031L9.03906 7.84375C9.11719 8.03906 9.07812 8.23438 8.96094 8.39062C8.84375 8.54688 8.64844 8.625 8.45312 8.625H5.25C5.01562 8.625 4.85938 8.54688 4.74219 8.39062L3.0625 6.125H1.5L2.71094 10.3438C2.75 10.4609 2.75 10.5781 2.71094 10.6953L1.5 14.875H3.0625L4.74219 12.6484C4.85938 12.4922 5.01562 12.375 5.25 12.375H8.45312C8.64844 12.375 8.84375 12.4922 8.96094 12.6484C9.07812 12.8047 9.11719 13 9.03906 13.1953L7.32031 19.25H9.50781C9.74219 19.25 9.9375 19.1328 10.0547 18.9375L13.6094 12.7266C13.7266 12.5312 13.9219 12.375 14.1562 12.375H18.4531C18.9609 12.375 19.7812 12.1797 20.4453 11.75C21.1484 11.3203 21.5 10.8906 21.5 10.5C21.5 10.3438 21.4219 10.1484 21.2266 9.95312C21.0703 9.71875 20.7969 9.48438 20.4453 9.28906ZM10.6016 1.78906L10.0547 2.10156L10.6016 1.78906Z" fill="black"></path>
                                                </svg>
                    <div class="text">Flights</div>
                    </div>
                    `
            : ``
        }



                  
                    ${
          item.inclusions.accommodation
            ? `
                    <div class="inclusion">
                    <svg width="20" height="19" viewBox="0 0 20 19" fill="none" xmlns="http://www.w3.org/2000/svg">
                      <path
                        d="M18.6136 8.97287V3.35C18.6136 1.99276 17.4891 0.868182 16.1318 0.868182H3.72273C2.3267 0.868182 1.24091 1.99276 1.24091 3.35V8.97287C0.465341 9.51577 0 10.4077 0 11.4159V17.6205C0 17.9695 0.271449 18.2409 0.620455 18.2409C0.930682 18.2409 1.24091 17.9695 1.24091 17.6205V15.7591H18.6136V17.6205C18.6136 17.9695 18.8851 18.2409 19.2341 18.2409C19.5443 18.2409 19.8545 17.9695 19.8545 17.6205V11.4159C19.8545 10.4077 19.3504 9.51577 18.6136 8.97287ZM16.7523 8.31364H10.5477V7.07273C10.5477 6.41349 11.0906 5.83182 11.7886 5.83182H16.1318C16.7911 5.83182 17.3727 6.41349 17.3727 7.07273V8.39119C17.1401 8.35241 16.9462 8.31364 16.7523 8.31364ZM3.72273 2.10909H16.1318C16.7911 2.10909 17.3727 2.69077 17.3727 3.35V4.93991C16.9849 4.74602 16.5584 4.59091 16.1318 4.59091H11.7886C11.0131 4.59091 10.3538 4.93991 9.92727 5.48281C9.46193 4.93991 8.8027 4.59091 8.06591 4.59091H3.72273C3.25739 4.59091 2.83082 4.74602 2.48182 4.93991V3.35C2.48182 2.69077 3.02472 2.10909 3.72273 2.10909ZM2.48182 7.07273C2.48182 6.41349 3.02472 5.83182 3.72273 5.83182H8.06591C8.72514 5.83182 9.30682 6.41349 9.30682 7.07273V8.31364H3.10227C2.8696 8.31364 2.67571 8.35241 2.48182 8.39119V7.07273ZM18.6136 14.5182H1.24091V11.4159C1.24091 10.4077 2.05526 9.55455 3.10227 9.55455H16.7523C17.7605 9.55455 18.6136 10.4077 18.6136 11.4159V14.5182Z"
                        fill="black"
                      />
                    </svg>
                    <div class="text">Accommodation</div>
                    </div>
                    `
            : ``
        }



                    ${
          item.inclusions.meals
            ? `
                        <div class="inclusion">
                    <svg width="19" height="21" viewBox="0 0 19 21" fill="none" xmlns="http://www.w3.org/2000/svg">
                      <path d="M9.57003 6.44403C9.68636 6.94815 9.68636 7.45227 9.60881 7.91761C9.49247 8.42173 9.2598 8.88707 8.94957 9.27486C8.63935 9.66264 8.21278 9.97287 7.74744 10.2055C7.2821 10.4382 6.77798 10.5545 6.23509 10.5545H5.96364V19.8614C5.96364 20.0165 5.88608 20.1716 5.76974 20.2879C5.65341 20.4043 5.4983 20.4818 5.34318 20.4818C5.14929 20.4818 4.99418 20.4043 4.87784 20.2879C4.76151 20.1716 4.72273 20.0165 4.72273 19.8614V10.5545H4.4125C3.8696 10.5545 3.36548 10.4382 2.90014 10.2055C2.4348 10.0116 2.00824 9.66264 1.69801 9.27486C1.38778 8.88707 1.15511 8.42173 1.03878 7.91761C0.922443 7.41349 0.961222 6.90937 1.07756 6.40526L1.93068 1.17017C1.96946 1.01506 2.04702 0.859943 2.16335 0.782386C2.31847 0.704829 2.47358 0.666051 2.62869 0.666051C2.78381 0.704829 2.93892 0.782386 3.01648 0.9375C3.13281 1.05384 3.17159 1.20895 3.13281 1.36406L2.27969 6.63793C2.20213 6.94815 2.20213 7.29716 2.24091 7.60739C2.31847 7.91761 2.47358 8.22784 2.66747 8.49929C2.86136 8.73196 3.13281 8.96463 3.44304 9.11974C3.75327 9.23608 4.06349 9.31364 4.4125 9.31364H6.23509C6.58409 9.31364 6.89432 9.23608 7.20455 9.08097C7.51477 8.96463 7.78622 8.73196 7.98011 8.49929C8.17401 8.22784 8.32912 7.95639 8.40668 7.64616C8.44545 7.29716 8.44545 6.98693 8.3679 6.6767L7.51477 1.36406C7.47599 1.20895 7.51477 1.05384 7.59233 0.898721C7.70866 0.782386 7.86378 0.666051 8.01889 0.666051C8.17401 0.627273 8.32912 0.666051 8.48423 0.782386C8.60057 0.859943 8.67813 1.01506 8.7169 1.17017L9.57003 6.44403ZM3.83082 8.03395C3.75327 7.99517 3.67571 7.95639 3.63693 7.87884C3.55938 7.84006 3.5206 7.7625 3.48182 7.68494C3.48182 7.60739 3.44304 7.52983 3.48182 7.45227L3.79205 1.24773C3.79205 1.17017 3.79205 1.05384 3.83082 0.976278C3.8696 0.898721 3.90838 0.859943 3.94716 0.782386C4.02472 0.743608 4.10227 0.704829 4.17983 0.666051C4.25739 0.627273 4.33494 0.627273 4.4125 0.666051C4.49006 0.666051 4.56761 0.666051 4.64517 0.704829C4.72273 0.743608 4.80028 0.782386 4.83906 0.859943C4.91662 0.898721 4.9554 0.976278 4.99418 1.05384C4.99418 1.13139 5.03295 1.20895 5.03295 1.28651L4.72273 7.49105C4.68395 7.64616 4.64517 7.80128 4.52884 7.91761C4.4125 8.03395 4.25739 8.07273 4.10227 8.07273H4.06349C3.98594 8.07273 3.90838 8.07273 3.83082 8.03395ZM6.11875 7.91761C6.00241 7.80128 5.96364 7.64616 5.96364 7.49105L5.65341 1.28651C5.61463 1.13139 5.69219 0.976278 5.80852 0.859943C5.88608 0.743608 6.04119 0.666051 6.23509 0.666051C6.31264 0.627273 6.3902 0.666051 6.46776 0.666051C6.54531 0.704829 6.62287 0.743608 6.66165 0.782386C6.7392 0.859943 6.77798 0.9375 6.81676 1.01506C6.85554 1.05384 6.85554 1.17017 6.89432 1.24773L7.20455 7.45227C7.20455 7.52983 7.16577 7.60739 7.16577 7.68494C7.12699 7.7625 7.08821 7.84006 7.01065 7.87884C6.97188 7.95639 6.89432 7.99517 6.81676 8.03395C6.7392 8.07273 6.66165 8.07273 6.58409 8.07273C6.3902 8.07273 6.23509 8.03395 6.11875 7.91761ZM18.3727 3.69077V19.8226C18.3727 20.0165 18.2952 20.1716 18.1788 20.2879C18.0625 20.4043 17.9074 20.443 17.7523 20.443C17.5584 20.443 17.4033 20.4043 17.2869 20.2879C17.1706 20.1716 17.1318 20.0165 17.1318 19.8226V14.2385H14.6112C14.2622 14.2385 13.952 14.1997 13.6418 14.0834C13.3315 13.9283 13.0601 13.7732 12.8274 13.5405C12.5947 13.3078 12.4396 12.9976 12.2845 12.7261C12.1682 12.4159 12.1294 12.0669 12.1294 11.7567L12.1682 7.33594C12.1682 5.90114 12.5947 4.50511 13.3703 3.34176C14.1847 2.13963 15.3092 1.24773 16.6665 0.704829C16.8216 0.627273 17.0543 0.588494 17.2482 0.627273C17.442 0.627273 17.6359 0.704829 17.7911 0.821165C17.9849 0.9375 18.1013 1.09261 18.2176 1.28651C18.2952 1.44162 18.3727 1.63551 18.3727 1.86818V3.69077ZM17.1318 1.86818C16.0072 2.29474 15.0766 3.03153 14.4173 4.03977C13.7581 5.00923 13.3703 6.17259 13.4091 7.33594L13.3703 11.7567C13.3703 11.9506 13.4091 12.1057 13.4479 12.2608C13.5254 12.4159 13.603 12.5322 13.7193 12.6486C13.8357 12.7649 13.9908 12.8425 14.1071 12.92C14.2622 12.9976 14.4561 13.0364 14.6112 13.0364H17.0543L17.1318 1.86818Z" fill="black" />
                    </svg>
                    <div class="text">Meals</div>
                    </div>
                    `
            : ``
        }


                    ${
          item.inclusions.transfer
            ? `
                        <div class="inclusion">
                    <svg width="18" height="21" viewBox="0 0 18 21" fill="none" xmlns="http://www.w3.org/2000/svg">
                      <path
                        d="M2.79205 14.2773C2.79205 13.7732 3.17983 13.3466 3.72273 13.3466C4.22685 13.3466 4.65341 13.7732 4.65341 14.2773C4.65341 14.8202 4.22685 15.208 3.72273 15.208C3.17983 15.208 2.79205 14.8202 2.79205 14.2773ZM14.5807 14.2773C14.5807 14.8202 14.1541 15.208 13.65 15.208C13.1071 15.208 12.7193 14.8202 12.7193 14.2773C12.7193 13.7732 13.1071 13.3466 13.65 13.3466C14.1541 13.3466 14.5807 13.7732 14.5807 14.2773ZM17.0625 3.38054C17.2952 3.69077 17.3727 4.03977 17.3727 4.35V15.5182C17.3727 16.4489 16.8686 17.2632 16.1318 17.6898V19.8614C16.1318 20.2104 15.8216 20.4818 15.5114 20.4818C15.1624 20.4818 14.8909 20.2104 14.8909 19.8614V18H2.48182V19.8614C2.48182 20.2104 2.17159 20.4818 1.86136 20.4818C1.51236 20.4818 1.24091 20.2104 1.24091 19.8614V17.6898C0.465341 17.2632 0 16.4489 0 15.5182V4.35C0 4.03977 0.0387784 3.65199 0.310227 3.34176C1.24091 2.21719 3.87784 0.627273 8.68636 0.627273C13.8051 0.627273 16.1706 2.17841 17.0625 3.38054ZM1.24091 4.35H16.1318C16.1318 4.19489 16.093 4.15611 16.0543 4.11733C15.4726 3.38054 13.5724 1.86818 8.68636 1.86818C4.11051 1.86818 1.90014 3.38054 1.27969 4.15611C1.24091 4.15611 1.24091 4.19489 1.24091 4.35ZM1.24091 5.59091V10.5545H16.1318V5.59091H1.24091ZM16.1318 11.7955H1.24091V15.5182C1.24091 16.2162 1.78381 16.7591 2.48182 16.7591H14.8909C15.5501 16.7591 16.1318 16.2162 16.1318 15.5182V11.7955Z"
                        fill="black"
                      />
                    </svg>
                    <div class="text">Transfer</div>
                    </div>
                    `
            : ``
        }


                  </div>
                  ${

          self.currencyDiscountApplicable && item.discount && item.discount > 0
            ? `

                  <div class="save">Save ${self.currencySymbol}${numeral(self.convertPrice(item.discount)).format('0,0')}</div>
                  `
            : ``
        }

                </div>
                <div class="price">
                    ${false && item.departureDate && item.departureDate.length > 0 ? `<div class="valid">Start <span class="valid-date">${dayjs(dayjs.unix(item.departureDate[0])).format('D MMM YYYY')}</span></div>` : ``}
                  ${item.fromPrice > 0 ? `

                    <div class="from">From${self.currencyDiscountApplicable && item.fromPrice && item.fromPrice > 0 && item.fromPrice !== item.price ? ` <span>${self.currencyCode}${numeral(self.convertPrice(item.fromPrice)).format('0,0')}</span>` : ``}</div>
                    <div class="price-value"><span>${self.currencyCode}</span>${self.currencyDiscountApplicable ? numeral(self.convertPrice(item.price)).format('0,0') : numeral(self.convertPrice(item.fromPrice)).format('0,0') }</div>
                    <div class="per">${priceType}</div>
                  ` : `
                    <div class="from">Price</div>
                    <div class="price-value">TBC</div>
                    <div class="per">${priceType}</div>
                  `}
                </div>
              </div>
              </a>
            </div>
          </div>

          `
      })
      .join('')}
      </div>
    `
  }

  self.convertPrice = function(audPrice){
    if (self.conversionRate == 1){
      return audPrice;
    }

    return Math.ceil(audPrice * self.conversionRate)
  }

  self.renderDepartureInput = function (renderOptions, isFirstRender) {
    const { start, range, canRefine, refine } = renderOptions
    const [min, max] = start

    if (isFirstRender) {

    } else {

    }
  }

  self.renderStartCityRefinementList = (renderOptions, isFirstRender) => {
    const { items } = renderOptions

    document.querySelector('.start-submenu .items').innerHTML = `
        <div class="item" data-value="">
          All Start Locations
        </div>
      ${items.map(item => `
        <div class="item" data-value="${item.label}">
          ${item.label}
        </div>`
    ).join('')}
    `
  }

  self.renderEndCityRefinementList = (renderOptions, isFirstRender) => {
    const { items } = renderOptions

    document.querySelector('.end-submenu .items').innerHTML = `
        <div class="item" data-value="">
          All End Locations
        </div>
      ${items.map(item => `
        <div class="item" data-value="${item.label}">
          ${item.label}
        </div>`
    ).join('')}
    `
  }

  self.endCityRefinementList = connectRefinementList(
    self.renderEndCityRefinementList
  )

  self.startCityRefinementList = connectRefinementList(
    self.renderStartCityRefinementList
  )

  // Set the InstantSearch index UI state from external events.
  self.setInstantSearchUiState = function (indexUiState) {

    self.search.setUiState(uiState => ({
      ...uiState,
      [self.searchIndex]: {
        ...uiState[self.searchIndex],
        // We reset the page when the search state changes.
        page: 1,
        ...indexUiState,
      },
    }))
  }

  // Return the InstantSearch index UI state.
  self.getInstantSearchUiState = function () {
    const uiState = self.instantSearchRouter.read()

    return (uiState && uiState[self.searchIndex]) || {}
  }

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {

    }

    if ($('.conversion').length){

      self.currencyCode = $('.conversion').data('currency')
      self.currencySymbol = $('.conversion').data('symbol')
      self.currencyDiscountApplicable = $('.conversion').data('discount') == "True"
      self.conversionRate = Number($('.conversion').data('rate'))
    }

    self.el.css('opacity', 1)

    let params = new URLSearchParams(window.location.search)
    if (params.has("showFilters")) {
      showFilters()
    }

    $('.submenu.where-submenu .item').on('click', function (e) {
      e.preventDefault()

      var val = $(this).attr('data-value')
      var label = $(this).text()

      var state = self.getInstantSearchUiState()
      console.log('state', state)
      var dest = state.refinementList && state.refinementList.destination ? state.refinementList.destination : []

      dest.push(val)

      self.setInstantSearchUiState({
        // query: val,
        refinementList: {
          startCity: [],
          endCity: [],
          destination: dest,
        }
      })

      // self.el.find('.ui.dropdown').dropdown('restore defaults');

      // $('.trigger.destination .value').text(label).removeClass('default');
      $('.trigger.start .value').text('Select a Start Location').addClass('default')
      $('.trigger.end .value').text('Select an End Location').addClass('default')

      setTimeout(function () {
        $('.trigger.destination').removeClass('active')
      }, 100)

    })

    // const departureInput = connectRange(self.renderDepartureInput)
    const customHits = connectHits(self.renderHits)

    self.instantSearchRouter = historyRouter()

    self.search = instantsearch({
      indexName: self.searchIndex,
      searchClient: self.searchClient,
      routing: self.instantSearchRouter,
      searchFunction: function (helper) {
        helper.search()

        setTimeout(function () {
          self.handleRender(helper)
          if (self.initialLoad) {
            self.initFilterBox()
          }

          self.initialLoad = false
        }, 200)
      }
    })

    $('body').on('click', '.date-range-button', function (e) {
      e.preventDefault()

      $(this).addClass('active').siblings().removeClass('active')
      self.dateRange = parseInt($(this).data('range'))

      if ($(this).hasClass('flexible')) {
        $('#departureDate').datepicker('setDate', null)

        self.selectedDate = null

        self.setInstantSearchUiState({
          refinementList: {
            ...self.getInstantSearchUiState().refinementList,
            departureDate: null
          }
        })

        $('.trigger.date .value').html('Add Date').addClass('default')
        $('#departureDate').datepicker('refresh')
        $('.trigger.date.active').removeClass('active')
        $('.date-range-button.exact').addClass('active').siblings().removeClass('active')

      } else {
        //$("#departureDate").datepicker('refresh')

        var departureRefinement = []
        var startDate = dayjs(dayjs($('#departureDate').datepicker('getDate')).format('YYYY-MM-DD')).startOf('day').subtract(self.dateRange, 'days').startOf('day')
        var endDate = dayjs($('#departureDate').datepicker('getDate')).startOf('day').add(self.dateRange, 'days').startOf('day')
        departureRefinement.push(startDate.unix())
        for (let index = startDate.unix() + 86400; index <= endDate.unix(); index += 86400) {
          departureRefinement.push(index)
        }
        self.setInstantSearchUiState({
          refinementList: {
            ...self.getInstantSearchUiState().refinementList,
            departureDate: departureRefinement
          }
        })

        // self.setInstantSearchUiState({
        //   refinementList: {
        //     departureDate: (dayjs($("#departureDate").datepicker('getDate')).subtract(self.dateRange, 'days').startOf('day').unix()) + ':' + (dayjs($("#departureDate").datepicker('getDate')).add(self.dateRange, 'days').endOf('day').unix())
        //   }
        // })

        if (self.selectedDate) {
          $('.trigger.date .value').html(self.selectedDate.format('MMM D, YYYY')).removeClass('default')
          if (self.dateRange > 0) {
            $('.trigger.date .value').append(`
              <div class="plusminus">
                <svg width="9" height="13" viewBox="0 0 9 13" fill="none" xmlns="http://www.w3.org/2000/svg">
                  <path d="M8.25 11H0.75C0.328125 11 0 11.3516 0 11.75C0 12.1719 0.328125 12.5 0.75 12.5H8.25C8.64844 12.5 9 12.1484 9 11.75C9 11.375 8.64844 11 8.25 11ZM1.125 5.375H3.75V8C3.75 8.39844 4.07812 8.72656 4.5 8.72656C4.89844 8.72656 5.25 8.375 5.25 8V5.375H7.875C8.27344 5.375 8.625 5.04688 8.625 4.625C8.625 4.22656 8.27344 3.875 7.875 3.875H5.25V1.25C5.25 0.851562 4.89844 0.5 4.5 0.5C4.07812 0.5 3.75 0.851562 3.75 1.27344V3.89844H1.125C0.703125 3.89844 0.375 4.22656 0.375 4.625C0.375 5.04688 0.703125 5.375 1.125 5.375Z" fill="currentColor"/>
                </svg>
                ${self.dateRange}
              </div>
            `)
          }
        }
      }

      if ($(window).width() < 1024) {
        $('.trigger').removeClass('active')
      }
    })

    $(document).mouseup(function (e) {
      var container = $('.trigger')

      // if the target of the click isn't the container nor a descendant of the container
      if (!container.is(e.target) && container.has(e.target).length === 0) {
        $('.trigger').removeClass('active')
      }

      if (
        !filterButton.is(e.target) && filterButton.has(e.target).length === 0 &&
        !filterPanel.is(e.target) && filterPanel.has(e.target).length === 0
      ) {
        hideFilters()
      }
    })

    $('body').on('click', '.submenu.start-submenu .item', function (e) {
      e.preventDefault()

      var val = $(this).attr('data-value')
      var label = $(this).text()

      self.setInstantSearchUiState({
        refinementList: {
          ...self.getInstantSearchUiState().refinementList,
          startCity: [val],
        }
      })

      self.selectedStartLocation = val

      $('.trigger.start .value').text(label).removeClass('default')
      $('.trigger.start').removeClass('active')

    })

    $('body').on('click', '.submenu.end-submenu .item', function (e) {
      e.preventDefault()

      var val = $(this).attr('data-value')
      var label = $(this).text()

      self.setInstantSearchUiState({
        refinementList: {
          ...self.getInstantSearchUiState().refinementList,
          endCity: [val],
        }
      })

      self.selectedEndLocation = val

      $('.trigger.end .value').text(label).removeClass('default')
      $('.trigger.end').removeClass('active')
    })

    $('.new-search .trigger').on('click', function (e) {
      e.preventDefault()
      $(this).addClass('active').siblings('.trigger').removeClass('active')

      if ($(this).hasClass('destination')) {
        $(this).find('input.search').trigger('focus')
      }

      hideFilters()
    })

    filterButton.on('click', function (e) {
      e.preventDefault()

      filterButton.siblings('.trigger').removeClass('active')

      if (filterButton.hasClass('active')) {
        hideFilters()
      } else {

        showFilters()
      }
    })

    // note to self - once extra departure date column is added try using it as a virtual refinement list and storing the items globally so the calendar can access them

    self.search.addWidgets([
      // configure({
      //   filters: 'price > 0',
      // }),
      self.keywordSearch({
        queryHook (query, refine) {
          clearTimeout(self.timerId)
          self.timerId = setTimeout(function () {
            refine(query)
          }, 500)
        }
      }),
      self.destinationRefinementList({
        attribute: 'destination',
        limit: 100,
        sortBy: ['name']
      }),
      // self.virtualRange({
      //   attribute: 'departureDate',
      // }),
      // self.virtualDepartureRefinement({
      //   attribute: 'departureDateIndex',
      //   limit: 999999
      // }),
      self.departureRefinementList({
        attribute: 'departureDate',
        limit: 999999
      }),
      self.startCityRefinementList({
        attribute: 'startCity',
        limit: 1000,
        sortBy: ['name']
      }),
      self.endCityRefinementList({
        attribute: 'endCity',
        limit: 1000,
        sortBy: ['name']
      }),
      refinementList({
        container: '#travelStyles',
        limit: 1000,
        attribute: 'travelStyles',
        sortBy: ['name']
      }),
      refinementList({
        container: '#experiences',
        limit: 1000,
        attribute: 'experiences',
        sortBy: ['name']
      }),
      // refinementList({
      //   container: '#departureDates',
      //   limit: 999999,
      //   attribute: 'departureDate',
      //   sortBy: ['name'],
      //   transformItems: items => {
      //     return items.map(item => {
      //       return {
      //         ...item,
      //         name: dayjs(parseInt(item.label) * 1000).format('YYYY-MM-DD')
      //       }
      //     })
      //   },
      //   templates: {
      //     item(item, {html}) {
      //       return `
      //         <div class="item" data-value="${item.value}">
      //           <div class="label">${item.isRefined ? 'X' : ''} ${dayjs(parseInt(item.label) * 1000).format('DD/MM/YYYY')} - ${item.label}</div>
      //         </div>
      //       `;
      //     }
      //   }
      // }),
      rangeSlider({
        container: '#priceSlider',
        attribute: 'price'
      }),
      rangeSlider({
        container: '#durationSlider',
        attribute: 'duration'
      }),
      toggleRefinement({
        container: '#bookNow',
        attribute: 'options.bookNow',
        templates: {
          labelText () {
            return `Book Now`
          }
        }
      }),
      toggleRefinement({
        container: '#freedomOfChoice',
        attribute: 'options.freedomOfChoice',
        templates: {
          labelText () {
            return `Freedom of Choice`
          }
        }
      }),
      toggleRefinement({
        container: '#peaceOfMind',
        attribute: 'options.peaceOfMind',
        templates: {
          labelText () {
            return `Peace of Mind`
          }
        }
      }),
      toggleRefinement({
        container: '#safeTravels',
        attribute: 'options.safeTravels',
        templates: {
          labelText () {
            return `Sustainable Travel`
          }
        }
      }),
      customHits({
        // Optional parameters
        escapeHTML: false
      }),
      stats({
        container: '#stats',
        templates: {
          text (data, { html }) {
            if (data.hasManyResults) {
              return html`Showing <b>${data.nbHits}</b> results`
            } else if (data.hasOneResult) {
              return html`Showing <b>1</b> result`
            }
          },
        },
      }),
      stats({
        container: '#statsHits',
        templates: {
          text (data, { html }) {
            if (data.hasManyResults) {
              return ''
            } else if (data.hasOneResult) {
              return ''
            } else {
              return html`No results`
            }
          },
        },
      }),
      sortBy({
        container: '#sortBy',
        items: [
          { label: 'Price lowest to highest', value: self.searchIndex },
          { label: 'Price highest to lowest', value: self.searchIndex + '_price_desc' },
          { label: 'Oldest to newest', value: self.searchIndex + '_date_asc' },
          { label: 'Newest to oldest', value: self.searchIndex + '_date_desc' },
          { label: 'Alphabetical a to z', value: self.searchIndex + '_alpha_asc' },
          { label: 'Alphabetical z to a', value: self.searchIndex + '_alpha_desc' },
          { label: 'Duration short to long', value: self.searchIndex + '_duration_asc' },
          { label: 'Duration long to short', value: self.searchIndex + '_duration_desc' }
        ],
      }),
      hitsPerPage({
        container: '#hitsPerPage',
        items: [
          { label: '30', value: 30, default: true },
          { label: '60', value: 60 }
        ],
      }),
      pagination({
        container: '#pagination'
      })
      // hits({
      //   container: '#hits',
      //   templates: {
      //     item(hit) {
      //       return `
      //         <h2>${hit.name}</h2>
      //         <p>${hit.description}</p>
      //         <p>${hit.price}</p>
      //       `
      //     }
      //   }
      // })
    ])

    if (self.currencyDiscountApplicable){
      self.search.addWidgets([
        toggleRefinement({
          container: '#onSale',
          attribute: 'options.onSale',
          templates: {
            labelText () {
              return `On Sale`
            }
          }
        })]);
    }

    self.searchPageState = self.getInstantSearchUiState()

    if (self.searchPageState.refinementList && self.searchPageState.refinementList.departureDate) {

      var u1 = parseInt(self.searchPageState.refinementList.departureDate[0])
      var u2 = parseInt(self.searchPageState.refinementList.departureDate[self.searchPageState.refinementList.departureDate.length - 1])

      self.selectedDate = dayjs.unix(u1 + ((u2 - u1) / 2))
      self.dateRange = Math.floor(((u2 - u1) / 60 / 60 / 24) / 2)

      $('#departureDate').datepicker('setDate', self.selectedDate.toDate())
      $('#departureDate').datepicker('refresh')

      if (self.dateRange) {
        $('.date-range-button').removeClass('active')
        $('.date-range-button[data-range="' + self.dateRange + '"]').addClass('active')
      }

      $('.trigger.date .value').html(self.selectedDate.format('MMM D, YYYY')).removeClass('default')
      if (self.dateRange > 0) {
        $('.trigger.date .value').append(`
            <div class="plusminus">
              <svg width="9" height="13" viewBox="0 0 9 13" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path d="M8.25 11H0.75C0.328125 11 0 11.3516 0 11.75C0 12.1719 0.328125 12.5 0.75 12.5H8.25C8.64844 12.5 9 12.1484 9 11.75C9 11.375 8.64844 11 8.25 11ZM1.125 5.375H3.75V8C3.75 8.39844 4.07812 8.72656 4.5 8.72656C4.89844 8.72656 5.25 8.375 5.25 8V5.375H7.875C8.27344 5.375 8.625 5.04688 8.625 4.625C8.625 4.22656 8.27344 3.875 7.875 3.875H5.25V1.25C5.25 0.851562 4.89844 0.5 4.5 0.5C4.07812 0.5 3.75 0.851562 3.75 1.27344V3.89844H1.125C0.703125 3.89844 0.375 4.22656 0.375 4.625C0.375 5.04688 0.703125 5.375 1.125 5.375Z" fill="currentColor"/>
              </svg>
              ${self.dateRange}
            </div>
          `)
      }
    }

    if (self.searchPageState.refinementList && self.searchPageState.refinementList.startCity) {
      $('.trigger.start .value').text(self.searchPageState.refinementList.startCity[0]).removeClass('default')
    }

    if (self.searchPageState.refinementList && self.searchPageState.refinementList.endCity) {
      $('.trigger.end .value').text(self.searchPageState.refinementList.endCity[0]).removeClass('default')
    }

    self.search.start()

    // init semantic ui dropdowns on the menu selects
    setTimeout(function () {
      self.el.find('.stats-bar select').addClass('ui fluid dropdown').dropdown({
        placeholder: false,
        direction: 'downwards'
      })
    }, 1000)
  }
}
