if (process.env.NODE_ENV === 'development') {
  require('./articles.njk')
}

import('../widgets/new-article-listing/new-article-listing')
