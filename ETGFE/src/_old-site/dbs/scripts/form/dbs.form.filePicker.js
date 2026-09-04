'use strict';

window.dbs = window.dbs || {};
dbs.form = dbs.form || {};

dbs.form.filePicker = function (el) {
  var self = this;
  self.el = el;
  self.events = new dbs.events();
  self.files = [];
  self.index = 0;

  self.init = function () {
    // bind the file input
    self.el.find('input[type="file"]').on('change', function(e) {
      for (var f = 0; f < e.target.files.length; f++) {
        if(self.files.length < 10) {
          self.files.push({file: e.target.files[f]});
        }
      }
      self.events.emit('FilePicker:files_picked', self.files, true);
      self.parseFiles();
    });

    // hide the file input
    self.el.find('input[type="file"]').hide();

    // bind the trigger div
    self.el.find('.trigger').click(function(e){
      e.preventDefault();
      self.el.find('input[type="file"]').trigger('click');
    });

    self.el.on('click', '.remove-file', function(e){
      e.preventDefault();
      self.removeFile(parseInt($(this).parents('.file').attr('data-index')));
    });
  };

  self.updateCount = function () {
    self.events.emit('FilePicker:update_count', self.files, true);

    var fc = '0 files';
    if(self.files.length === 1) {
      fc = '1 file';
    }
    if(self.files.length > 1) {
      fc = self.files.length + ' files';
    }
    self.el.find('.file-count').html(fc);
  };

  self.updateJson = function () {

    var json = [];

    for (var f = 0; f < self.files.length; f++) {
      var file = self.files[f];

      json.push({
        name: file.name,
        data: file.reader.result
      });
    }

    self.el.find('.file-json').val(JSON.stringify(json));
  };

  self.parseFiles = function () {
    for (var f = 0; f < self.files.length; f++) {
      var file = self.files[f];
      file.extension = self.getFileExtension(file.file.name);
      file.name = file.file.name;
      file.size = {};
      file.size.kb = self.getFileSize(file.file.size, 'kb');
      file.size.mb = self.getFileSize(file.file.size, 'mb');
      file.size.friendly = self.getFileSize(file.file.size, 'friendly');
      file.index = self.index;

      self.index ++;

      if(!file.reader) {
        file.reader = new FileReader();
        self.readFile(file);
      }
    }

    // self.events.emit('FilePicker:files_parsed', self.files, true);

    self.updateCount();
    self.listFiles();
  };

  self.removeFile = function (index) {
    for (var f = 0; f < self.files.length; f++) {
      var file = self.files[f];
      if(file.index === index) {
        self.files.splice(f, 1);
      }
    }

    self.updateCount();
    self.listFiles();
  };

  self.listFiles = function () {

    if(!self.files.length) {
      self.updateJson();
    }

    self.el.find('.file-list').html('');
    for (var f = 0; f < self.files.length; f++) {
      var file = self.files[f];
      self.listFile(file);
    }

    self.updateJson();
  };

  self.getFilePreview = function (file) {
    switch (file.file.type.split('/')[0]) {
      case 'image':
        return '<div class="file-preview image-preview" style="background-image:url(' + file.reader.result + ')"></div>';
        break;

      default:
        return '<div class="file-preview"><div class="file-extension">' + file.extension + '</div></div>';
        break;
    }
  };

  self.listFile = function (file) {
    file.el = $('<div class="file" data-index="' + file.index + '">' + self.getFilePreview(file) + '<div class="file-label"><div class="file-name">' + file.name + '</div><div class="file-size">' + file.size.friendly + '</div></div><div class="remove-file"><span class="icon"></span></div></div>');
    self.el.find('.file-list').append(file.el);
  };

  self.readFile = function(file) {
    file.reader.addEventListener('load', function () {
      self.events.emit('FilePicker:file_read', file, true);
      self.listFiles();
    }, false);
    file.reader.readAsDataURL(file.file);
  };

  self.getFileExtension = function (filename) {
    var nameArray = filename.split('.');
    return nameArray[nameArray.length - 1];
  };

  self.getFileSize = function (size, units) {
    switch (units) {
      case 'kb':
        return size / 1024;
      case 'mb':
        return size / 1024 / 1024;
      case 'friendly':
        if(size / 1024 / 1024 >= 1) {
          return Math.round(size / 1024 / 1024) + 'mb';
        } else if(size / 1024 >= 1) {
          return Math.round(size / 1024) + 'kb';
        } else {
          return (size) + 'b';
        }
      default:
        return size;
    }
  };

  self.init();

};
