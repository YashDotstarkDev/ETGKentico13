import { dom } from '@fortawesome/fontawesome-svg-core'
import { history } from 'instantsearch.js/es/lib/routers'

import('algoliasearch/lite').then(({ default: algoliasearch }) => {
  import('instantsearch.js').then(({ default: instantsearch }) => {
    import('dayjs').then(({ default: dayjs }) => {
      import('../../plugins/numeraljs/numeral.min.js').then(({ default: numeral }) => {
        window.numeral = numeral
        import(/* webpackMode: "eager" */ './find-a-package.scss')
        import('../../plugins/jquery-ui/custom')
        import('../../plugins/semantic/form.js')
        import('../../plugins/semantic/checkbox.css')
        import('../../plugins/semantic/checkbox.js')
        import('../../plugins/semantic/dropdown.scss')
        import('../../plugins/semantic/dropdown.js')
        import('../../dbs/scripts/form/dbs.semantic.form.js')
        import('../../plugins/rangeslider/ion.rangeSlider.css')
        require('../../plugins/rangeslider/ion.rangeSlider.js')

        $('.widget.find-a-package').each(function (i, el) {
          $(el).data('widget', new FindAPackage(el, algoliasearch, instantsearch, dayjs))
          $(el).data('widget').init()
        })
      })
    })
  })
})

// import { HitsPerPage, sortBy } from 'react-instantsearch-dom';
import {
  currentRefinements,
  searchBox,
  hits,
  refinementList,
  rangeSlider,
  stats,
  toggleRefinement,
  pagination,
  hitsPerPage,
  sortBy,
  clearRefinements,
  configure,
  menuSelect
} from 'instantsearch.js/es/widgets'
import { connectRefinementList, connectRange } from 'instantsearch.js/es/connectors'

function FindAPackage (el, algoliasearch, instantsearch, dayjs) {
  const self = this
  self.el = $(el)
  self.initialLoad = true
  self.tourIndexName = $(el).attr('data-tour-indexname')

  self.getParameterByName = function (name) {
    var url = window.location.href
    name = name.replace(/[\[\]]/g, '\\$&')
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
      results = regex.exec(url)
    if (!results) return null
    if (!results[2]) return ''
    return decodeURIComponent(results[2].replace(/\+/g, ' '))
  }

  self.currentTab = self.getParameterByName('tab') ? self.getParameterByName('tab') : 'tours'

  // on page load we get it from the querystring
  // activate the tab based on self.currentTab

  $(this).addClass('active').siblings().removeClass('active')
  self.el.find('.bottom[data-tab="' + self.currentTab + '"]').show().siblings().hide()
  self.el.find('.nav a[data-tab="' + self.currentTab + '"], .mobile-nav .links a[data-tab="' + self.currentTab + '"]').addClass('active').siblings().removeClass('active')

  self.handleRender = function () {

    console.log('handleRender')

    var max = self.el.find('.rheostat-handle-upper .rheostat-tooltip').html()
    var min = self.el.find('.rheostat-handle-lower .rheostat-tooltip').html()

    self.el.find('.bottom[data-tab="tours"] .from-value').html(numeral(min).format('$0,0'))
    self.el.find('.bottom[data-tab="tours"] .to-value').html(numeral(max).format('$0,0'))

    self.el.find('.tour_stats').html(self.el.find('#tour_stats .ais-Stats-text').html())
    self.el.find('.inspiration_stats').html(self.el.find('#inspiration_stats .ais-Stats-text').html())
    self.el.find('.content_stats').html(self.el.find('#content_stats .ais-Stats-text').html())

    self.el.find('.formatted-price').each(function (i, el) {
      var price = $(el).text()
      $(el).text('AUD' + numeral(price).format('0,0'))
    })
    self.el.find('.trim-pp').each(function (i, el) {
      var text = $(el).text()
      if (text != null) {
        $(el).text(text.replace(/per person/, 'pp'))
      }

    })

    self.el.find('[data-inclusions]').each(function (i, el) {
      $(el).html('')
      var inclusions = $(el).attr('data-inclusions').split('|')
      for (var j = 0; j < inclusions.length; j++) {
        var inclusion = inclusions[j]
        $(el).append(self.el.find('.icon-set div[data-inclusion="' + inclusion + '"]').html())
      }
    })

    self.el.find('.view-container select').addClass('make-dropdown')
    self.el.find('.make-dropdown').dropdown({
      placeholder: false,
      direction: 'downwards'
    })

    if (self.el.find('#tour_stats .ais-Stats-text').length > 0) {
      var activeTab = null
      if (self.el.find('#tour_stats .ais-Stats-text').text() == '0') {
        if (self.el.find('#inspiration_stats .ais-Stats-text').length > 0 && self.el.find('#inspiration_stats .ais-Stats-text').text() != '0') {
          activeTab = 'inspiration'
        } else if (self.el.find('#content_stats .ais-Stats-text').length > 0 && self.el.find('#content_stats .ais-Stats-text').text() != '0') {
          activeTab = 'content'
        }

        if (activeTab != null) {
          self.el.find('.nav a').removeClass('active')
          self.el.find('.nav a[data-tab="' + activeTab + '"]').addClass('active')

          self.el.find('.bottom').hide()
          self.el.find('.bottom[data-tab="' + activeTab + '"]').show()

        }
      }

    }

    dom.i2svg()
  }

  self.initFilterBox = function () {
    console.log('init filter box')
    var query = self.getParameterByName('query')

    if (query) {
      self.el.find('.search-query').val(query)
    }

    var destination = self.getParameterByName('destination')
    var experiences = self.getParameterByName('experiences')
    var travelstyles = self.getParameterByName('travelstyles')
    if (destination) {

      self.el.find('.filter-set-destination').addClass('expanded')
      self.el.find('.filter-set-destination .filters').show()
    }

    if (experiences) {
      self.el.find('.filter-set-experiences').addClass('expanded')
      self.el.find('.filter-set-experiences .filters').show()
    }

    if (travelstyles) {
      self.el.find('.filter-set-travelstyles').addClass('expanded')
      self.el.find('.filter-set-travelstyles .filters').show()
    }

  }
  self.init = function () {
    self.el.find('.advanced-search-trigger').click(function (e) {
      self.el.find('.advanced-search-container').slideDown()
      self.el.find('.advanced-search-container').css('display', 'flex')
      //self.el.find('.advanced-search-close-trigger').show();
      //$(this).hide();
      self.el.find('.search-filters').addClass('advance-filters')
    })

    self.el.find('.advanced-search-close-trigger').click(function (e) {
      //$(this).hide();
      //self.el.find('.advanced-search-trigger').show();
      self.el.find('.advanced-search-container').slideUp()
      self.el.find('.search-filters').removeClass('advance-filters')
    })

    self.searchClient = algoliasearch('SK049MNMX7', '79d4022f9d2199e9dcc43221a5171c43')

    setTimeout(function () {

      self.el.find('.bottom[data-tab="' + $('.nav a.active').attr('data-tab') + '"]').fadeIn()
      self.handleRender()
    }, 1000)

    self.el.find('.selected .icon').click(function (event) {
      event.preventDefault()
      $(this).parents('.filter-set').removeClass('haschecked')
      $(this).parents('.filter-set').find('[type=\'checkbox\']:checked').trigger('click')
    })

    var mainRouter = {
      router: history({
        createURL ({ qsModule, routeState, location }) {

          var url = self.el.attr('data-current-url')
          const urlParts = location.href.match(`/^(.*?)\/${url}/`)
          const baseUrl = `${urlParts ? urlParts[1] : ''}/${url}/`

          const queryParameters = {}

          // all will have this
          if (routeState.query) {
            queryParameters.query = encodeURIComponent(routeState.query)
          }
          if (routeState.page !== 1) {
            queryParameters.page = routeState.page
          }

          if (routeState.destination) {
            queryParameters.destination = encodeURIComponent(routeState.destination.join(','))
          }
          if (routeState.experiences) {
            queryParameters.experiences = encodeURIComponent(routeState.experiences.join(','))
          }
          if (routeState.travelstyles) {
            queryParameters.travelstyles = encodeURIComponent(routeState.travelstyles.join(','))
          }
          if (routeState.duration) {
            queryParameters.duration = encodeURIComponent(routeState.duration.join(','))
          }
          if (routeState.price) {
            queryParameters.price = encodeURIComponent(routeState.price)
          }
          if (routeState.deal) {
            queryParameters.deal = routeState.deal
          }
          if (routeState.peaceofmind) {
            queryParameters.peaceofmind = routeState.peaceofmind
          }
          if (routeState.safetravel) {
            queryParameters.safetravel = routeState.safetravel
          }
          if (routeState.freedomofchoice) {
            queryParameters.freedomofchoice = routeState.freedomofchoice
          }
          if (routeState.tab) {
            queryParameters.tab = routeState.tab
          }

          const queryString = qsModule.stringify(queryParameters, {
            addQueryPrefix: true,
            arrayFormat: 'repeat'
          })

          if (queryString == '?tab=tours') {
            return
          }
          return `${baseUrl}${queryString}`
        },

        parseURL ({ qsModule, location }) {
          const {
            query = '',
            destination = '',
            experiences = '',
            travelstyles = '',
            duration = '',
            tourcode = '',
            price = '',
            deal = '',
            peaceofmind = '',
            safetravel = '',
            freedomofchoice = '',
            page = ''
          } = qsModule.parse(
            location.search.slice(1)
          )
          return {
            query: decodeURIComponent(query),
            destination: decodeURIComponent(destination),
            experiences: decodeURIComponent(experiences),
            travelstyles: decodeURIComponent(travelstyles),
            tourcode: decodeURIComponent(tourcode),
            duration: decodeURIComponent(duration),
            price: decodeURIComponent(price),
            deal: decodeURIComponent(deal),
            peaceofmind: decodeURIComponent(peaceofmind),
            freedomofchoice: decodeURIComponent(freedomofchoice),
            safetravel: decodeURIComponent(safetravel),
            page: decodeURIComponent(page)
          }
        }
      }),

      stateMapping: {
        stateToRoute (uiState) {

          let indexUiState = {}

          if (self.currentTab === 'tours') {
            indexUiState = uiState[$(el).attr('data-tour-indexname')] || {}
          }

          if (self.currentTab === 'inspiration') {
            indexUiState = uiState[$(el).attr('data-article-indexname')] || {}
          }

          if (self.currentTab === 'content') {
            indexUiState = uiState[$(el).attr('data-contents-indexname')] || {}
          }

          return {
            query: indexUiState.query,
            destination: indexUiState.refinementList && indexUiState.refinementList.DestinationNameList,
            experiences: indexUiState.refinementList && indexUiState.refinementList.ExperienceNameList,
            tours: indexUiState.refinementList && indexUiState.refinementList.TravelStylesList,
            travelstyles: indexUiState.refinementList && indexUiState.refinementList.TravelStyles,
            duration: indexUiState.refinementList && indexUiState.refinementList.TourDays,
            tourcode: indexUiState.refinementList && indexUiState.refinementList.TourCode,
            price: indexUiState.range && indexUiState.range.Price,
            deal: indexUiState.toggle && indexUiState.toggle.HasDiscount,
            peaceofmind: indexUiState.toggle && indexUiState.toggle.TourHasPeaceOfMindGuarantee,
            freedomofchoice: indexUiState.toggle && indexUiState.toggle.TourHasFreedomOfChoice,
            safetravel: indexUiState.toggle && indexUiState.toggle.TourHasSafeTravel,
            page: indexUiState.page,
            tab: self.currentTab
          }
        },

        routeToState (routeState) {

          return {
            [$(el).attr('data-tour-indexname')]: {
              query: routeState.query,
              refinementList: {
                DestinationNameList: checkAndReturnArray(routeState.destination),
                ExperienceNameList: checkAndReturnArray(routeState.experiences),
                TravelStyles: checkAndReturnArray(routeState.travelstyles),
                TourDays: checkAndReturnArray(routeState.duration),
                TourCode: checkAndReturnArray(routeState.tourcode),
              },
              range: {
                Price: routeState.price
              },
              toggle: {
                HasDiscount: routeState.deal,
                TourHasPeaceOfMindGuarantee: routeState.peaceofmind,
                TourHasFreedomOfChoice: routeState.freedomofchoice,
                TourHasSafeTravel: routeState.safetravel
              },
              page: routeState.page
            },
            [$(el).attr('data-article-indexname')]: {
              query: routeState.query,
              refinementList: {
                DestinationNameList: checkAndReturnArray(routeState.destination),
                ExperienceNameList: checkAndReturnArray(routeState.experiences)
              },
              page: routeState.page
            },
            [$(el).attr('data-contents-indexname')]: {
              query: routeState.query,
              page: routeState.page
            }
          }
        }
      }
    }

    function checkAndReturnArray (arrayObject) {

      if (arrayObject == null) {
        return []
      }

      return Array.isArray(arrayObject) ? arrayObject : (arrayObject ? arrayObject.split(',') : [])
    }

    // inspiration search
    self.inspirationSearch = instantsearch({
      indexName: $(el).attr('data-article-indexname'),
      searchClient: self.searchClient,
      routing: mainRouter,
      searchFunction: function (helper) {
        helper.search()
        setTimeout(function () {
          self.handleRender()
        }, 200)
      }
    })
    self.inspirationSearch.addWidgets([
      stats({
        container: '#inspiration_stats',
        templates: {
          text: `
      {{nbHits}}
    `,
        }
      }),
      hits({
        container: '#inspiration_hits',
        templates: {
          item: `

          <a href="{{Url}}" class="item">
                  <span class="image">
                    <img class="lazyload" data-src="{{ArticleHeroImage}}" alt="">
                  </span>
                  <span class="location">{{DestinationNameList}}</span>
                  <span class="title clamp2">{{ArticleTitle}} </span>
                </a>
          `,
        },
      }),
      refinementList({
        container: '#inspiration_destinationfilters',
        attribute: 'DestinationNameList',
        limit: 30,
        templates: {
          item: `
          <div class="filter">
            <div class="ui checkbox">
            {{#isRefined}}
              <input type="checkbox" checked/>
              {{/isRefined}}
              <input type="checkbox"/>

              <label>{{label}} ({{count}})</label>
            </div>
          </div>
        `
        }
      }),
      refinementList({
        container: '#inspiration_experiencefilters',
        attribute: 'ExperienceNameList',
        limit: 30,
        templates: {
          item: `
          <div class="filter">
            <div class="ui checkbox">
            {{#isRefined}}
              <input type="checkbox" checked/>
              {{/isRefined}}
              <input type="checkbox"/>

              <label>{{label}} ({{count}})</label>
            </div>
          </div>
        `
        }
      }),
      hitsPerPage({
        container: '#inspiration_view',
        items: [
          { label: '30', value: 30, default: true },
          { label: '60', value: 60 }
        ]
      }),
      sortBy({
        container: '#inspiration_sort',
        items: [
          { label: 'Oldest to newest', value: 'Article' },
          { label: 'Newest to oldest', value: 'inspiration_created_asc' }
        ],
      }),
      pagination({
        container: '#inspiration_pagination'
      })
    ])
    self.inspirationSearch.start()

    // content search
    self.contentSearch = instantsearch({
      indexName: $(el).attr('data-contents-indexname'),
      searchClient: self.searchClient,
      routing: mainRouter,
      searchFunction: function (helper) {
        helper.search()
        setTimeout(function () {
          self.handleRender()
        }, 200)
      }
    })
    self.contentSearch.addWidgets([
      stats({
        container: '#content_stats',
        templates: {
          text: `
      {{nbHits}}
    `,
        }
      }),
      hits({
        container: '#content_hits',
        templates: {
          item: `

               <a href="{{Url}}" class="item">
                  <span class="image">
                    {{#SearchImage}}
                      <img class="lazyload" data-src="{{SearchImage}}" alt="">
                    {{/SearchImage}}
                    {{^SearchImage}}
                    <img class="lazyload" data-src="/images/watermark-image.png" alt="">
                    {{/SearchImage}}

                  </span>
                  <span class="location">{{SearchTitle}}</span>
                  <span class="title clamp2">{{SearchDescription}} </span>
                </a>

          `,
        },
      }),
      hitsPerPage({
        container: '#content_view',
        items: [
          { label: '30', value: 30, default: true },
          { label: '60', value: 60 }
        ]
      }),
      pagination({
        container: '#content_pagination'
      })
    ])
    self.contentSearch.start()

    // start date
    const renderStartRangeInput = (renderOptions, isFirstRender) => {
      const { start, range, refine, widgetParams } = renderOptions
      const [min, max] = start
      if (isFirstRender) {
        document.querySelector(widgetParams.container).innerHTML = `<input type="text" class="calendar-start" placeholder="Select desired start date"/>`
        setTimeout(function () {
          self.el.find('.calendar-start').datepicker({
            dateFormat: 'dd/mm/yy',
            showOtherMonths: true,
            selectOtherMonths: true,
            firstDay: 1
          })
          self.el.find('.calendar-start').change(function () {
            if (self.el.find('.calendar-start').datepicker('getDate')) {
              const startDate = dayjs(self.el.find('.calendar-start').datepicker('getDate')).unix()
              refine([0, startDate])
              /* if ($('.calendar-end').datepicker('getDate')){
                 refine([startDate, dayjs($('.calendar-end').datepicker('getDate')).unix()])

               }else{
                 refine([0, startDate])

               }*/
            } else {
              refine([])
            }
          })
        }, 0)

      } else {
        /*console.log('setting start min date', dayjs(range.min * 1000).format('DD/MM/YYYY'))
        console.log('setting start max date', dayjs(range.max * 1000).format('DD/MM/YYYY'))
        self.el.find('.calendar-start').datepicker('option', 'maxDate', dayjs(range.max * 1000).toDate())
        self.el.find('.calendar-start').datepicker('option', 'minDate', dayjs(range.min * 1000).toDate())*/
      }
    }

    // end date
    const renderEndRangeInput = (renderOptions, isFirstRender) => {
      const { start, range, refine, widgetParams } = renderOptions
      const [min, max] = start
      if (isFirstRender) {
        document.querySelector(widgetParams.container).innerHTML = `<input type="text" class="calendar-end" placeholder="Select desired end date"/>`
        setTimeout(function () {
          self.el.find('.calendar-end').datepicker({
            dateFormat: 'dd/mm/yy',
            showOtherMonths: true,
            selectOtherMonths: true,
            firstDay: 1
          })
          self.el.find('.calendar-end').change(function () {
            if (self.el.find('.calendar-end').datepicker('getDate')) {
              const endDate = dayjs(self.el.find('.calendar-end').datepicker('getDate')).unix()
              refine([endDate, Infinity])
              /*
              if ($('.calendar-start').datepicker('getDate')){
                refine([dayjs($('.calendar-start').datepicker('getDate')).unix(), endDate])
              }else
              {
                refine([endDate, Infinity ])
              }*/

            } else {
              refine([])
            }
          })
        }, 0)

      } else {
        /*console.log('setting end min date', dayjs(range.min * 1000).format('DD/MM/YYYY'))
        console.log('setting end max date', dayjs(range.max * 1000).format('DD/MM/YYYY'))
        self.el.find('.calendar-end').datepicker('option', 'maxDate', dayjs(range.max * 1000).toDate())
        self.el.find('.calendar-end').datepicker('option', 'minDate', dayjs(range.min * 1000).toDate())*/
      }
    }

    const customStartRangeInput = connectRange(
      renderStartRangeInput
    )

    const customEndRangeInput = connectRange(
      renderEndRangeInput
    )

    // tour search
    self.tourSearch = instantsearch({
      indexName: $(el).attr('data-tour-indexname'),
      searchClient: self.searchClient,
      // routing: (self.currentTab === 'tours') ? tourRouter : null,
      routing: mainRouter,
      searchFunction: function (helper) {
        self.inspirationSearch.helper.setQuery(helper.state.query).search()
        self.contentSearch.helper.setQuery(helper.state.query).search()
        helper.search()

        setTimeout(function () {
          self.handleRender()
          if (self.initialLoad) {
            self.initFilterBox()
          }

          self.initialLoad = false
        }, 200)
      }
    })
    self.tourSearch.addWidgets([

      searchBox({
        container: '#tour_searchbox',
        placeholder: 'Enter Destinations, Experiences, Travel Styles, or Tour Codes',
        queryHook: function (query, search) {
          if (query.length >= 0) {
            search(query)

            setTimeout(function () {

              var max = self.el.find('.rheostat-handle-upper .rheostat-tooltip').html()
              var min = self.el.find('.rheostat-handle-lower .rheostat-tooltip').html()

              self.el.find('.bottom[data-tab="tours"] .from-value').html(min)
              self.el.find('.bottom[data-tab="tours"] .to-value').html(max)
            }, 100)
          } else {

          }
        }
      }),/*customStartRangeInput({
        container: '#inputStartDate',
        attribute: 'TourStartDateTimeStamp'
      }),

      customEndRangeInput({
        container: '#inputEndDate',
        attribute: 'TourEndDateTimeStamp'
      }),*/
      /*
            menuSelect({
              container: '#destinationMenu',
              attribute: 'DestinationNameFilterList',
              showMoreLimit: 30,
              templates: {
                defaultOption: 'All destinations',
              },
            }),

            menuSelect({
              container: '#tourCodeMenu',
              attribute: 'TourCode',
              showMoreLimit: 500,
              templates: {
                item: '{{label}}',
                defaultOption: 'All tour codes',
              },
            }),
      */
      menuSelect({
        container: '#startLocationMenu',
        attribute: 'TourDepartsFromInAustralia',
        limit: 100,
        showMoreLimit: 500,
        templates: {
          defaultOption: 'All Start Locations',
        },
      }),

      menuSelect({
        container: '#endLocationMenu',
        attribute: 'TourTravelEnds',
        limit: 100,
        showMoreLimit: 500,
        templates: {
          defaultOption: 'All End Locations',
        },
      }),
      stats({
        container: '#tour_stats',
        templates: {
          text: `
      {{nbHits}}
    `,
        },
      }),
      hits({
        container: '#tour_hits',
        transformItems (items) {

          return items.map(function (item) {
            if (item.TourPrimaryCountryName === undefined) {
              item.TourPrimaryCountryName = ''
            }
            if (item.TourSubCountryNames === undefined) {
              item.TourSubCountryNames = []
            }
            if (item.MainHotelName === undefined) {
              item.MainHotelName = ''
            }
            if (item.TourHighlights === undefined) {
              item.TourHighlights = []
            }
            var itemHighlights = []
            if (item.MainHotelName.length) {
              itemHighlights.push('Stay at the ' + item.MainHotelName)
              if (item.TourHighlights[0] !== undefined) {
                itemHighlights.push(item.TourHighlights[0])
              }
            } else {
              if (item.TourHighlights[0] !== undefined) {
                itemHighlights.push(item.TourHighlights[0])
              }
              if (item.TourHighlights[1] !== undefined) {
                itemHighlights.push(item.TourHighlights[1])
              }
            }
            if (itemHighlights.length) {
              item.TourHighlightsFormatted = '<ul>'
              if (itemHighlights[0] !== undefined) {
                item.TourHighlightsFormatted += '<li>' + itemHighlights[0] + '</li>'
              }
              if (itemHighlights[1] !== undefined) {
                item.TourHighlightsFormatted += '<li>' + itemHighlights[1] + '</li>'
              }
              item.TourHighlightsFormatted += '</ul>'
            } else {
              item.TourHighlightsFormatted = ''
            }
            if (item.TourTravelEnds != null && item.TourTravelEnds.length && item.TourTravelEnds !== item.TourDepartsFromInAustralia) {
              item.TourWhereFormatted = item.TourDepartsFromInAustralia + ' > ' + item.TourTravelEnds
            } else {
              if (item.TourDepartsFromInAustralia != null && item.TourDepartsFromInAustralia.length) {
                item.TourWhereFormatted = item.TourDepartsFromInAustralia + ' (Starts/Ends)'
              } else {
                item.TourWhereFormatted = null
              }
            }
            return {
              ...item,
              TourSubCountryNamesFormatted: item.TourSubCountryNames.join(' + '),
              TravelStylesFormatted: item.TravelStyles && item.TravelStyles.length ? item.TravelStyles.join(' + ') : []
            }
          })
        },
        templates: {
          item: `
          <a href="{{Url}}" class="widget product-tile" style="opacity: 1;">
            <span class="image">
            <img class="lazyload bg" data-src="{{TourHeroImage}}" alt="">
            <span class="logos">
                {{#TourHasPeaceOfMindGuarantee}}
                  <img class="lazyload logo" data-src="/images/product-tile-pom.png" alt="">
                {{/TourHasPeaceOfMindGuarantee}}
                {{#TourHasFreedomOfChoice}}
                  <img class="lazyload logo" data-src="/images/product-tile-foc.png" alt="">
                {{/TourHasFreedomOfChoice}}
                {{#TourIsExclusive}}
                  <img class="lazyload logo" data-src="/images/product-tile-exclusive.png" alt="">
                {{/TourIsExclusive}}
                {{#TourHasSafeTravel}}
                  <img class="lazyload logo" data-src="/images/product-tile-safe.png" alt="">
                {{/TourHasSafeTravel}}
            </span>

            {{#DiscountText}}
              <span class="save">{{{DiscountText}}}</span>
              {{/DiscountText}}
            </span>
<!--            <span class="location">{{TourDepartureCity}} <span class="grey">></span> {{TourDestinationCity}}</span>-->
            <span class="location">{{TourPrimaryCountryName}} {{#TourSubCountryNamesFormatted}}<span> + {{TourSubCountryNamesFormatted}}</span>{{/TourSubCountryNamesFormatted}}</span>
            <span class="title" data-mh="even-title">
            {{#helpers.highlight}}{ "attribute": "TourName" }{{/helpers.highlight}}
            </span>
            <span class="code">
            Package Code: {{#helpers.highlight}}{ "attribute": "TourCode" }{{/helpers.highlight}}
            </span>
            <span class="type">
            {{TravelStylesFormatted}}
            </span>
            <span class="where">
            {{TourWhereFormatted}}
            </span>

              <span class="fixed-element">
                <span class="line">{{TourDays}} days
                  <span class="grey">|</span>
                  <span class="icons" data-inclusions="{{TourPriceInclusions}}">
                    <span class="icon fas fa-plane"></span>
                    <span class="icon fas fa-bed"></span>
                    <span class="icon fas fa-utensils"></span>
                    <span class="icon fas fa-bus"></span>
                  </span>
                </span>

                <span class="line">
                {{#DiscountedPrice}}
                  <span class="grey">From</span> <span class="strike">{{TourPriceCurrency}}<span class="formatted-price">{{Price}}</span></span> {{TourPriceCurrency}}<span class="formatted-price">{{DiscountedPrice}}</span> <span class="trim-pp">{{TourPriceTypeLabel}}</span>
                  {{/DiscountedPrice}}
                  {{^DiscountedPrice}}
                  <span class="grey">From</span> {{TourPriceCurrency}}<span class="formatted-price">{{Price}}</span> {{TourPriceTypeLabel}}
                  {{/DiscountedPrice}}
                </span>
              </span>
            <span class="hover-overlay">
              <p class="clamp">{{TourSummary}}</p>
              <div class="button primary ui">View package</div>
            </span>
          </a>
          `,
        },
      })/*,
      refinementList({
        container: "#tourcode_filter",
        attribute: 'TourCode',
        limit:30,
        templates: {
          item: `
          <div class="filter">
            <div class="ui checkbox">
            {{#isRefined}}
              <input type="checkbox" checked/>
              {{/isRefined}}
              <input type="checkbox"/>

              <label>{{label}} ({{count}})</label>
            </div>
          </div>
        `
        }
      })*/,
      refinementList({
        container: '#tour_destinationfilters',
        attribute: 'DestinationNameList',
        sortBy: ['name'],
        limit: 30,
        templates: {
          item: `
          <div class="filter">
            <div class="ui checkbox">
            {{#isRefined}}
              <input type="checkbox" checked/>
              {{/isRefined}}
              <input type="checkbox"/>

              <label>{{label}} ({{count}})</label>
            </div>
          </div>
        `
        }
      }),
      refinementList({
        container: '#tour_experiencefilters',
        attribute: 'ExperienceNameList',
        sortBy: ['name'],
        limit: 30,
        templates: {
          item: `
          <div class="filter">
            <div class="ui checkbox">
            {{#isRefined}}
              <input type="checkbox" checked/>
              {{/isRefined}}
              <input type="checkbox"/>

              <label>{{label}} ({{count}})</label>
            </div>
          </div>
        `
        }
      }),
      refinementList({
        container: '#tour_travelstylesfilters',
        attribute: 'TravelStyles',
        sortBy: ['name'],
        templates: {
          item: `
          <div class="filter">
            <div class="ui checkbox">
            {{#isRefined}}
              <input type="checkbox" checked/>
              {{/isRefined}}
              <input type="checkbox"/>
              <label>{{label}} ({{count}})</label>
            </div>
          </div>
        `
        }
      }),
      refinementList({
        container: '#tour_durationfilters',
        attribute: 'TourDays',
        sortBy: function compareFn (a, b) {
          return parseInt(a) < parseInt(b)
        },
        limit: 30,
        templates: {
          item: `
          <div class="filter">
            <div class="ui checkbox">
            {{#isRefined}}
              <input type="checkbox" checked/>
              {{/isRefined}}
              <input type="checkbox"/>

              <label>{{label}} Days ({{count}})</label>
            </div>
          </div>
        `
        }
      }),
      toggleRefinement({
        container: '#tour_deals',
        attribute: 'HasDiscount',
        templates: {
          labelText:
            `
           On Sale Now
          `
        }
      }),
      toggleRefinement({
        container: '#tour_book_now',
        attribute: 'TourBookNow',
        templates: {
          labelText:
            `
           Book Now
          `
        }
      }),
      toggleRefinement({
        container: '#tour_peace_of_mind',
        attribute: 'TourHasPeaceOfMindGuarantee',
        templates: {
          labelText:
            `
           Peace of Mind
          `
        }
      }),
      toggleRefinement({
        container: '#tour_freedom_of_choice',
        attribute: 'TourHasFreedomOfChoice',
        templates: {
          labelText:
            `
           Freedom of Choice
          `
        }
      }),
      toggleRefinement({
        container: '#tour_safe_travel',
        attribute: 'TourHasSafeTravel',
        templates: {
          labelText:
            `
           Safe Travels
          `
        }
      }),
      rangeSlider({
        container: '#tour_range-slider',
        attribute: 'Price'
      }),
      hitsPerPage({
        container: '#tour_view',
        items: [
          { label: '30', value: 30, default: true },
          { label: '60', value: 60 }
        ]
      }),
      sortBy({
        container: '#tour_sort',
        items: [
          { label: 'Price lowest to highest', value: self.tourIndexName },
          { label: 'Price highest to lowest', value: self.tourIndexName.toLowerCase() + '_price_desc' },
          { label: 'Oldest to newest', value: self.tourIndexName.toLowerCase() + '_date_asc' },
          { label: 'Newest to oldest', value: self.tourIndexName.toLowerCase() + '_date_desc' },
          { label: 'Alphabetical a to z', value: self.tourIndexName.toLowerCase() + '_alpha_asc' },
          { label: 'Alphabetical z to a', value: self.tourIndexName.toLowerCase() + '_alpha_desc' },
          { label: 'Duration short to long', value: self.tourIndexName.toLowerCase() + '_duration_asc' },
          { label: 'Duration long to short', value: self.tourIndexName.toLowerCase() + '_duration_desc' }
        ],
      }),
      pagination({
        container: '#tour_pagination'
      }),
      clearRefinements({
        container: '#clear_refinements',
        templates: {
          resetLabel: 'Clear filters',
        },
      })
    ])
    self.tourSearch.start()

    // init semantic ui dropdowns on the menu selects
    setTimeout(function () {
      self.el.find('.advanced-search-inner select').addClass('ui fluid dropdown').dropdown({
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

    self.el.find('.search-button').click(function (e) {
      e.preventDefault()

      var searchUrl = location.pathname

      if (self.el.find('.search-query').val() !== '') {
        searchUrl += '?'
        //searchUrl = searchUrl.replace('//?', '/?')
        searchUrl += `query=${self.el.find('.search-query').val()}`
      }

      if (self.el.find('.advanced-search').length && self.el.find('.advanced-search').is(':visible')) {
        var destination = self.el.find('.destination-dropdown').dropdown('get value')
        var tourcode = self.el.find('.tourcode-dropdown').dropdown('get value')
        var startlocation = self.el.find('.start-location-dropdown').dropdown('get value')
        var endlocation = self.el.find('.end-location-dropdown').dropdown('get value')

        if (destination == '' && tourcode == '' && startlocation == '' && endlocation == '') {
          location = searchUrl
          return
        }

        if (searchUrl.indexOf('?') == -1) {
          searchUrl += '?'
          //searchUrl = searchUrl.replace('//?', '/?')
        }

        searchUrl += self.getSearchQuery('destination', destination)
        searchUrl += self.getSearchQuery('tourcode', tourcode)
        searchUrl += self.getSearchQuery('startlocation', startlocation)
        searchUrl += self.getSearchQuery('endlocation', endlocation)

      }

      location = searchUrl
    })

    if (process.env.NODE_ENV === 'development') {

    }
    self.el.css('opacity', 1)

    /* BINDINGS */
    self.el.find('.bottom .main').each(function (i, el) {
      $(el).find('.rangeslider').ionRangeSlider({
        type: 'double',
        grid: false,
        skin: 'big'
      })
    })

    self.el.find('.mobile-menu-trigger').click(function (event) {
      event.preventDefault()
      self.el.addClass('show-mobile-subnav')
    })

    self.el.find('.mobile-nav .links a').click(function (event) {
      event.preventDefault()
      self.el.removeClass('show-mobile-subnav')

      $(this).siblings().removeClass('active')
      $(this).addClass('active')

      self.el.find('.bottom').hide()
      self.el.find('.bottom[data-tab="' + $(this).attr('data-tab') + '"]').fadeIn()

    })

    self.el.find('.mobile-nav .close').click(function (event) {
      event.preventDefault()
      self.el.removeClass('show-mobile-subnav')
    })

    self.el.find('.expander').click(function (event) {
      event.preventDefault()
      $(this).parents('.filter-set').toggleClass('expanded')
      $(this).parents('.filter-set').find('.filters').slideToggle()
    })

    self.el.find('.mobile-expand-container .expand-filters').click(function (event) {
      event.preventDefault()
      $(this).toggleClass('expanded')
      self.el.find('.filter-container').slideToggle()

      if ($(this).hasClass('expanded')) {
        $('html, body').animate({ scrollTop: self.el.find('.filter-container').offset().top - 130 }, 500)
      }
    })

    self.el.find('.expand-container .expand-filters').click(function (event) {
      event.preventDefault()
      $(this).toggleClass('expanded')

      if ($(this).hasClass('expanded')) {
        $('html, body').animate({ scrollTop: self.el.find('.filter-container').offset().top - 130 }, 500)
        self.el.find('.filter-set').addClass('expanded')
        self.el.find('.filters').slideDown()
      } else {
        self.el.find('.filter-set').removeClass('expanded')
        self.el.find('.filters').slideUp()
      }
    })

    self.el.find('.nav a, .mobile-nav .links a').click(function (event) {
      event.preventDefault()
      $(this).addClass('active').siblings().removeClass('active')
      self.el.find('.bottom[data-tab="' + $(this).attr('data-tab') + '"]').show().siblings().hide()

      self.currentTab = $(this).attr('data-tab')
    })

  }
}


