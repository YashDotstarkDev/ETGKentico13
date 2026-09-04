'use strict';

window.dbs = window.dbs || {};
dbs.utilities = dbs.utilities || {};

dbs.utilities.sticky = function () {
  var self = this;
  self.events = new dbs.events();

  self.stickInParent = function(el, topOffset) {
    self.el = $(el);

    self.topOffset = topOffset ? topOffset : 20;

    self.calculateOffsets();

    $(document).on('scroll', function(){
      self.update();
    });
    $(window).on('resize load', function(){
      self.calculateOffsets();
      self.update();
    });
    $('body').on('touchmove', function(){
      self.update();
    });
  };

  self.calculateOffsets = function () {
    self.top = self.el.parent().offset().top - self.topOffset;
    self.bottom = self.top + self.el.parent().outerHeight() - self.el.outerHeight();
  };

  self.update = function () {
    var st = $(document).scrollTop();

    // unstick
    if(st < self.top) {
      self.el.css({
        'position': 'relative',
        'top': 'auto',
        'bottom': 'auto',
        'left': 'auto',
        'width': 'auto'
      });

      if(self.el.hasClass('stuck-top')) {
        self.events.emit('Sticky:unstick', null, true);
      }

      self.el.removeClass('stuck-top');
    }

    // stick top
    if(st >= self.top && st < self.bottom) {
      self.el.removeClass('stuck-bottom');

      self.el.css({
        'position': 'fixed',
        'top': self.topOffset + 'px',
        'bottom': 'auto',
        'left': self.el.parent().offset().left,
        'width': self.el.parent().width()
      });

      if(!self.el.hasClass('stuck-top')) {
        self.events.emit('Sticky:stick_top', null, true);
        self.el.addClass('stuck-top');
      }
    }

    // stick bottom
    if(st >= self.bottom) {
      self.el.removeClass('stuck-top');

      self.el.css({
        'position': 'absolute',
        'top': 'auto',
        'bottom': '0',
        'left': '0',
        'width': self.el.parent().width()
      });

      if(!self.el.hasClass('stuck-bottom')) {
        self.events.emit('Sticky:stick_bottom', null, true);
        self.el.addClass('stuck-bottom');
      }
    }
  };
};
