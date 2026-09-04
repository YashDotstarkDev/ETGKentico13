'use strict';

window.dbs = window.dbs || {};
dbs.utilities = dbs.utilities || {};

dbs.utilities.parallax = function () {
  var self = this;
  self.events = new dbs.events();

  self.init = function(el) {
    self.el = $(el);
    self.content = self.el.find('.parallax-content');
    self.visibility = new dbs.utilities.visibility(self.el);
    self.visibility.events.subscribe('Visibility:update', function (measurements) {
      if(measurements.element.isOnScreen) {
        if(measurements.element.startsOnScreen) {
          self.reposition(measurements.element.percentagePassed.totalFromStart);
        } else {
          self.reposition(measurements.element.percentagePassed.total);
        }
      }
    });
  };

  self.reposition = function (percentagePassed) {
    var heightDifference = self.content.height() - self.el.height();
    var offset = heightDifference / 100 * percentagePassed;

    self.content.css('transform', 'translate3d(0,' + offset + 'px,0)');
  };
};
