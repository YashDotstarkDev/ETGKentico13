if (process.env.NODE_ENV === 'development') {
  require('./experiences.njk')
}

import('../widgets/page-hero/page-hero')
import('../widgets/experience-cta/experience-cta')
