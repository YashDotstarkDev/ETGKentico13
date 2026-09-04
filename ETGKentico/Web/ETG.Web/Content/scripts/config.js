/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	config.allowedContent = true; // To disable CKEditor ACF
    config.enterMode = CKEDITOR.ENTER_BR;
    config.shiftEnterMode = CKEDITOR.ENTER_P;
    config.entities_latin = false;

    var sourceName = config.useInlineMode ? 'Sourcedialog' : 'Source';

    config.toolbar_Standard = config.toolbar_Default =
    [
        [sourceName, '-'],
        ['Undo', 'Redo', '-'],
        ['Bold', 'Italic', 'Underline', 'TextColor', '-'],
        ['Styles'],
        ['NumberedList', 'BulletedList', '-'],
        ['InsertLink', 'Unlink', '-'],
        ['InsertImageOrMedia','-'],
        ['Maximize']
    ];

    config.toolbar = config.toolbar_Standard;

    config.scayt_customerid = '1:vhwPv1-GjUlu4-PiZbR3-lgyTz1-uLT5t-9hGBg2-rs6zY-qWz4Z3-ujfLE3-lheru4-Zzxzv-kq4';
};
