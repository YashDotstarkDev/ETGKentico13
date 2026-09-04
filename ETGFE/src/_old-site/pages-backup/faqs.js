if (process.env.NODE_ENV === 'development') {
  require('./faqs.njk')
}

import('../widgets/faqs-page/faqs-page')
import('../widgets/generic-enquire-side/generic-enquire-side')
import('../widgets/filepicker/filepicker')
import('../widgets/side-email-order/side-email-order')
import('../widgets/feature-box/feature-box')
import('../plugins/semantic/divider.css')
