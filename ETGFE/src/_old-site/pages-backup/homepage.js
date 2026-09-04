if (process.env.NODE_ENV === 'development') {
  require('./homepage.njk')
}

import('../widgets/homepage-hero-al-search/homepage-hero-al-search')
import('../widgets/explore-destinations/explore-destinations')
import('../widgets/articles-section/articles-section')
import('../widgets/featured-tiles/featured-tiles')
import('../widgets/destination-expert/destination-expert')
import('../widgets/game-of-luck/game-of-luck')
import('../widgets/faqs/faqs')
