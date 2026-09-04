'use strict';

window.dbs = window.dbs || {};
dbs.dependencies = {
  js: [],
  css: []
};
dbs.loadDependencies = function () {
  for (var i = 0; i < dbs.dependencies.css.length; i++) {
    var css = dbs.dependencies.css[i];
    css.load();
  }
  for (var i = 0; i < dbs.dependencies.js.length; i++) {
    var js = dbs.dependencies.js[i];
    js.load();
  }

  loadJS('/custom/plugins/filament/cssrelpreload.js', function() {
    console.log('loaded polyfill')
  }, true);
};

dbs.stylesheetLoaded = function(src) {
  for (var i = 0; i < dbs.dependencies.css.length; i++) {
    var css = dbs.dependencies.css[i];
    if(src === css.src) {
      css.loaded = true;
      css.events.emit('Dependency:loaded', css, false);
    }
  }
};

dbs.dependency = function (src) {
  var self = this;
  self.events = new dbs.events();

  self.src = src;
  self.loaded = false;
  self.type = src.substr(src.lastIndexOf('.') + 1);

  // override type for google fonts
  if(self.src.indexOf('fonts.googleapis.com') !== -1) {
    self.type = 'css';
  }

  self.collection = self.type;
  if(self.type === 'woff2') {
    self.collection = 'css';
  }

  self.load = function () {

    switch(self.type) {
      case 'css':
        $('<link>', {rel:'preload', as:'style', 'href':self.src, 'onload': 'this.onload=null;this.rel=\'stylesheet\';dbs.stylesheetLoaded(\'' + self.src + '\')'}).appendTo('head');
        // self.cssLoader = loadCSS(self.src);
        // onloadCSS( self.cssLoader, function() {
        //   self.loaded = true;
        //   self.events.emit('Dependency:loaded', self, false);
        // });
        break;
      case 'woff2':
        $('<link>', {rel:'preload', as:'font', 'href':self.src, 'crossorigin': 'anonymous', 'onload': 'this.onload=null;this.rel=\'font\';dbs.stylesheetLoaded(\'' + self.src + '\')'}).appendTo('head');
        break;
      case 'js':
        loadJS(self.src, function() {
          self.loaded = true;
          self.events.emit('Dependency:loaded', self, false);
        }, true);
        break;
    }
  };

  if (dbs.dependencies[self.collection].filter(function(e) { return e.src === self.src; }).length === 0) {
    dbs.dependencies[self.collection].push(self);
    return self;
  } else {
    return dbs.dependencies[self.collection].filter(function(e) { return e.src === self.src; })[0];
  }



};
