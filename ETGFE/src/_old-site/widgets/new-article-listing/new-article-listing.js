import {liteClient} from 'algoliasearch/lite'
import instantsearch from 'instantsearch.js'



import historyRouter from 'instantsearch.js/es/lib/routers/history'

import('../../plugins/numeraljs/numeral.min.js').then(({ default: numeral }) => {
  window.numeral = numeral
  import('../../plugins/swiper/swiper.js').then(({ default: Swiper }) => {
    Promise.all([
      import(/* webpackMode: "eager" */ './new-article-listing.scss'),
      import('../../plugins/swiper/swiper.css'),
      import('../../plugins/semantic/icon.css'),
      import('../../plugins/semantic/input.css'),
      import('../../plugins/semantic/transition.css'),
      import('../../plugins/semantic/transition.js'),
      import('../../plugins/jquery-ui/custom')
    ]).then(() => {
      $('.widget.new-article-listing').each(function (i, el) {
        $(el).data('widget', new NewArticleListing(el, instantsearch, Swiper))
        $(el).data('widget').init()
      })
    })
  })
})

import { menuSelect, stats, pagination, hitsPerPage } from 'instantsearch.js/es/widgets'
import { connectSearchBox, connectHits } from 'instantsearch.js/es/connectors'

function NewArticleListing (el, instantsearch, Swiper) {
  const self = this
  self.el = $(el)
  self.initialLoad = true
  self.searchClient = liteClient('SK049MNMX7', '79d4022f9d2199e9dcc43221a5171c43')
  self.search = null
  self.searchIndex = self.el.attr('data-index')
  self.dateRange = null

  self.renderSearchBox = (renderOptions, isFirstRender) => {
    const { query, refine } = renderOptions
    const container = document.querySelector('#searchbox')
    if (isFirstRender) {
      const input = document.createElement('input')
      input.placeholder = 'Search by keyword'
      input.addEventListener('input', event => {
        refine(event.target.value)
      })
      container.appendChild(input)
    }
    container.querySelector('input').value = query
  }
  self.customSearchBox = connectSearchBox(
    self.renderSearchBox
  )

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
        return `
          <div class="tour">
            <a href="${item.url}" class="inner">
              <div class="image aspect">
                <img class="lazyload" data-src="${item.articleHeroImage}" alt="">
              </div>

              <div class="copy">
                <div class="destination">${item.destinations.join(', ')}</div>
                <div class="title">${item.articleTitle}</div>
                <div class="description">${item.articleSummary}</div>
              </div>
            </a>
          </div>
          `
      }).join('')
    }
    </div>
    `
  }
  self.customHits = connectHits(self.renderHits)

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('NewArticleListing init', self)
    }
    self.el.css('opacity', 1)

    self.instantSearchRouter = historyRouter()

    self.search = instantsearch({
      indexName: self.searchIndex,
      searchClient: self.searchClient,
      routing: self.instantSearchRouter,
      searchFunction: function (helper) {
        helper.search()
      }
    })

    self.search.addWidgets([
      self.customSearchBox({}),
      self.customHits({}),
      menuSelect({
        container: '#destinationSelect',
        attribute: 'destinations',
        limit: 100
      }),
      menuSelect({
        container: '#categorySelect',
        attribute: 'categories',
        limit: 100
      }),
      menuSelect({
        container: '#experienceSelect',
        attribute: 'experiences',
        limit: 100
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
      pagination({
        container: '#pagination'
      }),
      hitsPerPage({
        container: '#hitsPerPage',
        items: [
          { label: '30', value: 30, default: true },
          { label: '60', value: 60 }
        ],
      })
    ])

    self.search.start()

    setTimeout(() => {
      self.el.find('select').addClass('ui fluid').dropdown({
        placeholder: false
      })
    }, 500)
  }
}
