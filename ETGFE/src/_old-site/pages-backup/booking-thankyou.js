if (process.env.NODE_ENV === 'development') {
  require('./booking-paid-thankyou.njk')
  require('./booking-quote-thankyou.njk')
  require('./booking-other-thankyou.njk')
}

import('../widgets/new-hero/new-hero')
import('../widgets/booking-thankyou/booking-thankyou')
