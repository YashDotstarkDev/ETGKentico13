'use strict';

window.dbs = window.dbs || {};
dbs.utilities = dbs.utilities || {};

dbs.utilities.titleCase = function (str) {
  return str.replace(/\w\S*/g, function(txt){
    return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();
  });
};
