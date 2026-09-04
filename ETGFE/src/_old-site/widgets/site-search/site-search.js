import { dom } from '@fortawesome/fontawesome-svg-core'

Promise.all([
  import(/* webpackMode: "eager" */ '../../plugins/semantic/form.js')
]).then(() => {
  Promise.all([
    import(/* webpackMode: "eager" */ '../type-ahead/global-typeahead.scss'),
    import(/* webpackMode: "eager" */ './site-search.scss'),
    import('../../plugins/semantic/form.scss'),
    import('../../dbs/scripts/form/dbs.semantic.form.js'),
    import('../../plugins/semantic/api.js'),
    import('../../plugins/semantic/search'),
    import('../../plugins/match-height/jquery.matchHeight.js'),
    import('../../plugins/semantic/search.css')
  ]).then(() => {
    $('.widget.site-search').each(function (i, el) {
      $(el).data('widget', new SiteSearch(el))
      $(el).data('widget').init()
    })
  })
})

function SiteSearch (el) {
  const self = this;
  self.el = $(el);
  self.searchTemplate = require('../type-ahead/global-typeahead.hbs');
  self.events = new dbs.events();


  self.init = function () {

    if(process.env.NODE_ENV === 'development') {
      console.log('SiteSearch init', self);
    }
    self.el.css('opacity', 1);

    $.fn.search.settings.templates.globalSearch = function(response) {
      return self.searchTemplate(response);
    };

    // init generic form to validate and redirect on submit click
    self.searchForm = new dbs.form.genericForm();
    self.searchForm.init(self.el.find('.ui.form.search-form'));
    self.searchForm.events.subscribe('Form:validation_success', function(data) {

      window.location.href = self.el.find('.ui.search').attr('data-search-url') + data.values.search
    });

    if (self.el.hasClass('has-type-ahead')){

      self.search = self.el.find('.ui.search').search({
        type: 'globalSearch',
        cache: false,
        apiSettings: {
          url: self.el.find('.ui.search').attr('data-endpoint'),
          onResponse: function (results) {
            if(!results.results.destinations && !results.results.tours && !results.results.inspired) {
              results.results = null;
            }
            console.log(results);
            setTimeout(function(){
              $('.links').matchHeight();
            }, 100);

            return results;
          }
        },
        minCharacters: 3,
        selectFirstResult: true,
        showNoResults: true,
        onResultsAdd: function (html) {
          self.events.emit('SearchTypeahead: results_added', html, true);
        },
        onSelect: function (result) {
          self.events.emit('SearchTypeahead: result_selected', result, true);
          self.selectedResult = result;
          self.search.search('hide results');
          return false;
        },
        onSearchQuery: function (query) {
          self.events.emit('SearchTypeahead: search_query', query, true);
          self.selectedResult = null;
          self.query = query;
        },
        onResults: function (response) {
          self.events.emit('SearchTypeahead: results_retrieved', response, true);

          setTimeout(function(){
            $('.links').matchHeight();
          }, 100);


          dom.i2svg();

        }
      });

    }


  }
}


