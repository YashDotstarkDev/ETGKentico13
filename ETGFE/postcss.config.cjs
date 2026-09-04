module.exports = {
  parser: require('postcss-comment'),
  sourceMap: true,
  plugins: {
    'postcss-flexbox': {},

    // 'precss' is deprecated, replacing with individual plugins.
    // See: https://www.npmjs.com/package/precss
    'postcss-extend-rule': {},
    'postcss-advanced-variables': {},
    'postcss-preset-env': {},
    'postcss-nested': {},

    // more plugins
    'rucksack-css': {},
    'postcss-utilities': {},
    'postcss-math': {},
    'postcss-sass-color-functions': {},

    // autoprefixer
    'autoprefixer': {},

    // minifies webpack extracted .css
    'cssnano': {
      preset: ['default', {
        discardComments: {
          removeAll: true,
        },
      }]
    }
  }
}
