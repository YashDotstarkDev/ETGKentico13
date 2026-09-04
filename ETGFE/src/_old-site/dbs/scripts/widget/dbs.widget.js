'use strict';

window.dbs = window.dbs || {};
dbs.widgets = dbs.widgets || [];

dbs.widget = function (className) {
  var self = this;
  self.className = className;
  self.dependencies = [];
  dbs.widgets.push(self);

  self.addDependency = function (src) {
    var dependency = new dbs.dependency(src);
    dependency.events.subscribe('Dependency:loaded', function() {
      self.checkDependencies();
    });
    self.dependencies.push(dependency);
  };

  self.checkDependencies = function () {
    var dependenciesLoaded = true;
    for (var i = 0; i < self.dependencies.length; i++) {
      if(!self.dependencies[i].loaded) {
        dependenciesLoaded = false;
        break;
      }
    }
    if(dependenciesLoaded) {
      $('.widget.' + className).each(function(i, el){
        $(el).addClass('loaded');

        if(window.dbs.loggingEnabled) {
          console.log('WIDGET ' + className + ' LOADED');
        }
        if($(el).find('.make-dropdown').length) {
          $(el).find('.make-dropdown').dropdown({
            placeholder: false
          });
        }

        if($(el).find('.make-checkbox').length) {
          $(el).find('.ui.checkbox').checkbox();
        }

        if($(el).find('.ui .tooltip').length) {
          $(el).find('.ui .tooltip').popup();
        }

        if($(el).data('widget')) {
          $(el).data('widget').init();
        }
      });
    }
  }
};
