// import(/* webpackMode: "eager" */ '../styles/global.scss')
import('../styles/includes/buttons.scss')
import('../plugins/semantic/form.scss')
import('../plugins/semantic/checkbox.css')
import('../plugins/semantic/dropdown.scss')
import('../plugins/semantic/transition.css')


require('../dbs/scripts/events/dbs.events')
require('../dbs/scripts/request/dbs.request')
require('../dbs/scripts/response/dbs.response')
require('../plugins/semantic/form.js')
require('../plugins/semantic/checkbox.js')
require('../plugins/semantic/transition.js')
require('../plugins/semantic/dropdown.js')
require('../plugins/match-height/jquery.matchHeight.js')
require('../plugins/jquery/jquery.unobtrusive-ajax')

require('../plugins/clampjs/clamp')

function updateClamp () {
    $('.clamp1').each(function (index, el) {
        $clamp($(el).get(0), { clamp: 1 })
    })
    $('.clamp2').each(function (index, el) {
        $clamp($(el).get(0), { clamp: 2 })
    })
    $('.clamp3').each(function (index, el) {
        $clamp($(el).get(0), { clamp: 3 })
    })
}

$(document).ready(function () {
    $('.clamp1').each(function (index, el) {
        $clamp($(el).get(0), { clamp: 1 })
    })
    $('.clamp2').each(function (index, el) {
        $clamp($(el).get(0), { clamp: 2 })
    })
    $('.clamp3').each(function (index, el) {
        $clamp($(el).get(0), { clamp: 3 })
    })

    $('.expiry').on('keyup', function () {
        var value = $(this).val()

        value = value.split('/').join('')
        var slashValue = value.match(/.{1,2}/g).join('/')

        $(this).val(slashValue)
    })

    // NOTE Calendly is loaded from an external service.
    $('.calendry-button').click(function (e) {
        e.preventDefault()
        if (window.Calendly) {
            window.Calendly.showPopupWidget($(this).attr('data-link'))
        } else {
            console.info('Calendly is not found.')
        }
    })
})