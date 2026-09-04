'use strict';

(function (root, factory) {
  if (typeof define === 'function' && define.amd) {
    // AMD. Register as an anonymous module.
    define([], factory);
  } else if (typeof module === 'object' && module.exports) {
    // Node. Does not work with strict CommonJS, but
    // only CommonJS-like environments that support module.exports,
    // like Node.
    module.exports = factory();
  } else {
    // Browser globals (root is window)
    root.striptags = factory();
  }
}(this, function () {

  var STATE_OUTPUT       = 0,
    STATE_HTML         = 1,
    STATE_PRE_COMMENT  = 2,
    STATE_COMMENT      = 3,
    WHITESPACE         = /\s/,
    ALLOWED_TAGS_REGEX = /<(\w*)>/g;

  function striptags(html, allowableTags, tagReplacement) {
    var html = html || '',
      state = STATE_OUTPUT,
      depth = 0,
      output = '',
      tagBuffer = '',
      inQuote = false,
      i, length, c;

    if (typeof allowableTags === 'string') {
      // Parse the string into an array of tags
      allowableTags = parseAllowableTags(allowableTags);
    } else if (!Array.isArray(allowableTags)) {
      // If it is not an array, explicitly set to null
      allowableTags = null;
    }

    for (i = 0, length = html.length; i < length; i++) {
      c = html[i];

      switch (c) {
        case '<': {
          // ignore '<' if inside a quote
          if (inQuote) {
            break;
          }

          // '<' followed by a space is not a valid tag, continue
          if (html[i + 1] == ' ') {
            consumeCharacter(c);
            break;
          }

          // change to STATE_HTML
          if (state == STATE_OUTPUT) {
            state = STATE_HTML;

            consumeCharacter(c);
            break;
          }

          // ignore additional '<' characters when inside a tag
          if (state == STATE_HTML) {
            depth++;
            break;
          }

          consumeCharacter(c);
          break;
        }

        case '>': {
          // something like this is happening: '<<>>'
          if (depth) {
            depth--;
            break;
          }

          // ignore '>' if inside a quote
          if (inQuote) {
            break;
          }

          // an HTML tag was closed
          if (state == STATE_HTML) {
            inQuote = state = 0;

            if (allowableTags) {
              tagBuffer += '>';
              flushTagBuffer();
            }

            break;
          }

          // '<!' met its ending '>'
          if (state == STATE_PRE_COMMENT) {
            inQuote = state = 0;
            tagBuffer = '';
            break;
          }

          // if last two characters were '--', then end comment
          if (state == STATE_COMMENT &&
            html[i - 1] == '-' &&
            html[i - 2] == '-') {

            inQuote = state = 0;
            tagBuffer = '';
            break;
          }

          consumeCharacter(c);
          break;
        }

        // catch both single and double quotes
        case '"':
        case '\'': {
          if (state == STATE_HTML) {
            if (inQuote == c) {
              // end quote found
              inQuote = false;
            } else if (!inQuote) {
              // start quote only if not already in one
              inQuote = c;
            }
          }

          consumeCharacter(c);
          break;
        }

        case '!': {
          if (state == STATE_HTML &&
            html[i - 1] == '<') {

            // looks like we might be starting a comment
            state = STATE_PRE_COMMENT;
            break;
          }

          consumeCharacter(c);
          break;
        }

        case '-': {
          // if the previous two characters were '!-', this is a comment
          if (state == STATE_PRE_COMMENT &&
            html[i - 1] == '-' &&
            html[i - 2] == '!') {

            state = STATE_COMMENT;
            break;
          }

          consumeCharacter(c);
          break;
        }

        case 'E':
        case 'e': {
          // check for DOCTYPE, because it looks like a comment and isn't
          if (state == STATE_PRE_COMMENT &&
            html.substr(i - 6, 7).toLowerCase() == 'doctype') {

            state = STATE_HTML;
            break;
          }

          consumeCharacter(c);
          break;
        }

        default: {
          consumeCharacter(c);
        }
      }
    }

    function consumeCharacter(c) {
      if (state == STATE_OUTPUT) {
        output += c;
      } else if (allowableTags && state == STATE_HTML) {
        tagBuffer += c;
      }
    }

    function flushTagBuffer() {
      var normalized = '',
        nonWhitespaceSeen = false,
        i, length, c;

      normalizeTagBuffer:
        for (i = 0, length = tagBuffer.length; i < length; i++) {
          c = tagBuffer[i].toLowerCase();

          switch (c) {
            case '<': {
              break;
            }

            case '>': {
              break normalizeTagBuffer;
            }

            case '/': {
              nonWhitespaceSeen = true;
              break;
            }

            default: {
              if (!c.match(WHITESPACE)) {
                nonWhitespaceSeen = true;
                normalized += c;
              } else if (nonWhitespaceSeen) {
                break normalizeTagBuffer;
              }
            }
          }
        }

      if (allowableTags.indexOf(normalized) !== -1) {
        output += tagBuffer;
      } else if (tagReplacement) {
        output += tagReplacement;
      }

      tagBuffer = '';
    }

    return output;
  }

  /**
   * Return an array containing tags that are allowed to pass through the
   * algorithm.
   *
   * @param string allowableTags A string of tags to allow (e.g. "<b><strong>").
   * @return array|null An array of allowed tags or null if none.
   */
  function parseAllowableTags(allowableTags) {
    var tagsArray = [],
      match;

    while ((match = ALLOWED_TAGS_REGEX.exec(allowableTags)) !== null) {
      tagsArray.push(match[1]);
    }

    return tagsArray.length !== 0 ? tagsArray : null;
  }

  return striptags;
}));

$.fn.setContentEditableSelection = function(position, length) {
  if (typeof(length) == "undefined") {
    length = 0;
  }
  return this.each(function() {
    var $this = $(this);
    var editable = this;
    var selection;
    var range;
    var html = $this.html();
    html = html.substring(0, position) +
      '<a id="cursorStart"></a>' +
      html.substring(position, position + length) +
      '<a id="cursorEnd"></a>' +
      html.substring(position + length, html.length);
    console.log(html);
    $this.html(html);
    // Populates selection and range variables
    var captureSelection = function(e) {
      // Don't capture selection outside editable region
      var isOrContainsAnchor = false,
        isOrContainsFocus = false,
        sel = window.getSelection(),
        parentAnchor = sel.anchorNode,
        parentFocus = sel.focusNode;
      while (parentAnchor && parentAnchor != document.documentElement) {
        if (parentAnchor == editable) {
          isOrContainsAnchor = true;
        }
        parentAnchor = parentAnchor.parentNode;
      }
      while (parentFocus && parentFocus != document.documentElement) {
        if (parentFocus == editable) {
          isOrContainsFocus = true;
        }
        parentFocus = parentFocus.parentNode;
      }
      if (!isOrContainsAnchor || !isOrContainsFocus) {
        return;
      }
      selection = window.getSelection();
      // Get range (standards)
      if (selection.getRangeAt !== undefined) {
        range = selection.getRangeAt(0);
        // Get range (Safari 2)
      } else if (
        document.createRange &&
        selection.anchorNode &&
        selection.anchorOffset &&
        selection.focusNode &&
        selection.focusOffset
      ) {
        range = document.createRange();
        range.setStart(selection.anchorNode, selection.anchorOffset);
        range.setEnd(selection.focusNode, selection.focusOffset);
      } else {
        // Failure here, not handled by the rest of the script.
        // Probably IE or some older browser
      }
    };
    // Slight delay will avoid the initial selection
    // (at start or of contents depending on browser) being mistaken
    setTimeout(function() {
      var cursorStart = document.getElementById('cursorStart');
      var cursorEnd = document.getElementById('cursorEnd');
      // Don't do anything if user is creating a new selection
      if (editable.className.match(/\sselecting(\s|$)/)) {
        if (cursorStart) {
          cursorStart.parentNode.removeChild(cursorStart);
        }
        if (cursorEnd) {
          cursorEnd.parentNode.removeChild(cursorEnd);
        }
      } else if (cursorStart) {
        captureSelection();
        range = document.createRange();
        if (cursorEnd) {
          range.setStartAfter(cursorStart);
          range.setEndBefore(cursorEnd);
          // Delete cursor markers
          cursorStart.parentNode.removeChild(cursorStart);
          cursorEnd.parentNode.removeChild(cursorEnd);
          // Select range
          selection.removeAllRanges();
          selection.addRange(range);
        } else {
          range.selectNode(cursorStart);
          // Select range
          selection.removeAllRanges();
          selection.addRange(range);
          // Delete cursor marker
          document.execCommand('delete', false, null);
        }
      }
      // Register selection again
      captureSelection();
    }, 10);
  });
};
