'use strict';

window.dbs = window.dbs || {};
dbs.utilities = dbs.utilities || {};

dbs.utilities.visibility = function (el) {
  var self = this;
  self.el = el;
  self.data = {};
  self.events = new dbs.events();

  self.measurements = {
    window: {},
    document: {},
    element: {
      pixelOffset: {
        top: {},
        middle: {},
        bottom: {}
      },
      percentagePassed: {}
    }
  };

  self.calculate = function () {

    // window
    self.measurements.window.width = $(window).width();
    self.measurements.window.height = $(window).height();

    // element
    self.measurements.element.width = self.el.outerWidth();
    self.measurements.element.height = self.el.outerHeight();
    self.measurements.element.offsetTop = self.el.offset().top;

    // document
    self.measurements.document.scrollTop = $(document).scrollTop();

    // pixel offsets

    // element top
    self.measurements.element.pixelOffset.top.windowTop = self.measurements.element.offsetTop - self.measurements.document.scrollTop;
    self.measurements.element.pixelOffset.top.windowMiddle = self.measurements.element.offsetTop - (self.measurements.window.height / 2) - self.measurements.document.scrollTop;
    self.measurements.element.pixelOffset.top.windowBottom = self.measurements.element.offsetTop - self.measurements.window.height - self.measurements.document.scrollTop;

    // element middle
    self.measurements.element.pixelOffset.middle.windowTop = self.measurements.element.offsetTop + (self.measurements.element.height / 2) - self.measurements.document.scrollTop;
    self.measurements.element.pixelOffset.middle.windowMiddle = self.measurements.element.offsetTop + (self.measurements.element.height / 2) - (self.measurements.window.height / 2) - self.measurements.document.scrollTop;
    self.measurements.element.pixelOffset.middle.windowBottom = self.measurements.element.offsetTop + (self.measurements.element.height / 2) - self.measurements.window.height - self.measurements.document.scrollTop;

    // element bottom
    self.measurements.element.pixelOffset.bottom.windowTop = self.measurements.element.offsetTop + self.measurements.element.height - self.measurements.document.scrollTop;
    self.measurements.element.pixelOffset.bottom.windowMiddle = self.measurements.element.offsetTop + self.measurements.element.height - (self.measurements.window.height / 2) - self.measurements.document.scrollTop;
    self.measurements.element.pixelOffset.bottom.windowBottom = self.measurements.element.offsetTop + self.measurements.element.height - self.measurements.window.height - self.measurements.document.scrollTop;


    // percentages
    var totalScroll = self.measurements.element.height + self.measurements.window.height;
    self.measurements.element.percentagePassed.total = 100 / (totalScroll / (self.measurements.element.pixelOffset.top.windowBottom * -1));
    self.measurements.element.percentagePassed.top = 100 / (self.measurements.window.height / (self.measurements.element.pixelOffset.top.windowBottom * -1));
    self.measurements.element.percentagePassed.bottom = 100 / (self.measurements.window.height / (self.measurements.element.pixelOffset.bottom.windowBottom * -1));

    self.measurements.element.percentagePassed.totalFromStart = 100 - (100 / ((self.measurements.element.height + self.measurements.element.offsetTop) / self.measurements.element.pixelOffset.bottom.windowTop));



    // booleans
    self.measurements.element.isOnScreen = self.measurements.element.pixelOffset.top.windowBottom < 0 && self.measurements.element.pixelOffset.bottom.windowTop > 0;
    self.measurements.element.startsOnScreen = self.measurements.element.offsetTop < self.measurements.window.height;


    self.events.emit('Visibility:update', self.measurements);
  };


  $(document).on('scroll', function(){
    self.calculate();
  });
  $(window).on('resize', function(){
    self.calculate();
  });
  $('body').on('touchmove', function(){
    self.calculate();
  });

  self.calculate();


};
