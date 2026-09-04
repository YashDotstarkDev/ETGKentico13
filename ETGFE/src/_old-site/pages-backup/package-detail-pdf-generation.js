if (process.env.NODE_ENV === 'development') {
  require('./package-detail-pdf-generation.njk')
}

/*
* NOTE webpackMode: "eager" import everything required to correctly style the print page used for package PDF file generations.
* This will force the imports to all be bundled into a single file, that can be loaded without any lazy loaded assets.
* If any assets is lazy loaded, they may not be loaded before th PDF is generated, causing content display issues.
* */


/*
* Import any global stuff, so we have no dependent files. We only want to load this one bundle file on the page template.
* */
import(/* webpackMode: "eager" */ '../styles/global.scss')
import(/* webpackMode: "eager" */ '../styles/includes/buttons.scss')
import(/* webpackMode: "eager" */ '../plugins/semantic/form.scss')

/*
* Import any plugins or widgets that are dependents of the widgets below, so we can 'eager' import them into this bundle.
* So they are not lazy loaded from widgets, but bundled into this single bundle file.
* */
import(/* webpackMode: "eager" */ '../plugins/semantic/accordion.css')
import(/* webpackMode: "eager" */ '../widgets/product-tile/product-tile.scss') // NOTE sometimes used as content in package-accordion.


/*
*  Import all required page widgets.
* */

// Hero content
import(/* webpackMode: "eager" */ '../widgets/new-package-hero/new-package-hero.scss')

// Main content
import(/* webpackMode: "eager" */ '../widgets/package-detail-title/package-detail-title.scss')
import(/* webpackMode: "eager" */ '../widgets/package-detail-bar/package-detail-bar.scss')
import(/* webpackMode: "eager" */ '../widgets/package-intro/package-intro.scss')
import(/* webpackMode: "eager" */ '../widgets/package-highlights/package-highlights.scss')
import(/* webpackMode: "eager" */ '../widgets/package-overview/package-overview.scss')
import(/* webpackMode: "eager" */ '../widgets/peace-booking-plan/peace-booking-plan.scss')
import(/* webpackMode: "eager" */ '../widgets/package-inclusions/package-inclusions.scss')
import(/* webpackMode: "eager" */ '../widgets/package-accordion/package-accordion.scss')
import(/* webpackMode: "eager" */ '../widgets/package-itinerary/package-itinerary.scss')
import(/* webpackMode: "eager" */ '../widgets/package-additional-information/package-additional-information.scss')
import(/* webpackMode: "eager" */ '../widgets/package-sticky-footer/package-sticky-footer.scss')

// Other content
import(/* webpackMode: "eager" */ '../widgets/spacer/spacer.scss')
import(/* webpackMode: "eager" */ '../widgets/package-hotels/package-hotels.scss')
import(/* webpackMode: "eager" */ '../widgets/package-optional-extras/package-optional-extras.scss')
import(/* webpackMode: "eager" */ '../widgets/partnership-with-new/partnership-with-new.scss')
import(/* webpackMode: "eager" */ '../widgets/phone-cta-panel/phone-cta-panel.scss')
import(/* webpackMode: "eager" */ '../widgets/package-enquiry/package-enquiry.scss')
import(/* webpackMode: "eager" */ '../widgets/tour-tiles-slider/tour-tiles-slider.scss')
import(/* webpackMode: "eager" */ '../widgets/read-more/read-more.scss')
import(/* webpackMode: "eager" */ '../widgets/experience-ticker/experience-ticker.scss')
import(/* webpackMode: "eager" */ '../widgets/new-featured-packages/new-featured-packages.scss')
import(/* webpackMode: "eager" */ '../widgets/new-header/new-header.scss')


/*
* Import this bundles stylesheet. Used to directly influence the styles of this page. Can be used to tweak widget
* styles by overriding to ensure they are good for printing/PDF generation.
* */

import(/* webpackMode: "eager" */ './package-detail-pdf-generation.scss')
