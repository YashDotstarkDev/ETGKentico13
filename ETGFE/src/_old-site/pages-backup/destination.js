if (process.env.NODE_ENV === 'development') {
  require('./destination.njk')
}

import('../widgets/package-hero/package-hero')
import('../widgets/page-hero/page-hero')
import('../widgets/read-more/read-more')
import('../widgets/package-brochure/package-brochure')
import('../widgets/stats-side/stats-side')
import('../widgets/phone-cta-panel/phone-cta-panel')
import('../widgets/related-articles/related-articles')
import('../widgets/information-accordion/information-accordion')
import('../widgets/featured-tiles/featured-tiles')
import('../widgets/region-map/region-map')
import('../widgets/svg-map/svg-map')
