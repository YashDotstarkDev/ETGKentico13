if (process.env.NODE_ENV === 'development') {
  require('./hotelbuilder.njk')
}

import('../styles/includes/buttons.scss')
import('../plugins/semantic/dropdown.scss')
import('../plugins/semantic/transition.css')
import('../plugins/semantic/transition.js')
import('../widgets/hotel-builder/hotel-builder')
