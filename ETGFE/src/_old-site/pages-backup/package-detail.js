if (process.env.NODE_ENV === 'development') {
  require('./package-detail.njk')
  require('./package-detail-completed-booking.njk')
}

// Hero content
import('../widgets/new-package-hero/new-package-hero')

// Main content
import('../widgets/package-detail-title/package-detail-title')
import('../widgets/package-detail-bar/package-detail-bar')
import('../widgets/package-intro/package-intro')
import('../widgets/package-highlights/package-highlights')
import('../widgets/package-overview/package-overview')
import('../widgets/peace-booking-plan/peace-booking-plan')
import('../widgets/package-inclusions/package-inclusions')
import('../widgets/package-accordion/package-accordion')
import('../widgets/package-itinerary/package-itinerary')
import('../widgets/package-additional-information/package-additional-information')
import('../widgets/package-sticky-footer/package-sticky-footer')
import('../widgets/package-jump-menu/package-jump-menu')

// Side content
import('../widgets/package-offer/package-offer')
import('../widgets/discounts-offers/discounts-offers')
import('../widgets/new-book-now/new-book-now')
import('../widgets/departures-pricing/departures-pricing')
import('../widgets/package-brochure/package-brochure')

// Other content
import('../widgets/breadcrumbs/breadcrumbs')
import('../widgets/spacer/spacer')
import('../widgets/package-hotels/package-hotels')
import('../widgets/package-optional-extras/package-optional-extras')
import('../widgets/partnership-with-new/partnership-with-new')
import('../widgets/phone-cta-panel/phone-cta-panel')
import('../widgets/read-more/read-more')
import('../widgets/experience-ticker/experience-ticker')
import('../widgets/new-header/new-header')
import('../widgets/new-footer/new-footer')
import('../widgets/subscribe-to-win-widget/subscribe-to-win-widget')
import('../widgets/peace-booking-plan/peace-booking-plan')
