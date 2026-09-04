if (process.env.NODE_ENV === 'development') {
  require('./find-a-package.njk')
}

import(/* webpackMode: "eager" */ '../widgets/find-a-package/find-a-package')
import('../widgets/article-listing/article-listing')
import('../widgets/products/products')
