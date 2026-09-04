if (process.env.NODE_ENV === 'development') {
  require('./abcpdf-package-detail-html.njk')
}

import('../widgets/package-hero/package-hero')
import('../widgets/package-intro/package-intro')
import('../widgets/package-highlights/package-highlights')
import('../widgets/read-more/read-more')
import('../widgets/package-inclusions/package-inclusions')
import('../widgets/package-itinerary/package-itinerary')
import('../widgets/package-additional-information/package-additional-information')
import('../widgets/package-hotels/package-hotels')
import('../widgets/phone-cta-panel/phone-cta-panel')
import('../widgets/also-like/also-like')
import('../widgets/departures-pricing/departures-pricing')

