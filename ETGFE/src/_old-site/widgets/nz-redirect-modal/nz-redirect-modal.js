import('../../plugins/jscookie/js.cookie.js').then(({ default: Cookies }) => {
  window.Cookies = Cookies;
  Promise.all([
    import(/* webpackMode: "eager" */ './nz-redirect-modal.scss'),
    import('../../plugins/semantic/modal.css'),
    import('../../plugins/semantic/modal.js'),
    import('../../plugins/semantic/dimmer.css'),
    import('../../plugins/semantic/dimmer.js'),
  ]).then(() => {
    $('.widget.nz-redirect-modal').each(function (i, el) {
      $(el).data('widget', new NzRedirectModal(el));
      $(el).data('widget').init();
    });
  });
});

function NzRedirectModal(el) {
  const self = this;
  self.el = $(el);
  self.loaded = false;
  self.events = new dbs.events();
  self.cookieName = self.el.attr('data-cookie');
  self.checkApiUrl = self.el.attr('data-check-api-url');
  self.checkApiMethod = self.el.attr('data-check-api-method') || 'GET';

  self.init = function () {
    if (process.env.NODE_ENV === 'development') {
      console.log('NzRedirectModal init', self);
    }
    self.el.css('opacity', 1);

    window.isNzRedirectModalOpen = false;

    self.nzRedirectModal = self.el.modal();
    self.nzRedirectModal.modal({
      closable: false, // Prevent closing by clicking modal overlay
      onHide: function () {
        window.isNzRedirectModalOpen = false;
        self.events.emit('NzRedirectModal:hide');
      },
    });

    // const shouldShowModal = document.body.classList.contains('show-nz-redirect-modal');
    // if (shouldShowModal) self.showModal();


    const confirmShowModal = !Cookies.get(self.cookieName) && !self.loaded;
    if (confirmShowModal) {
      self.checkUserLocation();
    } else {
      console.log('Cookie found or modal already loaded, not showing modal.\n', self.cookieName + ':', Cookies.get(self.cookieName));
    }

    // self.checkUserLocation();
  };

  self.checkUserLocation = function () {
    if (!self.checkApiUrl || !self.checkApiMethod) {
      console.error('API URL or Method not provided');
      return;
    }

    const url = `${self.checkApiUrl}`;
    // console.log('NzRedirectModal', 'Checking user location:', url);
    fetch(url)
      .then((response) => response.json())
      .then((result) => {
        // console.log('NzRedirectModal', 'User location:', result);
        if (result.Code === 'NZ') {
          self.showModal();
        }
      })
      .catch((error) => console.error(error));
  };

  self.showModal = function () {
    window.isNzRedirectModalOpen = true;
    self.nzRedirectModal.modal('show');
    self.loaded = true;
    self.events.emit('NzRedirectModal:show');

    self.bindEvents();
  };

  self.bindEvents = function () {
    if (self.el.find('.js-deny-redirect').length) {
      self.el.find('.js-deny-redirect').on('click', function (e) {
        self.setCookie(true);
        self.nzRedirectModal.modal('hide');
      });
    }

    if (self.el.find('.close-popup').length) {
      self.el.find('.close-popup').on('click', function (e) {
        self.setCookie(true);
        self.nzRedirectModal.modal('hide');
      });
    }
  };

  self.setCookie = function (value = true) {
    Cookies.set(self.cookieName, value, { path: '/' });
  };
}
