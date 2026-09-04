import('dayjs').then(({ default: dayjs }) => {
  Promise.all([
    import( './new-book-now.scss'),
    import('../../dbs/scripts/form/dbs.semantic.form.js'),
    import('../../plugins/semantic/form.scss'),
    import('../../plugins/semantic/modal.css'),
    import('../../plugins/semantic/modal.js'),
    import('../../plugins/semantic/popup.css'),
    import('../../plugins/semantic/popup.js'),
    import('../../plugins/semantic/accordion.css'),
    import('../../plugins/semantic/accordion.js'),
    import('../../plugins/semantic/dimmer.css'),
    import('../../plugins/semantic/dimmer.js'),
    import('../../plugins/jquery-ui/custom'),
  ]).then(() => {
    // dayjs plugins
    const isSameOrAfter = require('dayjs/plugin/isSameOrAfter');
    dayjs.extend(isSameOrAfter);
    const isSameOrBefore = require('dayjs/plugin/isSameOrBefore');
    dayjs.extend(isSameOrBefore);
    const customParseFormat = require('dayjs/plugin/customParseFormat');
    dayjs.extend(customParseFormat);

    $('.widget.new-book-now').each(function (i, el) {
      $(el).data('widget', new NewBookNow(el, dayjs));
      $(el).data('widget').init(true);
    });
  });
});



function NewBookNow(el, dayjs) {
  const self = this;

  self.el = $(el);
  self.uiEnabled = true;
  self.currencySymbol = '$';
  self.currency = 'AUD';
  self.initialHtml = '';
  self.initialBookingSummaryHtml = '';
  self.reset = false;
  self.initialApp = {
    hasPeaceOfMind: false,
    hasFreedomOfChoice: false,
    hasNoRoomOptions: true,
    hasEntireFlex: false,
    isOnSaleNow: false,
    focentireflex: false,
    showCalendar: false,
    tourDays: 0,
    hasSingleRoomUpgrade: false,
    cartCompleted: false,
    selectedDepartureDate: '',
    entireFlexIsOffered: true,
    hasPrePostNightPrices:false,
    steps: {
      date: {
        active: true,
        complete: false,
        visible: true,
        label: self.el.find('.step[data-step="date"] .step-trigger .label').html(),
        subLabel: self.el.find('.step[data-step="date"] .sub-label-container .sub-label').html(),
        datesAndPrices: [],
        calendarDates: [],
      },
      travellers: {
        active: false,
        complete: false,
        visible: true,
        label: self.el.find('.step[data-step="travellers"] .step-trigger .label').html(),
        subLabel: self.el.find('.step[data-step="travellers"] .sub-label-container .sub-label').html(),
      },
      roomOptions: {
        active: false,
        complete: false,
        visible: true,
        hasRoomOptions: true,
        roomOptions: [],
        label: self.el.find('.step[data-step="roomOptions"] .step-trigger .label').html(),
        subLabel: self.el.find('.step[data-step="roomOptions"] .sub-label-container .sub-label').html(),
      },
      prePost: {
        active: false,
        complete: false,
        visible: true,
        label: self.el.find('.step[data-step="prePost"] .step-trigger .label').html(),
        subLabel: self.el.find('.step[data-step="prePost"] .sub-label-container .sub-label').html(),
      },
      optionalExtras: {
        active: false,
        complete: false,
        visible: true,
        hasExtras: true,
        extraOptions: [],
        label: self.el.find('.step[data-step="optionalExtras"] .step-trigger .label').html(),
        subLabel: self.el.find('.step[data-step="optionalExtras"] .sub-label-container .sub-label').html(),
      },
      freedomOfChoice: {
        active: false,
        complete: false,
        visible: true,
        label: self.el.find('.step[data-step="freedomOfChoice"] .step-trigger .label').html(),
        subLabel: self.el.find('.step[data-step="freedomOfChoice"] .sub-label-container .sub-label').html(),
        freedomOfChoices: [],
      },
      /*entireFlexOption: {
        active: false,
        complete: false,
        visible: true,
        label: 'Entire Flex Option',
        subLabel: self.el.find('.step[data-step="entireFlexOption"] .step-trigger .sub-label').html()
      },*/
      otherOptions: {
        active: false,
        complete: false,
        visible: true,
        label: self.el.find('.step[data-step="otherOptions"] .step-trigger .label').html(),
        subLabel: self.el.find('.step[data-step="otherOptions"] .sub-label-container .sub-label').html(),
      },
    },
  };
  self.app = {
    hasPeaceOfMind: false,
    hasFreedomOfChoice: false,
    hasNoRoomOptions: true,
    isOnSaleNow: false,
    focentireflex: false,
    showCalendar: false,
    tourDays: 0,
    hasSingleRoomUpgrade: false,
    cartCompleted: false,
    hasEntireFlex: false,
    changeOfMindThresholdDays: 35,
    selectedDepartureDate: '',
    entireFlexIsOffered: true,
    hasPrePostNightPrices:false,
    steps: {
      date: {
        active: true,
        complete: false,
        visible: true,
        label: self.el.find('.step[data-step="date"] .step-trigger .label').html(),
        subLabel: self.el.find('.step[data-step="date"] .sub-label-container .sub-label').html(),
        datesAndPrices: [],
        calendarDates: [],
      },
      prePost: {
        active: false,
        complete: false,
        visible: true,
        label: self.el.find('.step[data-step="prePost"] .step-trigger .label').html(),
        subLabel: self.el.find('.step[data-step="prePost"] .sub-label-container .sub-label').html(),
      },
      travellers: {
        active: false,
        complete: false,
        visible: true,
        label: self.el.find('.step[data-step="travellers"] .step-trigger .label').html(),
        subLabel: self.el.find('.step[data-step="travellers"] .sub-label-container .sub-label').html(),
      },
      freedomOfChoice: {
        active: false,
        complete: false,
        visible: true,
        label: 'Freedom of Choice',
        subLabel: self.el.find('.step[data-step="freedomOfChoice"] .sub-label-container .sub-label').html(),
        freedomOfChoices: [],
      },
      roomOptions: {
        active: false,
        complete: false,
        visible: true,
        hasRoomOptions: true,
        roomOptions: [],
        label: self.el.find('.step[data-step="roomOptions"] .step-trigger .label').html(),
        subLabel: self.el.find('.step[data-step="roomOptions"] .sub-label-container .sub-label').html(),
      },
      optionalExtras: {
        active: false,
        complete: false,
        visible: true,
        hasExtras: true,
        extraOptions: [],
        label: self.el.find('.step[data-step="optionalExtras"] .step-trigger .label').html(),
        subLabel: self.el.find('.step[data-step="optionalExtras"] .sub-label-container .sub-label').html(),
      },
      /*entireFlexOption: {
        active: false,
        complete: false,
        visible: true,
        label: 'Entire Flex Options',
        subLabel: self.el.find('.step[data-step="entireFlexOption"] .step-trigger .sub-label').html()
      },*/
      otherOptions: {
        active: false,
        complete: false,
        visible: true,
        label: 'Other Options',
        subLabel: self.el.find('.step[data-step="otherOptions"] .sub-label-container .sub-label').html(),
      },
    },
  };

  function isInViewport(element) {
    const rect = element.getBoundingClientRect();
    return (
      rect.top >= 0 &&
      rect.left >= 0 &&
      rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
      rect.right <= (window.innerWidth || document.documentElement.clientWidth)
    );
  }

  function scrollToElement(element) {
    let scrollTopOffset = 200; // At least height of site header
    // minus another 100 because francis wasn't happy with it scrolling past the previous
    // https://devotion.atlassian.net/browse/HELP-1735?focusedCommentId=55145
    let scrollDuration = 700;
    let scrollDelay = 440;

    setTimeout(() => {
      $('html, body')
        .stop()
        .animate(
          {
            scrollTop: $(element).position().top - scrollTopOffset,
          },
          scrollDuration
        );
    }, scrollDelay);
  }

  // self.el.click(function (event) {
  // Highlight page content that relates to current booking step.
  /* NOT IN USE
  setTimeout(function () {
    $('.step-content').each(function (i, el) {
      const stepPanel = $(el).parents('.step').attr('data-step')

      if ($(el).is(':visible')) {
        $('.package-accordion .title[data-step="' + stepPanel + '"]').addClass('highlight');
      } else {
        $('.package-accordion .title[data-step="' + stepPanel + '"]').removeClass('highlight');
      }
    });
  }, 500);*/
  // END Highlight page content that relates to current booking step.
  // });

  self.resetSummaryValues = function () {
    $('.booking-summary .label').text('');
    $('.booking-summary .qty').text('');
    $('.booking-summary .price').text('');
    $('.booking-summary .room-list').html('');
    $('.booking-summary .extras-list').html('');
    $('.booking-summary .total-price span').text('');
    $('.booking-summary .total-deposit-price span').text('');
    $('.booking-summary .terms').html('');
  };

  self.resetApp = function () {
    self.el.find('.form-steps').html(self.initialHtml);
    self.resetSummaryValues();
    self.el.find('.step-date').attr('data-endpoint', self.el.find('.step-date').attr('data-endpoint-nonagent'));
    self.app.steps.date.form.unbindSubmit();
    self.app.steps.date.form.init(self.el.find('.step-date'));
    self.el.find('.proceed-button').attr('href', self.el.find('.proceed-button').attr('data-url-nonagent'));
    for (const step in self.app.steps) {
      if (self.app.steps.hasOwnProperty(step)) {
        self.app.steps[step].active = self.initialApp.steps[step].active;
        self.app.steps[step].complete = self.initialApp.steps[step].complete;
        self.app.steps[step].visible = self.initialApp.steps[step].visible;
        self.app.steps[step].label = self.initialApp.steps[step].label;
        self.app.steps[step].subLabel = null;
      }
    }

    self.app.steps['travellers'].label = self.initialApp.steps['travellers'].label;
    self.el.find('.checkbox').checkbox('uncheck');
    self.el.find('.form-steps .step').removeClass('complete');
    self.el.removeClass('cart-complete');
    self.el.removeAttr('data-selecteddate');
    self.el.find('.option-set-container').hide();
    self.el.find('.double-room-suboptions').html('');
    self.el.find('.comment-yes').hide();
    self.el.find('input[type="text"], input[type="email"], textarea').val('');
    self.app.cartCompleted = false;
    self.reset = true;

    self.el.find('.step-date').attr('data-endpoint', self.el.find('.step-date').attr('data-endpoint-nonagent'));
    self.app.steps.date.form.init();
    self.bindDateForm();
  };

  self.resetSucceedingSteps = function (currentStep) {
    //self.el.find('.booking-summary').html(self.initialBookingSummaryHtml);
    self.resetSummaryValues();
    self.el.find('.booking-summary').slideUp();

    for (const step in self.app.steps) {
      if (currentStep === step || (currentStep === 'travellers' && step === 'date') || (currentStep === 'roomOptions' && step === 'date')
             || (currentStep === 'roomOptions' && step === 'travellers')) {
        continue;
      }

      if (self.app.steps.hasOwnProperty(step)) {
        self.app.steps[step].active = self.initialApp.steps[step].active;
        self.app.steps[step].complete = self.initialApp.steps[step].complete;
        if (!(currentStep === 'travellers' && step === 'optionalExtras')) {
          self.app.steps[step].visible = self.initialApp.steps[step].visible;
        }
        self.app.steps[step].label = self.initialApp.steps[step].label;
        self.app.steps[step].subLabel = '';
      }
    }

    self.app.steps['travellers'].label = self.initialApp.steps['travellers'].label;

    self.el.find('.form-steps .step').each(function (e) {
      if ($(this).attr('data-step') !== 'date') {
        $(this).find('.ui.checkbox').checkbox('uncheck');
      }
    });

    self.el.find('.select-later-copy').removeAttr('style');
    self.el.find('.multiple-room-copy').removeAttr('style');
    self.el.find('.option-set-container').hide();
    self.el.find('.form-steps .step').removeClass('complete');
    self.el.removeClass('cart-complete');
    self.el.removeAttr('data-selecteddate');
    self.app.cartCompleted = false;
    self.setInitialStepsVisibility();
    self.bindOtherEvents();
    self.renderUI();
  };

  self.init = function (initialLoad) {
    if (initialLoad) {
      self.initialHtml = self.el.find('.form-steps').html();
      self.initialBookingSummaryHtml = self.el.find('.booking-summary').html();
      $('.dynamic-button').addClass('transparent');
      //self.app.entireFlexIsOffered = self.el.attr('data-offerentireflex') == "true";
    }

    self.el.find('.travel-agent-form .agencyname').on('blur', function () {
      if ($(this).val() === 'Entire Travel Group') {
        try {
          self.el.find('.travel-agent-form .agencypostcode').val('2068');
          self.el.find('.travel-agent-form .agencyphone').val('0290943322');
        } catch (e) {
          // Handle errors
        }
      }
    });

    self.el.css('opacity', 1);

    const symbol = self.el.attr('data-currency-symbol');
    if (symbol) {
      self.currencySymbol = symbol;
    }

    const currency = self.el.attr('data-currency');
    if (currency) {
      self.currency = currency;
    }

    self.app.tourDays = Number(self.el.attr('data-tourdays'));
    self.app.cartCompleted = self.el.hasClass('cart-complete');
    self.app.hasNoRoomOptions = self.el.attr('data-no-roomupgrades') === 'true';

    self.app.isOnSaleNow = self.el.attr('data-isonsalenow') === 'true';
    //self.app.focentireflex = self.el.attr('data-focentireflex') === "true";
    self.app.hasFreedomOfChoice = self.el.attr('data-hasfreedomofchoice') === 'true';
    self.app.hasPeaceOfMind = self.el.attr('data-peaceofmind') === 'true';

    //self.app.hasEntireFlex= self.el.attr('data-hasentireflex') == "true";
    self.app.changeOfMindThresholdDays = Number(self.el.attr('data-changeofmindthresholddays'));
    self.initForms();

    try {
      if (window.performance.getEntriesByType('navigation')[0].type === 'back_forward') {
        location.reload();
      }
    } catch (err) {
      console.log(err);
    }

    if (self.app.cartCompleted) {
      $('.package-sticky-footer').hide();
      self.app.selectedDepartureDate = self.el.attr('data-selecteddate');

      const roomOptionStep = self.el.find('.step[data-step="roomOptions"]');
      if (roomOptionStep) {
        self.app.steps.roomOptions.visible = !roomOptionStep.hasClass('hidden');
      }

      const optionalExtras = self.el.find('.step[data-step="optionalExtras"]');
      if (optionalExtras) {
        self.app.steps.optionalExtras.visible = !optionalExtras.hasClass('hidden');
      }

      self.app.steps.date.active = false;

      for (const step in self.app.steps) {
        if (self.app.steps.hasOwnProperty(step)) {
          self.app.steps[step].complete = true;
        }
      }

      self.el.find('.form-steps').show();

      self.el.find('select').dropdown();

      self.generateBookingSummary();
      self.el.find('.booking-summary').slideDown();
    }

    self.setInitialStepsVisibility();

    // get initial setup
    self.getInitialSetup();
    self.bindEvents();
  };

  self.setInitialStepsVisibility = function () {
    if (self.app.hasNoRoomOptions) {
      self.app.steps.roomOptions.visible = false;
    }

    if (!self.app.hasFreedomOfChoice) {
      self.app.steps.freedomOfChoice.visible = false;
    }

    /*
    if (self.hasEntireFlexStep()){
      self.app.steps.entireFlexOption.visible = true;
    }else{
      self.app.steps.entireFlexOption.visible = false;
    }
    */
  };

  self.getInitialSetup = function () {
    $.ajax({
      url: self.el.find('.endpoints').attr('data-initial') + '?tourcode=' + self.el.attr('data-tourcode'),
      type: self.el.find('.endpoints').attr('data-initial-type'),
      contentType: 'application/json',
    })
      .done(function (response) {
        console.log('getInitialSetup response', response);

        if (response.datesAndPrices?.length === 0) {
          self.el.find('.book-now-trigger').hide();
          return;
        }

        self.app.steps.date.datesAndPrices = response.datesAndPrices;

        if (response.departureDatesOption === 2) {
          $('.form-steps .step[data-step="date"] .calendar-container').remove(); // NOTE remove from DOM to avoid form validation issues
          $('.form-steps .step[data-step="date"] .radio-list').show();
          self.generateDatesAndPricesList();
        } else {
          $('.form-steps .step[data-step="date"] .radio-list').remove(); // NOTE remove from DOM to avoid form validation issues
          $('.form-steps .step[data-step="date"] .calendar-container').show();
          self.generateCalendar();
        }

        self.renderUI();
      })
      .fail(function () {
        console.log('fail');
      })
      .always(function () {
        self.enableUI();
      });
  };

  self.preparePrePostStepContent = function(data){
    
      // PRE dropdown
      let optionsHtml = '<option value=" ">None</option>';
      $.each(data.response.preNights, function (index, value) {
        optionsHtml += '<option value="' + value.value + '">' + value.label + '</option>';
      });
      $('.form-steps .step[data-step="prePost"] select.select-pre').html(optionsHtml);
      let preOptions = [];
      preOptions.push({
          value: ' ',
          text: 'None',
          name: 'None',
      });
      $.each(data.response.preNights, function (index, value) {
          preOptions.push({
              value: value.value,
              text: value.label,
              name: value.label,
          });
      });
      $('.form-steps .step[data-step="prePost"] .select-pre').dropdown('setup menu', {
          values: preOptions,
      });

      // POST dropdown
      optionsHtml = '<option value=" ">None</option>';
      $.each(data.response.postNights, function (index, value) {
          optionsHtml += '<option value="' + value.value + '">' + value.label + '</option>';
      });
      $('.form-steps .step[data-step="prePost"] select.select-post').html(optionsHtml);
      let postOptions = [];
      postOptions.push({
          value: ' ',
          text: 'None',
          name: 'None',
      });
      $.each(data.response.postNights, function (index, value) {
          postOptions.push({
              value: value.value,
              text: value.label,
              name: value.label,
          });
      });
      $('.form-steps .step[data-step="prePost"] .select-post').dropdown('setup menu', {
          values: postOptions,
      });
    
  }

  self.generateCalendar = function () {
    const earliestDate = new dayjs(self.app.steps.date.datesAndPrices[0].departureDate);
    let selectedDate = null;
    let previousDate = null;

    if (self.app.cartCompleted || self.app.selectedDepartureDate) {
      selectedDate = dayjs(self.app.selectedDepartureDate, 'DD/MM/YYYY');
    }

    if (self.reset) {
      selectedDate = null;
      previousDate = null;
      self.el.find('.form-steps .step[data-step="date"] .calendar-date').val('');
    }

    const calendarDatePicker = self.el.find('.calendar').datepicker({
      dateFormat: 'dd/mm/yy',
      showOtherMonths: true,
      firstDay: 1,
      beforeShowDay: function (cd) {
        let enabled = false;
        let cssClass = '';
        let price = '';
        const currentDate = dayjs(cd);
        // enable/disable and set price
        for (let i = 0; i < self.app.steps.date.datesAndPrices.length; i++) {
          const departureDate = dayjs(self.app.steps.date.datesAndPrices[i].departureDate);
          const departurePrice = self.app.steps.date.datesAndPrices[i].price;
          if (currentDate.isSame(departureDate, 'day')) {
            var hasPrePostNightPrice = self.app.steps.date.datesAndPrices[i].hasPrePostNightPrice
            cssClass += ' departure-day ';
            if (hasPrePostNightPrice){
              cssClass += ' hasPrePostNight '
            }
            enabled = true;
            price = self.currencySymbol + self.commaSeparateNumber(departurePrice);
          }
        }
        // first day highlight
        if (selectedDate) {
          if (currentDate.isSame(selectedDate, 'day')) {
            cssClass += ' first-day ui-datepicker-current-day ';
          }
        }
        // highlight
        if (selectedDate) {
          if (currentDate.isSameOrAfter(selectedDate, 'day') && currentDate.isSameOrBefore(selectedDate.add(self.app.tourDays - 1, 'day'), 'day')) {
            cssClass += 'ui-state-highlight ';
          }
        }
        // last day highlight
        if (selectedDate) {
          if (currentDate.isSame(selectedDate.add(self.app.tourDays - 1, 'day'), 'day')) {
            cssClass += ' last-day';
          }
        }
        return [enabled, cssClass, price];
      },
      onUpdateDatepicker: function(cal){

        self.app.hasPrePostNightPrices = $(cal.dpDiv).find('.ui-datepicker-current-day').hasClass('hasPrePostNight')
        $('.step-date').find('input[name="hasPrePostNights"]').val(self.app.hasPrePostNightPrices ? "1" : "0")

      },
      onSelect: function (date, cal, cd) {

       
        selectedDate = dayjs(date, 'DD/MM/YYYY');

        //toggle select
        if (previousDate && selectedDate.isSame(previousDate, 'day')) {
          selectedDate = null;
          previousDate = null;
          self.el.find('.form-steps .step[data-step="date"] .calendar-date').val('');
        } else {
          previousDate = dayjs(date, 'DD/MM/YYYY');
          self.el.find('.form-steps .step[data-step="date"] .calendar-date').val($(this).val());
        }

        const currentDate = dayjs(cd);
        self.app.entireFlexIsOffered = false;
        if (!selectedDate) {
          self.el.find('.full-payment-message').hide();
        } else {
          const daysFromSelectedDay = selectedDate.diff(currentDate, 'day');

          if (self.app.changeOfMindThresholdDays > 0) {
            const fullPaymentDays = self.app.changeOfMindThresholdDays + 1;

            if (selectedDate.isBefore(currentDate.add(fullPaymentDays, 'days'), 'day')) {
              self.el.find('.full-payment-message').show();
            } else {
              self.el.find('.full-payment-message').hide();
            }
          } else {
            self.el.find('.full-payment-message').show();
          }
        }
      },
    });

    if (!self.app.cartCompleted || !self.app.selectedDepartureDate) {
      calendarDatePicker.datepicker('setDate', earliestDate.toDate());
    } else {
      selectedDate = dayjs(self.app.selectedDepartureDate, 'DD/MM/YYYY');
      calendarDatePicker.datepicker('setDate', selectedDate.toDate());
    }
  };

  self.generateDatesAndPricesList = function () {
    // Loop over 'dates and prices' data and group data by common headings
    let activeIndex = 0;
    let activeGroupHeading = '';
    const groupedData = self.app.steps.date.datesAndPrices.reduce(function (acc, currentValue) {
      // If a new group
      if (activeGroupHeading !== currentValue.groupHeading) {
        acc.push([]);
        if (acc.length > 1) activeIndex++;
        activeGroupHeading = currentValue.groupHeading;

        // Add group heading as first item in array
        acc[activeIndex].push(activeGroupHeading);
      }

      // Add date and price
      acc[activeIndex].push(currentValue);
      return acc;
    }, []);

    // Create accordion from grouped 'dates and prices' data
    let accordionElement = '<div class="ui styled fluid accordion">'; // Open accordion div
    $.each(groupedData, function (index, value) {
      // Build accordion heading and content
      let title = '';
      let content = '';
      console.log('value', value)
      $.each(value, function (index, value) {
        if (typeof value === 'string') {
          title += value;
        } else {
          content += '<div class="field">';
          content += '<div class="ui radio checkbox">';
          if (self.app.cartCompleted || self.app.selectedDepartureDate === value.departureFormattedDate) {
            content += `<input type="radio" name="departureDate" data-hasPrePost="${value.hasPrePostNightPrice}" value="${value.departureFormattedDate}" checked/>`;
          } else {
            content += `<input type="radio" name="departureDate" data-hasPrePost="${value.hasPrePostNightPrice}" value="${value.departureFormattedDate}"/>`;
          }
          content += `<label>${value.departureDateAndPriceDisplay}</label>`;
          content += '</div>';
          content += '</div>';
        }
      });

      // Add accordion panels
      accordionElement += `<div class="title">
        <h5 class="heading">${title}</h5> <i class="dropdown icon"></i>
      </div>
      <div class="content accordion-content">
        <div class="inner-content">
          ${content}
        </div>
      </div>`;
    });
    accordionElement += '</div>'; // Close accordion div

    // Add accordion content to page
    const datesElement = $('.form-steps .step[data-step="date"] .radio-buttons .dates');
    datesElement.html(accordionElement);

    // Init accordion
    datesElement.find('.ui.accordion').accordion({
      exclusive: false,
    });

    $('input[name="departureDate"]').on('change', function(e){     
      self.app.hasPrePostNightPrices = $(this).attr('data-hasPrePost') == 'true'
      $('.step-date').find('input[name="hasPrePostNights"]').val(self.app.hasPrePostNightPrices ? "1" : "0")
    })
  };

  self.getPrePostNightsSummaryBreakdownHtml = function(response){

    var nightsBreakdownHtml = ''
    if (response.bookingSummary.preNightsDetails != null || response.bookingSummary.postNightsDetails != null){
      
      if (response.bookingSummary.preNightsDetails != null){
        nightsBreakdownHtml += `${response.bookingSummary.preNightsDetails.numberOfNights} Pre Nights added (${response.bookingSummary.preNightsDetails.dateRangeDisplay})`
      }

      if (response.bookingSummary.postNightsDetails != null){
        if (response.bookingSummary.preNightsDetails != null){
          nightsBreakdownHtml += '<br>'
        }
        nightsBreakdownHtml += `${response.bookingSummary.postNightsDetails.numberOfNights} Post Nights added (${response.bookingSummary.postNightsDetails.dateRangeDisplay})`
      }
      
    }
    return nightsBreakdownHtml;
  };

  self.generateBookingSummary = function () {
    if (!self.app.cartCompleted) {
      return;
    }

    $.ajax({
      url: self.el.find('.endpoints').attr('data-booking-summary'),
      type: self.el.find('.endpoints').attr('data-booking-summary-type'),
      contentType: 'application/json',
    })
      .done(function (response) {
        //future action to parent contain properties in data and loop through to render html
        //set html from summary response

        var prePostNightsHtml = ''

        if (response.bookingSummary.preNightsDetails != null || response.bookingSummary.postNightsDetails != null){
          prePostNightsHtml = '<br>'
          if (response.bookingSummary.preNightsDetails != null){
            prePostNightsHtml += `<br>Pre nights: ${response.bookingSummary.preNightsDetails.dateRangeDisplay}`
          }
  
          if (response.bookingSummary.postNightsDetails != null){
            prePostNightsHtml += `<br>Post nights: ${response.bookingSummary.postNightsDetails.dateRangeDisplay}`
          }
  
        }
        
        self.el
          .find('.booking-summary .booking-summary-breakdown h5')
          .html(response.bookingSummary.tourName + ' (' + response.bookingSummary.tourCode + ')' + '<br>' + response.bookingSummary.departureDateDisplay);

        if (response.bookingSummary.packages.count > 0) {
          self.el.find('.booking-summary .package .label').html(response.bookingSummary.packages.itemLabel);
          self.el
            .find('.booking-summary .package .qty')
            .html(
              response.bookingSummary.packages.count + ' @ ' + self.currencySymbol + self.commaSeparateNumber(response.bookingSummary.packageUnitPrice) + 'pp'
            );
          self.el.find('.booking-summary .package .price').html(self.currencySymbol + self.commaSeparateNumber(response.bookingSummary.packagesBreakdownTotalPrice));

          self.el.find('.booking-summary .package .prepost-nights-breakdown').html(self.getPrePostNightsSummaryBreakdownHtml(response));
          
          if (response.bookingSummary.selectedTwinShareRoomsType != null && response.bookingSummary.selectedTwinShareRoomsType.length > 0) {
            let roomTypesHtml = '';

            for (let i = 1; i <= response.bookingSummary.selectedTwinShareRoomsType.length; i++) {
              roomTypesHtml += '<div>Room ' + i.toString() + ': ' + response.bookingSummary.selectedTwinShareRoomsType[i - 1] + '</div>';
            }

            self.el.find('.booking-summary .package .roomtypes-breakdown').html(roomTypesHtml);
          }

          self.el.find('.booking-summary .package').show();
        } else {
          self.el.find('.booking-summary .package').hide();
        }

        if (response.bookingSummary.singlePackages.count > 0) {
          self.el.find('.booking-summary .single-package .label').html(response.bookingSummary.singlePackages.itemLabel);
          self.el
            .find('.booking-summary .single-package .qty')
            .html(
              response.bookingSummary.singlePackages.count +
                ' @ ' +
                self.currencySymbol +
                self.commaSeparateNumber(response.bookingSummary.singlePackageUnitPrice) +
                'pp'
            );

          self.el.find('.booking-summary .single-package .prepost-nights-breakdown').html(self.getPrePostNightsSummaryBreakdownHtml(response));
          
          self.el
            .find('.booking-summary .single-package .price')
            .html(self.currencySymbol + self.commaSeparateNumber(response.bookingSummary.singlePackagesBreakdownTotalPrice));
          self.el.find('.booking-summary .single-package').show();
        } else {
          self.el.find('.booking-summary .single-package').hide();
        }

        // room option summary
        if (response.bookingSummary.roomOptions?.length === 0 || response.bookingSummary.roomOptionsTotalPrice === 0) {
          self.el.find('.booking-summary .room-option').hide();
        } else {
          let roomOptionSummary = '';
          $.each(response.bookingSummary.roomOptions, function (index, value) {
            roomOptionSummary +=
              '<div class="qty">' + value.count + ' ' + value.itemLabel + ' @ ' + self.currencySymbol + self.commaSeparateNumber(value.unitPriceWithPrePostNights) + 'pp</div>';
          });
          self.el.find('.booking-summary .room-option .item-name .label').html('Room Upgrades');
          self.el.find('.booking-summary .room-option .prepost-nights-breakdown').html(self.getPrePostNightsSummaryBreakdownHtml(response));
          self.el.find('.booking-summary .room-option .room-list').html(roomOptionSummary);
          self.el
            .find('.booking-summary .room-option .price')
            .html(self.currencySymbol + self.commaSeparateNumber(response.bookingSummary.roomOptionsTotalPrice) + '');
          self.el.find('.booking-summary .room-option').show();
        }

        // extras summary
        if (response.bookingSummary.extraOptionsTotalPrice !== 0) {
          let extrasOptionSummary = '';
          $.each(response.bookingSummary.extraOptions, function (index, value) {
            extrasOptionSummary +=
              '<div class="qty">' + value.count + ' ' + value.itemLabel + ' @ ' + self.currencySymbol + self.commaSeparateNumber(value.unitPrice) + 'pp</div>';
          });

          self.el.find('.booking-summary .extras-option .label').html('Extras');
          self.el.find('.booking-summary .extras-option .extras-list').html(extrasOptionSummary);
          self.el
            .find('.booking-summary .extras-option .price')
            .html(self.currencySymbol + self.commaSeparateNumber(response.bookingSummary.extraOptionsTotalPrice) + '');
          self.el.find('.booking-summary .extras-option').show();
        } else {
          self.el.find('.booking-summary .extras-option').hide();
        }

        // entire flex summary
        if (response.bookingSummary.entireFlexPricing != null && response.bookingSummary.entireFlexPricing.count > 0) {
          self.el.find('.booking-summary .entire-flex .label').html(response.bookingSummary.entireFlexPricing.itemLabel);
          self.el
            .find('.booking-summary .entire-flex .qty')
            .html(
              response.bookingSummary.entireFlexPricing.count +
                ' @ ' +
                self.currencySymbol +
                self.commaSeparateNumber(response.bookingSummary.entireFlexPricing.unitPrice) +
                'pp'
            );
          self.el
            .find('.booking-summary .entire-flex .price')
            .html(self.currencySymbol + ' ' + self.commaSeparateNumber(response.bookingSummary.entireFlexPricing.totalPrice));
          self.el.find('.booking-summary .entire-flex').show();
        } else {
          self.el.find('.booking-summary .entire-flex').hide();
        }

        self.el.find('.full-payment-additional-message').html('');

        //deposit
        if (
          response.bookingSummary.due != null &&
          response.bookingSummary.due.depositPrice != null &&
          response.bookingSummary.due.depositPrice.totalPrice !== 0
        ) {
          self.el
            .find('.deposit-due .deposit .qty')
            .html(
              response.bookingSummary.due.depositPrice.count +
                ' @ ' +
                self.currencySymbol +
                self.commaSeparateNumber(response.bookingSummary.due.depositPrice.unitPrice) +
                'pp'
            );
          self.el
            .find('.deposit-due .deposit .price')
            .html(self.currencySymbol + ' ' + self.commaSeparateNumber(response.bookingSummary.due.depositPrice.totalPrice));
          self.el
            .find('.deposit-due .item-deposit .total-deposit-price span')
            .first()
            .html(self.currency + ' ' + self.commaSeparateNumber(response.bookingSummary.due.totalDeposit));

          if (self.currency != 'AUD' && self.el.find('.deposit-due .item-deposit .total-deposit-price .price-aud span').length) {
            self.el
              .find('.deposit-due .item-deposit .total-deposit-price .price-aud span')
              .html(self.commaSeparateNumber(response.bookingSummary.due.totalDepositeInAUD));
          }

          self.el.find('.deposit-due .item-deposit .terms').html(response.bookingSummary.due.dueCopy);

          if (response.bookingSummary.entireFlexPricing != null && response.bookingSummary.due.entireFlexPricing.count > 0) {
            self.el.find('.deposit-due .deposit-entire-flex .label').html(response.bookingSummary.due.entireFlexPricing.itemLabel);
            self.el
              .find('.deposit-due .deposit-entire-flex .qty')
              .html(
                response.bookingSummary.due.entireFlexPricing.count +
                  ' @ ' +
                  self.currencySymbol +
                  self.commaSeparateNumber(response.bookingSummary.due.entireFlexPricing.unitPrice) +
                  'pp'
              );
            self.el
              .find('.deposit-due .deposit-entire-flex .price')
              .html(self.currencySymbol + self.commaSeparateNumber(response.bookingSummary.due.entireFlexPricing.totalPrice));
            self.el.find('.deposit-due .deposit-entire-flex').show();
          } else {
            self.el.find('.deposit-due .deposit-entire-flex').hide();
          }

          self.el.find('.deposit-due').show();
        } else {
          self.el.find('.full-payment-additional-message').html(response.message);
          self.el.find('.deposit-due').hide();
        }

        //discount
        if (response.bookingSummary.promotion != null) {
          self.el.find('.booking-summary .discount .label').html(response.bookingSummary.promotion.promotionName);
          self.el.find('.booking-summary .discount .qty').html(response.bookingSummary.promotion.bookByText);
          self.el
            .find('.booking-summary .discount .price')
            .html('-' + self.currencySymbol + self.commaSeparateNumber(response.bookingSummary.discount.toString()));
        }

        //total price
        self.el
          .find('.booking-summary .total .total-price span')
          .first()
          .html(self.currency + ' ' + self.commaSeparateNumber(response.bookingSummary.totalPrice));

        if (self.currency != 'AUD' && self.el.find('.booking-summary .total .total-price .price-aud span').length) {
          self.el.find('.booking-summary .total .total-price .price-aud span').html(self.commaSeparateNumber(response.bookingSummary.totalPriceInAUD));
        }
        //$('.book-now-disclaimer').html(response.bookingSummary.bookNowDisclaimer);

        // Highlight page content that relates to current booking step.
        /* NOT IN USE
        setTimeout(function () {
          $('.step-content').each(function (i, el) {
            const stepPanel = $(el).parents('.step').attr('data-step')

            if ($(el).is(':visible')) {
              $('.package-accordion .title[data-step="' + stepPanel + '"]').addClass('highlight');
            } else {
              $('.package-accordion .title[data-step="' + stepPanel + '"]').removeClass('highlight');
            }
          });
        }, 500);*/
        // END Highlight page content that relates to current booking step.
      })
      .fail(function () {
        console.log('fail');
      })
      .always(function () {
        self.enableUI();
      });
  };

  self.disableUI = function () {
    // maybe have a blocker on top of everything with a spinner
    self.el.find('.loader').show();
    self.uiEnabled = false;
  };

  self.enableUI = function () {
    // hide blocker
    self.el.find('.loader').hide();
    self.uiEnabled = true;
  };

  self.initForms = function () {
    self.app.steps.date.form = new dbs.form.genericForm();
    self.app.steps.date.form.init(self.el.find('.step[data-step="date"] .ui.form'));

    self.app.steps.prePost.form = new dbs.form.genericForm();
    self.app.steps.prePost.form.init(self.el.find('.step[data-step="prePost"] .ui.form'));

    self.app.steps.travellers.form = new dbs.form.genericForm();
    self.app.steps.travellers.form.init(self.el.find('.step[data-step="travellers"] .ui.form'));

    self.app.steps.roomOptions.form = new dbs.form.genericForm();
    self.app.steps.roomOptions.form.init(self.el.find('.step[data-step="roomOptions"] .ui.form'));

    self.app.steps.freedomOfChoice.form = new dbs.form.genericForm();
    self.app.steps.freedomOfChoice.form.init(self.el.find('.step[data-step="freedomOfChoice"] .ui.form'));

    self.app.steps.optionalExtras.form = new dbs.form.genericForm();
    self.app.steps.optionalExtras.form.init(self.el.find('.step[data-step="optionalExtras"] .ui.form'));

    //self.app.steps.entireFlexOption.form = new dbs.form.genericForm();
    //self.app.steps.entireFlexOption.form.init(self.el.find('.step[data-step="entireFlexOption"] .ui.form'));

    self.app.steps.otherOptions.form = new dbs.form.genericForm();
    self.app.steps.otherOptions.form.init(self.el.find('.step[data-step="otherOptions"] .ui.form'));

    // NEW bind new agent form
    self.agentForm = new dbs.form.genericForm();
    self.agentForm.init(self.el.find('.js-agent-form .ui.form'));

    // NEW agent form events
    self.agentForm.events.subscribe('Form:send', function () {
      console.log('agent form send');
      self.disableUI();
    });

    self.agentForm.events.subscribe('Form:send_success', function () {
      console.log('agent form response');

      self.el.find('.js-start-buttons').slideUp();
      self.el.find('.js-agent-form').slideUp();
      self.el.find('.js-form-steps').slideDown();

      self.enableUI();
    });
  };

  self.scrollToBookingForm = function () {
    // 'scrollToElement' should include booking form and heading so when you scroll to the content, all the
    // relevant content is displayed on the screen, and the form heading is not cut off or hidden from view.
    scrollToElement($('.book-now-package-booking'));
  };

  window.bookNowClient = function () {
    self.el.find('.js-start-buttons').slideUp();
    self.el.find('.js-agent-form').slideUp();
    self.el.find('.js-form-steps').slideDown();
    self.el.find('.js-booking-summary').slideUp();

    self.el.find('.proceed-button').attr('href', '/booking/checkout');

    let iconLockOpen =
      '<svg xmlns="http://www.w3.org/2000/svg" width="23" height="18" fill="none"><path fill="#CA568E" d="M18.809.264a.77.77 0 0 1 1.16 0l2.25 2.25c.175.14.281.351.281.562a.751.751 0 0 1-.281.598l-2.25 2.25a.77.77 0 0 1-1.16 0c-.352-.317-.352-.844 0-1.195l.808-.81H13.5c-.492 0-.844-.35-.844-.843 0-.457.352-.844.844-.844h6.117l-.808-.808a.77.77 0 0 1 0-1.16ZM3.656 13.236l-.808.809h6.117c.492 0 .844.387.844.844 0 .492-.352.843-.844.843H2.848l.808.809c.352.352.352.879 0 1.195a.77.77 0 0 1-1.16 0l-2.25-2.25C.07 15.346 0 15.135 0 14.89c0-.211.07-.422.246-.598l2.25-2.25a.828.828 0 0 1 1.16 0c.352.352.352.879 0 1.195ZM3.34 2.232h8.508a2.416 2.416 0 0 0-.176.844c0 1.02.808 1.828 1.828 1.828h3.41c.106.633.457 1.16.985 1.477.07.105.14.176.21.246a1.764 1.764 0 0 0 2.567 0l.703-.668v7.523c0 1.266-1.02 2.25-2.25 2.25h-8.508c.106-.246.176-.527.176-.843 0-.985-.809-1.829-1.828-1.829h-3.41a2.098 2.098 0 0 0-.985-1.44c-.07-.106-.14-.177-.21-.247a1.764 1.764 0 0 0-2.567 0l-.703.668V4.482c0-1.23 1.02-2.25 2.25-2.25Zm0 4.5c1.265 0 2.25-.984 2.25-2.25H3.34v2.25Zm15.785 6.75v-2.25a2.243 2.243 0 0 0-2.25 2.25h2.25Zm-7.91-1.125c1.898 0 3.41-1.511 3.41-3.375 0-1.863-1.512-3.375-3.41-3.375a3.376 3.376 0 0 0 0 6.75Z"/></svg>';
    self.el.find('.book-now-heading').text('Book Now');
    self.el.find('.book-now-note span').text('Only $100pp deposit to Book Now');
    self.el.find('.book-now-note .icon').html(iconLockOpen);

    self.scrollToBookingForm();
  };

  window.bookNowTravelAgent = function () {
    self.el.find('.js-start-buttons').slideUp();
    self.el.find('.js-form-steps').slideUp();
    self.el.find('.js-agent-form').slideDown();
    self.el.find('.js-booking-summary').slideUp();

    let iconMoneyTransfer =
      '<svg xmlns="http://www.w3.org/2000/svg" width="21" height="18" fill="none"><path fill="#CA568E" d="M12.375 6.75H13.5c1.23 0 2.25 1.02 2.25 2.25v6.75c0 1.266-1.02 2.25-2.25 2.25H2.25A2.221 2.221 0 0 1 0 15.75V9c0-1.23.984-2.25 2.25-2.25h7.875V5.062A5.064 5.064 0 0 1 15.188 0c2.777 0 5.062 2.285 5.062 5.063V6.75c0 .633-.527 1.125-1.125 1.125A1.11 1.11 0 0 1 18 6.75V5.062a2.82 2.82 0 0 0-2.813-2.812 2.798 2.798 0 0 0-2.812 2.813V6.75Z"/></svg>';
    self.el.find('.book-now-heading').text('Travel agents');
    self.el.find('.book-now-note span').html('No password required to <br>Quote or Book Now');
    self.el.find('.book-now-note .icon').html(iconMoneyTransfer);

    self.scrollToBookingForm();
  };

  self.hasEntireFlexStep = function () {
    return false; //self.app.hasPeaceOfMind && self.app.hasEntireFlex && self.app.entireFlexIsOffered;
  };

  self.bindTravellerStepsDropdown = function () {
    $('.form-steps .step[data-step="travellers"] .double-room').dropdown({
      onChange: function () {
        let roomTypesHtml = '';
        const noOfRooms = Number($('.form-steps .step[data-step="travellers"] .double-room').dropdown('get value'));

        console.log('onchange', $('.form-steps .step[data-step="travellers"] .double-room'));
        for (let i = 1; i <= noOfRooms; i++) {
          roomTypesHtml += '<label>Room ' + i.toString() + '</label>';
          roomTypesHtml += '<select class="room-type" name="twinRoomType">';
          roomTypesHtml += '<option value=" ">None</option><option value="Twin">Twin</option><option value="Double">Double</option></select>';
        }

        self.el.find('.double-room-suboptions').html(roomTypesHtml);

        $('.form-steps .step[data-step="travellers"] select.room-type').dropdown({
          onChange: function () {
            let selected = '';

            $('.form-steps .step[data-step="travellers"] .room-type').each(function () {
              if (selected !== '') {
                selected += ',';
              }
              selected += $(this).dropdown('get value');
            });

            $('input[name="selectedRoomTypes"]').val(selected);
          },
        });
      },
    });
  };

  self.bindDateForm = function () {
    // calendar date events
    self.app.steps.date.form.events.subscribe('Form:send', function (data) {
      console.log('date form send', data);
      self.resetSucceedingSteps('date');
      self.disableUI();
    });

    self.app.steps.date.form.events.subscribe('Form:send_success', function (data) {
      console.log('date form response', data);

      self.generateBookingSummary();

      // Update step labels and data
      self.app.steps.date.subLabel = data.response.selectedDepartureDisplayDate;

      // Mark step as complete
      self.app.steps.date.complete = true;
      self.app.steps.date.active = false;

      // Activate next step
      const activateNextStep = function () {
          self.app.steps.travellers.active = true;
      };

      activateNextStep();

      // Consume the response data to prepare step content
      const prepareStepContent = function () {

        let optionsHtml = '<option value=" ">None</option>';
        $.each(data.response.twinShareOptions, function (index, value) {
          optionsHtml += '<option value="' + value.numberOfRooms + '">' + value.label + '</option>';
        });

        $('.form-steps .step[data-step="travellers"] select').html(optionsHtml);
        let twinShareOptions = [];
        twinShareOptions.push({
          value: ' ',
          text: 'None',
          name: 'None',
        });
        $.each(data.response.twinShareOptions, function (index, value) {
          twinShareOptions.push({
            value: value.numberOfRooms,
            text: value.label,
            name: value.label,
          });
        });
        $('.form-steps .step[data-step="travellers"] .double-room').dropdown('setup menu', {
          values: twinShareOptions,
        });
        self.bindTravellerStepsDropdown();

        $('.form-steps .step[data-step="travellers"] .field-double-room').show();

        if (data.response.hasSingleSupplementOption) {
          let singleRoomOptions = '<option value=" ">None</option>';
          $.each(data.response.singleRoomOptions, function (index, value) {
            singleRoomOptions += '<option value="' + value.numberOfRooms + '">' + value.label + '</option>';
          });
          $('.form-steps .step[data-step="travellers"] select.single-room').html(singleRoomOptions).dropdown();
          $('.form-steps .step[data-step="travellers"] .field-single-room').show();
        } else {
          $('.form-steps .step[data-step="travellers"] .field-single-room').hide();
        }

        //if room options false hide room options step
        if (!data.response.hasTwinShareOptions && !data.response.hasSingleRoomOptions) {
          self.app.steps.roomOptions.hasRoomOptions = false;
          self.app.steps.roomOptions.visible = false;
        } else {
          self.app.steps.roomOptions.visible = true;
          self.app.steps.roomOptions.hasRoomOptions = true;
        }

        self.preparePrePostStepContent(data)
      };
      prepareStepContent();

      self.renderUI();
    });
  };

  self.bindPrePostForm = function () {
    self.app.steps.prePost.form.events.subscribe('Form:send', function (data) {
      console.log('prePost form send', data);
      //self.resetSucceedingSteps('prePost');
      self.disableUI();
    });

    self.app.steps.prePost.form.events.subscribe('Form:send_success', function (data) {
      console.log('prePost form response', data);

      // do I need to do this here?
      self.generateBookingSummary();

      // Update step labels and data
      self.app.steps.prePost.subLabel = data.response.selectedPrePost;

      // Mark step as complete
      self.app.steps.prePost.complete = true;
      self.app.steps.prePost.active = false;

      // Activate next step
      const activateNextStep = function () {
        
        if (self.app.hasFreedomOfChoice && !self.app.steps.freedomOfChoice?.complete) {
          self.app.steps.freedomOfChoice.active = true;
          return;
        }

        if (self.app.steps.optionalExtras.hasExtras && !self.app.steps.optionalExtras?.complete) {
          self.app.steps.optionalExtras.active = true;
          return;
        }

        if (!self.app.steps.otherOptions?.complete) {
          self.app.steps.otherOptions.active = true;
        }
      };
      activateNextStep();

      // Consume the response data to prepare step content
      const prepareStepContent = function () {
        //self.prepareTravellerStepContent(data)
        
      };
      prepareStepContent();

      self.renderUI();
    });
  };

  self.bindEvents = function () {
    $(document)
      .find('.js-scrollto-book-now')
      .click(function (e) {
        e.preventDefault();
        const SITE_HEADER_HEIGHT = 94;
        setTimeout(() => {
          $('html, body')
            .stop()
            .animate(
              {
                scrollTop: $('.book-now-heading').position().top - SITE_HEADER_HEIGHT,
              },
              700
            );
        }, 500);
      });

    $(document)
      .find('.book-now-trigger')
      .click(function (e) {
        e.preventDefault();
        window.bookNowClient();
      });

    self.el.find('.ta-client-quote-button').on('click', function (e) {
      e.preventDefault();
      self.agentForm.form.form('validate form');
      if (self.agentForm.form.form('is valid')) {
        self.el.find('.proceed-button').attr('href', $(this).attr('data-url'));
        self.el.find('.step-date').attr('data-endpoint', self.el.find('.step-date').attr('data-endpoint-agent'));
        self.app.steps.date.form.unbindSubmit();
        self.app.steps.date.form.init(self.el.find('.step-date'));
        self.bindDateForm();
        self.bindPrePostForm();
        self.agentForm.send();
      }
    });

    self.el.find('.ta-book-now-button').on('click', function (e) {
      e.preventDefault();
      self.agentForm.form.form('validate form');
      if (self.agentForm.form.form('is valid')) {
        self.el.find('.proceed-button').attr('href', $(this).attr('data-url'));
        self.el.find('.step-date').attr('data-endpoint', self.el.find('.step-date').attr('data-endpoint-agent'));
        self.app.steps.date.form.unbindSubmit();
        self.app.steps.date.form.init(self.el.find('.step-date'));
        self.bindDateForm();
        self.bindPrePostForm();
        self.agentForm.send();
      }
    });

    self.bindDateForm();
    self.bindPrePostForm();

    self.bindTravellerStepsDropdown();
    // Travellers Events events
    self.app.steps.travellers.form.events.subscribe('Form:send', function (data) {
      console.log('traveller form send', data);
  
      if (self.app.hasPrePostNightPrices){

        $('.form-steps .step[data-step="prePost"] .select-pre').dropdown('set selected', ' ')
        $('.form-steps .step[data-step="prePost"] .select-post').dropdown('set selected', ' ')
      }
      self.resetSucceedingSteps('travellers');
      self.disableUI();
    });

    self.app.steps.travellers.form.events.subscribe('Form:send_success', function (data) {
      console.log('traveller form response', data);

      self.generateBookingSummary();

      // Update step labels and data
      self.app.steps.travellers.subLabel = `${data.response.numberOfTravellers} Travellers (${data.response.totalDisplayPrice})`;

      if (data.response.roomOptions?.length === 0) {
        self.app.steps.roomOptions.hasRoomOptions = false;
      }

      if (!self.app.steps.roomOptions.hasRoomOptions) {
        self.app.steps.roomOptions.visible = false;
        self.app.hasNoRoomOptions = true;
      }

      // Mark step as complete
      self.app.steps.travellers.complete = true;
      self.app.steps.travellers.active = false;

      // Activate next step
      const activateNextStep = function () {
        
        if (!self.app.hasNoRoomOptions && !self.app.steps.roomOptions?.complete) {
          self.app.steps.roomOptions.active = true;
          return;
        }


        if (self.app.hasPrePostNightPrices && !self.app.steps.prePost?.complete) {
          
          self.app.steps.prePost.active = true;
          return;
        }


        if (self.app.hasFreedomOfChoice && !self.app.steps.freedomOfChoice?.complete) {
          self.app.steps.freedomOfChoice.active = true;
          return;
        }

        if (self.app.steps.optionalExtras.hasExtras && !self.app.steps.optionalExtras?.complete) {
          self.app.steps.optionalExtras.active = true;
          return;
        }

        if (!self.app.steps.otherOptions?.complete) {
          self.app.steps.otherOptions.active = true;
        }
      };

      // Consume the response data to prepare step content
      const prepareStepContent = function () {
        $('.form-steps .step[data-step="optionalExtras"] .option-set-container').hide();
        $('.form-steps .step[data-step="freedomOfChoice"] .option-set-container').hide();

        if (data.response.totalRooms > 0) {
          self.app.steps.roomOptions.roomOptions = data.response;

          // loop through response and generate markup for room options step
          let roomOptions = '';
          $.each(data.response.roomOptions, function (index, value) {
            roomOptions += '<div class="field">';
            roomOptions += '<label>' + value.fieldLabel + '</label>';
            roomOptions += '<select name="' + value.fieldName + '">';
            roomOptions += '<option value=" ">None</option>';
            $.each(value.options, function (item, option) {
              roomOptions +=
                '<option value="' +
                option.optionGuid +
                '">' +
                option.label +
                ' (+' +
                self.currencySymbol +
                self.commaSeparateNumber(option.supplementalCost) +
                'pp)</option>';
            });
            roomOptions += '</select>';
            roomOptions += '</div>';
          });

          $('.form-steps .step[data-step="roomOptions"] .field-container').html(roomOptions);
          $('.form-steps .step[data-step="roomOptions"] .field-container select').dropdown();
        } else {
          console.log('no room options');
        }

        if (data.response.totalRooms > 1) {
          $('.form-steps .step[data-step="optionalExtras"] .selectnow').checkbox('disable');
          $('.form-steps .step[data-step="optionalExtras"] .selectlater').checkbox('check');
          $('.form-steps .step[data-step="optionalExtras"] .multiple-room-copy').show();

          $('.form-steps .step[data-step="freedomOfChoice"] .selectnow').checkbox('disable');
          $('.form-steps .step[data-step="freedomOfChoice"] .selectlater').checkbox('check');
          $('.form-steps .step[data-step="freedomOfChoice"] .multiple-room-copy').show();
          $('.form-steps .step[data-step="freedomOfChoice"] .select-later-copy').hide();
        } else {
          $('.form-steps .step[data-step="optionalExtras"] .selectnow').checkbox('enable');
          $('.form-steps .step[data-step="optionalExtras"] .multiple-room-copy').hide();

          $('.form-steps .step[data-step="freedomOfChoice"] .selectnow').checkbox('enable');
          $('.form-steps .step[data-step="freedomOfChoice"] .multiple-room-copy').hide();
        }

        if (!data.response.hasExtras) {
          self.app.steps.optionalExtras.hasExtras = false;
          self.app.steps.optionalExtras.visible = false;
        } else {
          self.app.steps.optionalExtras.hasExtras = true;
          self.app.steps.optionalExtras.visible = true;

          // loop through response and generate markup for optional extras step
          let extraOptions = '';
          $.each(data.response.extraOptions, function (index, value) {
            extraOptions += '<div class="field">';
            extraOptions += '<div class="ui checkbox">';
            extraOptions += '<input type="checkbox" name="' + value.fieldName + '" value="' + value.optionGuid + '"/>';
            extraOptions += '<label>' + value.label + ' +' + self.currencySymbol + self.commaSeparateNumber(value.supplementalCost) + 'pp</label>';
            extraOptions += '</div>';
            extraOptions += '</div>';
          });
          $('.form-steps .step[data-step="optionalExtras"] .option-set').html(extraOptions);
          $('.form-steps .step[data-step="optionalExtras"] .ui.checkbox').checkbox();
          //$('.form-steps .step[data-step="entireFlexOption"] .ui.checkbox').checkbox();
        }
      };
      prepareStepContent();
      activateNextStep();

      self.renderUI();
    });

    // Room options events
    self.app.steps.roomOptions.form.events.subscribe('Form:send', function (data) {
      console.log('room options form send', data);

      self.app.hasSingleRoomUpgrade = false;
      for (var i=1;i<=9;i++){
        if (!data.data.hasOwnProperty(`singleRoomOption${i}`)){
          break;
        }

        const value = data.data[`singleRoomOption${i}`];
        if (!(value == null || value == ' ' || value == '')){
          self.app.hasSingleRoomUpgrade = true
          break;
        }

      }

      if (self.app.hasSingleRoomUpgrade){

        $('.form-steps .step[data-step="prePost"] .select-pre').dropdown('set selected', ' ')
        $('.form-steps .step[data-step="prePost"] .select-post').dropdown('set selected', ' ')
      }

      self.resetSucceedingSteps('roomOptions');
      
      self.disableUI();
    });

    self.app.steps.roomOptions.form.events.subscribe('Form:send_success', function (data) {
      console.log('room options form response', data);

      self.generateBookingSummary();

      // Update step labels and data
      if (data.response.roomOptionSubTotalDisplayPrice !== '') {
        self.app.steps.roomOptions.subLabel = '+' + data.response.roomOptionSubTotalDisplayPrice;
      }
      self.app.steps.roomOptions.rooms = data.response.rooms;

      // Mark step as complete
      self.app.steps.roomOptions.complete = true;
      self.app.steps.roomOptions.active = false;

      // Activate next step
      const activateNextStep = function () {

        console.log('self.app.hasSingleRoomUpgrade', self.app.hasSingleRoomUpgrade)

        if (!self.app.hasSingleRoomUpgrade && self.app.hasPrePostNightPrices) {
          
          self.app.steps.prePost.active = true;
          return;
        }

        if (self.app.hasFreedomOfChoice && !self.app.steps.freedomOfChoice?.complete) {
          self.app.steps.freedomOfChoice.active = true;
          return;
        }

        if (self.app.steps.optionalExtras.hasExtras && !self.app.steps.optionalExtras?.complete) {
          self.app.steps.optionalExtras.active = true;
          return;
        }

        if (!self.app.steps.otherOptions?.complete) {
          self.app.steps.otherOptions.active = true;
        }
      };
      activateNextStep();

      self.renderUI();
    });

    // Freedom of choices events
    self.app.steps.freedomOfChoice.form.events.subscribe('Form:send', function (data) {
      console.log('freedom of choice form send', data);
      self.disableUI();
    });

    self.app.steps.freedomOfChoice.form.events.subscribe('Form:send_success', function (data) {
      console.log('freedom of choice form response', data);

      self.generateBookingSummary();

      // Mark step as complete
      self.app.steps.freedomOfChoice.complete = true;
      self.app.steps.freedomOfChoice.active = false;

      // Activate next step
      const activateNextStep = function () {
        if (self.app.steps.optionalExtras.hasExtras && !self.app.steps.optionalExtras?.complete) {
          self.app.steps.optionalExtras.active = true;
          return;
        }

        if (!self.app.steps.otherOptions?.complete) {
          self.app.steps.otherOptions.active = true;
        }
      };
      activateNextStep();

      self.renderUI();
    });

    // Optional events
    self.app.steps.optionalExtras.form.events.subscribe('Form:send', function (data) {
      console.log('optional extras send', data);
      self.disableUI();
    });

    self.app.steps.optionalExtras.form.events.subscribe('Form:send_success', function (data) {
      console.log('optional extras form response', data);

      self.generateBookingSummary();

      // Update step labels and data
      if (data.response.extrasSubTotalPrice > 0) {
        self.app.steps.optionalExtras.subLabel = '+' + data.response.extrasSubTotalDisplayPrice;
      }

      // Mark step as complete
      self.app.steps.optionalExtras.complete = true;
      self.app.steps.optionalExtras.active = false;

      // Activate next step
      const activateNextStep = function () {
        if (self.app.steps.optionalExtras.hasExtras && !self.app.steps.optionalExtras?.complete) {
          self.app.steps.optionalExtras.active = true;
          return;
        }

        if (!self.app.steps.otherOptions?.complete) {
          self.app.steps.otherOptions.active = true;
        }
      };
      activateNextStep();

      self.renderUI();
    });

    // Entire flex events
    /*
    self.app.steps.entireFlexOption.form.events.subscribe('Form:send', function(data) {
      console.log('entire flex form send', data);
      self.disableUI();
    })
    self.app.steps.entireFlexOption.form.events.subscribe('Form:send_success', function(data) {
      console.log('entire flex form response', data);
      self.app.steps.entireFlexOption.subLabel = '';
      //generate booking summary
      self.generateBookingSummary();

      //set values on self.app for current step
      if (data.response.entireFlexSubTotalPrice > 0){
        self.app.steps.entireFlexOption.subLabel = '(+' + data.response.entireFlexDisplaySubTotalPrice + ')';
      }
      self.app.steps.entireFlexOption.complete = true;
      self.app.steps.entireFlexOption.active = false;
      //set values on self.app for next step
      if(!self.app.steps.otherOptions.complete) {
        self.app.steps.otherOptions.active = true;
      }

      self.renderUI();
    });
    */

    // Other options events
    self.app.steps.otherOptions.form.events.subscribe('Form:send', function (data) {
      console.log('other options form send', data);
      self.disableUI();
    });

    self.app.steps.otherOptions.form.events.subscribe('Form:send_success', function (data) {
      console.log('other options form response', data);

      // Mark step as complete
      self.app.steps.otherOptions.complete = true;
      self.app.steps.otherOptions.active = false;

      // Mark all steps are complete
      self.app.cartCompleted = true;
      self.generateBookingSummary();

      // Show the booking summary
      self.el.find('.booking-summary').slideDown();

      self.renderUI();
    });

    $('.step-trigger .tool-tip').click(function (event) {
      $('.step-trigger .tool-tip').popup('hide all');
      event.stopPropagation();
    });

    // show book now trigger
    /*$('.book-now-trigger').click(function(event) {
      event.preventDefault();
      window.location = "#booknow"
    });*/

    self.ensureTwoDecimal = function (paramString) {
      if (paramString == null) {
        return '';
      }

      const val = paramString.toString();
      const index = val.indexOf('.');
      if (index > -1) {
        const decimal = val.substring(index + 1, val.length - 1);

        if (decimal.length < 2) {
          return val + '0';
        }
      }

      return val;
    };

    // add commas
    self.commaSeparateNumber = function (val) {
      while (/(\d+)(\d{3})/.test(val.toString())) {
        val = val.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
      }
      return val;
    };

    // Handle 'edit' link on completed steps
    self.el.find('.step-trigger .js-edit').click(function (event) {
      event.preventDefault();
      const stepPanel = $(this).parents('.step');
      // Check if the step has been completed before
      if (stepPanel.hasClass('complete') || stepPanel.hasClass('active')) {
        // Show the step content
        stepPanel.toggleClass('open');
        stepPanel.find('.step-content').slideToggle();
      }
    });

    self.el.find('.show-dynamic-fields').on('change', function () {
      const selectLaterCopy = $(this).parents('.step-content').find('.select-later-copy');
      if ($(this).val() === 'selectnow') {
        if ($(this).is(':checked')) {
          $(this).parents('.step').find('.option-set-container').slideDown();
        }

        if ($(selectLaterCopy)) {
          $(selectLaterCopy).hide();
        }
      } else {
        $(this).parents('.step').find('.option-set-container').slideUp();
        if ($(selectLaterCopy)) {
          $(selectLaterCopy).show();
        }
      }
    });

    self.el.find('.step[data-step="otherOptions"] input[type=radio]').on('change', function (event) {
      const commentBox = $(this).parents('.field').find('.comment-yes');
      if ($(commentBox).length === 1) {
        if ($(this).val() === 'true') {
          $(commentBox).show();
        } else {
          $(commentBox).hide();
        }
      }
    });

    self.bindOtherEvents();
  };

  self.cancelModal = self.el.find('.cancel-booking-modal').modal();

  self.bindOtherEvents = function () {
    // cancel action
    self.el.find('.cancel').click(function (event) {
      event.preventDefault();
      self.cancelModal.modal('setting', 'closable', false).modal('show');
    });

    // cancel modal 'no' button click
    $('body').on('click', '.cancel-booking-modal .no', function () {
      self.cancelModal.modal('hide');
    });

    // cancel modal 'yes' button click
    $('body').on('click', '.cancel-booking-modal .yes', function () {
      $.ajax({
        url: self.el.find('.endpoints').attr('data-cancel'),
        type: 'GET',
        contentType: 'application/json',
      }).done(function (response) {
        self.el.find('.js-agent-form').slideUp();
        self.el.find('.js-form-steps').slideUp();
        self.el.find('.js-booking-summary').slideUp();
        self.resetApp();
        self.init();
        location = '#booknow';

        let iconLockOpen =
          '<svg xmlns="http://www.w3.org/2000/svg" width="23" height="18" fill="none"><path fill="#CA568E" d="M18.809.264a.77.77 0 0 1 1.16 0l2.25 2.25c.175.14.281.351.281.562a.751.751 0 0 1-.281.598l-2.25 2.25a.77.77 0 0 1-1.16 0c-.352-.317-.352-.844 0-1.195l.808-.81H13.5c-.492 0-.844-.35-.844-.843 0-.457.352-.844.844-.844h6.117l-.808-.808a.77.77 0 0 1 0-1.16ZM3.656 13.236l-.808.809h6.117c.492 0 .844.387.844.844 0 .492-.352.843-.844.843H2.848l.808.809c.352.352.352.879 0 1.195a.77.77 0 0 1-1.16 0l-2.25-2.25C.07 15.346 0 15.135 0 14.89c0-.211.07-.422.246-.598l2.25-2.25a.828.828 0 0 1 1.16 0c.352.352.352.879 0 1.195ZM3.34 2.232h8.508a2.416 2.416 0 0 0-.176.844c0 1.02.808 1.828 1.828 1.828h3.41c.106.633.457 1.16.985 1.477.07.105.14.176.21.246a1.764 1.764 0 0 0 2.567 0l.703-.668v7.523c0 1.266-1.02 2.25-2.25 2.25h-8.508c.106-.246.176-.527.176-.843 0-.985-.809-1.829-1.828-1.829h-3.41a2.098 2.098 0 0 0-.985-1.44c-.07-.106-.14-.177-.21-.247a1.764 1.764 0 0 0-2.567 0l-.703.668V4.482c0-1.23 1.02-2.25 2.25-2.25Zm0 4.5c1.265 0 2.25-.984 2.25-2.25H3.34v2.25Zm15.785 6.75v-2.25a2.243 2.243 0 0 0-2.25 2.25h2.25Zm-7.91-1.125c1.898 0 3.41-1.511 3.41-3.375 0-1.863-1.512-3.375-3.41-3.375a3.376 3.376 0 0 0 0 6.75Z"/></svg>';
        self.el.find('.book-now-heading').text('Book Now');
        self.el.find('.book-now-note span').text('Only $100pp deposit to Book Now');
        self.el.find('.book-now-note .icon').html(iconLockOpen);

        self.el.find('.js-start-buttons').slideDown();
        $('.package-sticky-footer').show();
      });

      self.cancelModal.modal('hide');
    });

    self.el.find('.js-start-steps').click(function (e) {
      // self.el.find('.js-start-buttons').slideUp()
      // self.el.find('.js-agent-form').slideUp()
      // self.el.find('.js-form-steps').slideDown()

      e.preventDefault();
      window.bookNowClient();
    });

    self.el.find('.js-start-agent').click(function (e) {
      e.preventDefault();
      // self.el.find('.js-start-buttons').slideUp()
      // self.el.find('.js-agent-form').slideDown()
      window.bookNowTravelAgent();
    });
  };

  self.renderUI = function () {
    $('.ui.tool-tip').each(function (i, el) {
      $(el).popup({
        on: 'click',
        position: 'top right',
      });
    });

    self.initForms();
    self.el.find('.ui.checkbox').checkbox();

    self.el.find('.make-dropdown').dropdown({
      placeholder: false,
    });

    for (const step in self.app.steps) {
      if (self.app.steps.hasOwnProperty(step)) {
        const stepPanel = $('.form-steps .step[data-step="' + step + '"]');
        console.log('stepPanel', stepPanel);
        console.log('step', step, self.app.steps[step]);

        // set active / inactive
        if (self.app.steps[step].active) {
          stepPanel.addClass('active');
          stepPanel.find('.step-content').slideDown();

          // TODO if step-content not in view, then scroll content into view
          let isInView = isInViewport(stepPanel[0]);
          if (!isInView) {
            scrollToElement(stepPanel[0]);
          }
        } else {
          stepPanel.removeClass('active');
          stepPanel.find('.step-content'); //.slideUp();
        }

        // set complete / incomplete
        if (self.app.steps[step].complete) {
          stepPanel.addClass('complete').removeClass('open');
          stepPanel.find('.step-content').slideUp();
        } else {
          stepPanel.removeClass('complete');
        }

        // set visible / hidden
        if (self.app.steps[step].visible) {
          stepPanel.removeClass('hidden');
        } else {
          stepPanel.addClass('hidden');
        }

        // update labels
        stepPanel.find('.label').html(self.app.steps[step].label);
        if (self.app.steps[step].subLabel) {
          stepPanel.find('.sub-label').html(self.app.steps[step].subLabel).show();
        } else {
          stepPanel.find('.sub-label').html('').hide();
        }
      }
    }

    //set step numbers
    self.el
      .find('.form-steps .step')
      .not('.hidden')
      .each(function (i, el) {
        $(el)
          .find('.step-number .number')
          .html(i + 1);
      });

    // Highlight page content that relates to current booking step.
    /* NOT IN USE
    setTimeout(function () {
      $('.step-content').each(function (i, el) {
        const stepPanel = $(el).parents('.step').attr('data-step')

        if ($(el).is(':visible')) {
          $('.package-accordion .title[data-step="' + stepPanel + '"]').addClass('highlight');
        } else {
          $('.package-accordion .title[data-step="' + stepPanel + '"]').removeClass('highlight');
          $('.package-accordion .title[data-step="' + stepPanel + '"]').accordion('close');
        }
      });
    }, 500);*/
    // END Highlight page content that relates to current booking step.
  };
}
