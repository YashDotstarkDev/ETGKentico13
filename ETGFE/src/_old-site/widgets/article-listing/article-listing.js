Promise.all([
  import(/* webpackMode: "eager" */ './article-listing.scss'),
  import('../../plugins/semantic/form.scss'),
  import('../../plugins/semantic/form.js'),
  import('../../plugins/paginationjs/pagination.css'),
  import('../../plugins/paginationjs/pagination.js')
]).then(() => {
  $('.widget.article-listing').each(function (i, el) {
    $(el).data('widget', new ArticleListing(el))
    $(el).data('widget').init()
  })
})

function ArticleListing (el) {
  const self = this;
  self.el = $(el);
  self.articleTemplate = require('../article-listing/article-listing.hbs');

  self.init = function () {
    if(process.env.NODE_ENV === 'development') {
      console.log('ArticleListing init', self);
    }
    self.el.css('opacity', 1);

    self.el.find('.mobile-menu-trigger').click(function(event) {
      event.preventDefault();
      self.el.addClass('show-mobile-subnav');
    });

    self.el.find('.mobile-nav .close').click(function(event) {
      event.preventDefault();
      self.el.removeClass('show-mobile-subnav');
    });

    var initialPageNumber = 1;
    var pageHash = window.location.hash;
    if (pageHash != null && pageHash.length > 1){
      var page = pageHash.substring(1,pageHash.length);
      initialPageNumber = Number(page);
    }
    var totalPageNumber = Number(self.el.find('.list').attr('data-total'));

    if (totalPageNumber <1){
      totalPageNumber = 1
    }

    self.el.find('.pagination').pagination({
      dataSource: self.el.find('.list').attr('data-endpoint'),
      locator: 'searchResult.results',
      showGoInput: true,
      prevText: 'Previous',
      nextText: 'Next',
      showNavigator: true,
      formatNavigator: 'of <%= totalPage %>',
      showPageNumbers: false,
      pageSize: 8,
      pageNumber:initialPageNumber,
      totalNumber : totalPageNumber,
      /*totalNumberLocator: function(response) {
        // you can return totalNumber by analyzing response content
        console.log(response);
        return response.searchResult.totalCount;
      },*/
      ajax: {
        beforeSend: function() {
          self.el.find('.list').html('Loading ...');
        }
      },
      callback: function(data, pagination) {
        // template method of yourself
        var html = self.articleTemplate({
         data: data
        });
        self.el.find('.list').html(html);
        console.log(data, pagination);

        if (pagination.pageNumber != 1){
          $('html, body').animate({scrollTop: self.el.find('.list').offset().top - 120})
        }

        self.el.find('.paginationjs-go-input input').val(pagination.pageNumber);
      }
    })

    self.el.find('.pagination').addHook('afterNextOnClick', function (e, page){
      window.location.hash = page;
    });

    self.el.find('.pagination').addHook('afterPreviousOnClick', function (e, page){
      window.location.hash = page;
    });

  }
}
