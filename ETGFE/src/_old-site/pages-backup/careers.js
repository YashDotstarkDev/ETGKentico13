if (process.env.NODE_ENV === 'development') {
  require('./careers.njk')
}

import('../widgets/page-hero/page-hero')
import('../widgets/career-callout/career-callout')
import('../widgets/testimonial/testimonial')
import('../widgets/career-opportunities/career-opportunities')
