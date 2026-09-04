import { dom } from '@fortawesome/fontawesome-svg-core'
import {liteClient} from 'algoliasearch/lite'
import instantsearch from 'instantsearch.js'
import {
  hits,
  stats,
  pagination,
  hitsPerPage, sortBy, menuSelect, rangeSlider, toggleRefinement
} from 'instantsearch.js/es/widgets'

import('../../plugins/numeraljs/numeral.min.js').then(({ default: numeral }) => {
  window.numeral = numeral
  import('lodash').then(({ default: lodash }) => {
    window._ = lodash
    Promise.all([
      import(/* webpackMode: "eager" */ './travel-deals.scss'),
      import('../../plugins/semantic/form.scss'),
      import('../../plugins/semantic/form.js'),
      import('../../dbs/scripts/form/dbs.semantic.form.js'),
      import('../../plugins/semantic/dropdown.scss'),
      import('../../plugins/semantic/dropdown.js'),
      import('../../plugins/match-height/jquery.matchHeight.js'),
      import('../../plugins/rangeslider/ion.rangeSlider.css'),
      require('../../plugins/rangeslider/ion.rangeSlider.js'),
    ]).then(() => {
      $('.widget.travel-deals').each(function (i, el) {
        $(el).data('widget', new TravelDeals(el))
        $(el).data('widget').init()
      })
    })
  })
})

function TravelDeals (el) {
  const self = this;
  self.el = $(el);

  self.updateLabels = function(data){
    self.el.find('.from-value').html(numeral(data.from).format('$0,0'));
    self.el.find('.to-value').html(numeral(appenddata.to).format('$0,0'));

    console.log(self.el.find('input[name="range"]').val());

  };

  self.handleRender = function () {

    var max = self.el.find('.rheostat-handle-upper .rheostat-tooltip').html();
    var min = self.el.find('.rheostat-handle-lower .rheostat-tooltip').html();

    self.el.find('.from-value').html(numeral(min).format('$0,0'));
    self.el.find('.to-value').html(numeral(max).format('$0,0'));

    setTimeout(function(){
      $('.description').matchHeight();
      $('.title').matchHeight();
    }, 100);


    dom.i2svg();

    self.el.find('#deal_sort select, #deal_view select, .ais-MenuSelect-select').addClass('make-dropdown');
    self.el.find('.make-dropdown').dropdown({
      placeholder: false
    });
  }

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('TravelDeals init', self);
    }
    self.el.css('opacity', 1);


    self.searchClient = liteClient('SK049MNMX7', '79d4022f9d2199e9dcc43221a5171c43');

    setTimeout(function(){
      self.el.find('.bottom[data-tab="' + $('.nav a.active').attr('data-tab') + '"]').fadeIn();
      self.handleRender();
      self.el.find('.description')

    }, 1000);



    // tour search
    self.deals = instantsearch({
      indexName: $(el).attr('data-indexname'),
      searchClient: self.searchClient,
      searchFunction: function(helper) {
       helper.state.facets = ['HasDiscount'];
        helper.addFacetRefinement('HasDiscount', true).search();
        setTimeout(function () {
          self.handleRender();

        }, 100)
      }
    });

    self.deals.addWidgets([
      hits({
        container: "#deal_hits",
        templates: {
          item: `
        <div class="item" data-mh="even-item">
        <a href="{{Url}}" class="inner" data-mh="even-inner">
          <img class="lazyload" data-src="{{TourHeroImage}}" alt="">
          <div class="tint"></div>
          <div class="title" data-mh="even-title">{{DiscountText}}</div>

          <div class="description" data-mh="even-description">{{{TourName}}}</div>
          <div class="button ui primary">View offer</div>
        </a>
      </div>
          `,
        },
      }),
      stats({
        container: '#deal_stats',
        templates: {
          text: `
      {{nbHits}}
    `,
        },
      }),
      // toggleRefinement({
      //   container: '#deals',
      //   attribute: 'HasDiscount',
      //   on: 'true'
      // }),
      sortBy({
        container: '#deal_sort',
        items: [
          { label: 'Oldest to newest', value: 'Tour' },
          { label: 'Newest to oldest', value: 'tour_date_desc' },
          { label: 'Alphabetical a to z', value: 'tour_alpha_asc' },
          { label: 'Alphabetical z to a', value: 'tour_alpha_desc' },
          { label: 'Duration short to long', value: 'tour_duration_asc' },
          { label: 'Duration long to short', value: 'tour_duration_desc' }
        ],
      }),
      rangeSlider({
        container: '#price_range-slider',
        attribute: 'Price'
      }),
      hitsPerPage({
        container: '#deal_view',
        items: [
          { label: '30', value: 30, default: true },
          { label: '60', value: 60 }
        ]
      }),
      pagination({
        container: '#deal_pagination'
      }),
      menuSelect({
        container: '#destination_dd',
        attribute: 'DestinationNameList'
      }),
      menuSelect({
        container: '#experience_dd',
        attribute: 'ExperienceNameList'
      }),
      menuSelect({
        container: '#duration_dd',
        attribute: 'TourNights',
        templates: {
          item:
            '{{label}} Nights ({{count}})',
        },
      })
    ]);
    self.deals.start();


    self.el.find('.make-dropdown').dropdown({
      placeholder: false
    });


    self.el.find('.rangeslider').ionRangeSlider({
      type: "double",
      grid: false,
      skin:"big",
      onStart: function (data) {
        self.updateLabels(data);
      },
      onChange: function(data){
        self.updateLabels(data);
      }
    });
  }
}


