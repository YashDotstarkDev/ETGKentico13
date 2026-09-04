(function (window) {
    'use strict';

    function define_UtmProcessor() {
        var UtmProcessor = {};
        UtmProcessor.readCookie = function (name) {
            var cookiename = name + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ')
                    c = c.substring(1, c.length);
                if (c.indexOf(cookiename) == 0)
                    return c.substring(cookiename.length, c.length);
            }
            return null;
        }

        UtmProcessor.setCookie = function (cname, cvalue, exdays) {
            var d = new Date();
            d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
            var expires = "expires=" + d.toUTCString();
            document.cookie = cname + "=" + cvalue + ";path=/; " + expires;
        }
        UtmProcessor.getUrlParams = function () {
            // This function is anonymous, is executed immediately and
            // the return value is assigned to QueryString!
            var query_string = {};
            var query = window.location.search.substring(1);
            var vars = query.split("&");
            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split("=");
                // If first entry with this name
                if (typeof query_string[pair[0]] === "undefined") {
                    query_string[pair[0]] = decodeURIComponent(pair[1]);
                    // If second entry with this name
                } else if (typeof query_string[pair[0]] === "string") {
                    var arr = [query_string[pair[0]],
                        decodeURIComponent(pair[1])];
                    query_string[pair[0]] = arr;
                    // If third or later entry with this name
                } else {
                    query_string[pair[0]].push(decodeURIComponent(pair[1]));
                }
            }
            return query_string;
        };
        UtmProcessor.init = function () {
            var params = UtmProcessor.getUrlParams();
            if (!params[""]) {
                var cookieStr = UtmProcessor.readCookie("etgUtm");

                if (typeof cookieStr === "undefined"
                    || cookieStr === "undefined"
                    || cookieStr == null
                    || cookieStr === "null"
                    || cookieStr === ""
                    || cookieStr === "{}") {
                    var isUtmParam = false;
                    var cookie = {};
                    for (var key in params) {
                        if ((key.indexOf("utm_source") > -1 ||
                            key.indexOf("utm_medium") > -1 ||
                            key.indexOf("utm_term") > -1 ||
                            key.indexOf("utm_content") > -1 ||
                            key.indexOf("utm_campaign") > -1)) {
                            cookie[key] = params[key];
                            isUtmParam = true;
                        }
                    }
                    if (isUtmParam === true)
                        UtmProcessor.setCookie("etgUtm", JSON.stringify(cookie), 100);
                } else {
                    var newCookie = {};
                    var isNewCookie = false;

                    var parsedCookie = JSON.parse(cookieStr);
                    for (var keyItem in params) {
                        if (parsedCookie[keyItem] != params[keyItem]) {
                            isNewCookie = true;
                        }
                        if ((keyItem.indexOf("utm_source") > -1 ||
                            keyItem.indexOf("utm_medium") > -1 ||
                            keyItem.indexOf("utm_term") > -1 ||
                            keyItem.indexOf("utm_content") > -1 ||
                            keyItem.indexOf("utm_campaign") > -1)) {
                            newCookie[keyItem] = params[keyItem];
                        }

                    }
                    if (isNewCookie)
                        UtmProcessor.setCookie("etgUtm", JSON.stringify(newCookie), 100);
                }
            }
        };
        return UtmProcessor;
    }

    // define globally if it doesn't already exist
    if (typeof (UtmProcessor) === 'undefined') {
        window.UtmProcessor = define_UtmProcessor();
        window.UtmProcessor.init();
    } else {
        console.log("UtmProcessor already defined.");
        window.UtmProcessor.init();
    }

})(window);