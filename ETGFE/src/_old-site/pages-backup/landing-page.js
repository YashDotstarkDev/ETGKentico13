if (process.env.NODE_ENV === 'development') {
  require('./landing-page.njk')
}
import(/* webpackMode: "eager" */ './landing-page.scss')
import('../widgets/homepage-hero/homepage-hero')
import('../widgets/landing-hero/landing-hero')
import('../widgets/landing-sticky-footer/landing-sticky-footer')
import('../widgets/form/form')
