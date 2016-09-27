var promise = require('es6-promise'),
    gulp = require('gulp'),
    watch = require('gulp-watch'),
    del = require('del'),
    concat = require('gulp-concat'),
    uglify = require('gulp-uglify'),
    merge = require('merge-stream'),
    order = require('gulp-order'),
    less = require('gulp-less'),
    cleanCSS = require('gulp-clean-css'),
    autoprefixer = require('gulp-autoprefixer'),
    rename = require('gulp-rename');

//clear all files
function clear() {
    del([
        'content/dist/css/**/*.css',
        'content/dist/js/**/*.js',
        'content/dist/fonts/**/*'
    ]);
}

//create css files
function createCss() {
    gulp.src([
       'less/site.less'
    ])
        .pipe(less())
        .pipe(cleanCSS({ compatibility: 'ie8' }))
        .pipe(cleanCSS())
        .pipe(autoprefixer('last 2 version', 'safari 5', 'ie 8', 'ie 9'))
        .pipe(autoprefixer('last 2 version'))
        .pipe(concat('site.min.css'))
        .pipe(gulp.dest('content/dist/css'));
}


//create js files
function createJs() {
    gulp.src([
        'scripts/site.js'
    ])
        .pipe(uglify())
        .pipe(concat('site.min.js'))
        .pipe(gulp.dest('content/dist/js'));
}

gulp.task('watch', function (cb) {
    watch([
        'less/**/*.less',
        'scripts/**/*.js'
    ], function () {
        clear();
        createCss();
        createJs();
    });
});