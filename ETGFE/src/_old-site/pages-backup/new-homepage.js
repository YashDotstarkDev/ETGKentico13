if (process.env.NODE_ENV === 'development') {
  require('./new-homepage.njk')
}

import('../widgets/new-homepage-hero/new-homepage-hero')
import('../widgets/new-homepage-search-bar-v2/new-homepage-search-bar-v2')
import('../widgets/new-experience-bar/new-experience-bar')
