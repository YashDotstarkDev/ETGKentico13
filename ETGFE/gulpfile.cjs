const gulp = require("gulp");
const concat = require("gulp-concat");
const cleanCss = require("gulp-clean-css");
const { series } = require("gulp");
const postcss = require("gulp-postcss");
const cssnano = require("cssnano");
const prettyHtml = require("gulp-pretty-html");

function html(cb) {
  gulp.src("./dist/**/*.html").pipe(prettyHtml()).pipe(gulp.dest("dist"));
  cb();
}

function css(cb) {
  gulp
    .src("./dist/assets/**/*.css")
    .pipe(concat("bundle.css"))
    .pipe(
      postcss([
        // https://cssnano.github.io/cssnano/docs/what-are-optimisations/
        // cssnano handles removal of duplicate css, caused by gulp bundling all Astro build css.
        cssnano({ preset: "default" }),
      ]),
    )
    .pipe(gulp.dest("./dist/assets/"));
  cb();
}

exports.default = series(html, css);
