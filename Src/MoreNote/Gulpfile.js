/// <binding AfterBuild='default' />
var gulp = require('gulp');
var clean = require('gulp-clean');
var uglify = require('gulp-uglify');
var rename = require('gulp-rename');
var minifyHtml = require("gulp-minify-html");
var concat = require('gulp-concat');
var replace = require('gulp-replace');
var inject = require('gulp-inject');
var gulpSequence = require('gulp-sequence');
var minifycss = require('gulp-minify-css');
var fs = require('fs');
const { series, src, dest, watch } = require('gulp');


var leanoteBase = process.cwd();
var base = leanoteBase + '/wwwroot'; // public base
var noteDev = leanoteBase + '/app/views/note/note-dev.html';
var noteProBase = leanoteBase + '/app/views/note';

var messagesPath = leanoteBase + 'messages';

const { resolve } = require('path')

// 合并Js, 这些js都是不怎么修改, 且是依赖
// 840kb, 非常耗时!!
function concatDepJs() {
    console.log('__dirname : ' + __dirname)
    console.log('resolve   : ' + resolve('./'))
    console.log('cwd       : ' + process.cwd())

    var jss = [
        'js/jquery-1.9.0.min.js',
        'js/jquery.ztree.all-3.5-min.js',
        // 'tinymce/tinymce.full.min.js', // 使用打成的包, 加载速度快
        // 'libs/ace/ace.js',
        'js/jQuery-slimScroll-1.3.0/jquery.slimscroll-min.js',
        'js/contextmenu/jquery.contextmenu-min.js',
        'js/bootstrap-min.js',
        'js/object_id.js',
    ];

    for (var i in jss) {
        jss[i] = base + '/' + jss[i];
    }

    return gulp
        .src(jss)
        // .pipe(uglify()) // 压缩
        .pipe(concat('dep.min.js'))
        .pipe(gulp.dest(base + '/js'));
}
// 合并app js 这些js会经常变化 90kb
function concatAppJs() {
    var jss = [
        'js/common.js',
        'js/app/note.js',
        'js/app/page.js', // 写作模式下, page依赖note
        'js/app/tag.js',
        'js/app/notebook.js',
        'js/app/share.js',
    ];

    for (var i in jss) {
        jss[i] = base + '/' + jss[i];
    }

    return gulp
        .src(jss)
        .pipe(uglify()) // 压缩
        .pipe(concat('app.min.js'))
        .pipe(gulp.dest(base + '/js'));
}
function plugins() {
    // gulp.src(base + '/js/plugins/libs/*.js')
    //     .pipe(uglify()) // 压缩
    //     // .pipe(concat('main.min.js'))
    //     .pipe(gulp.dest(base + '/js/plugins/libs-min'));

    // 所有js合并成一个
    var jss = [
        'note_info',
        'tips',
        'history',
        'attachment_upload',
        'editor_drop_paste',
        'main'
    ];

    for (var i in jss) {
        jss[i] = base + '/js/plugins/' + jss[i] + '.js';
    }
    jss.push(base + '/js/plugins/libs-min/fileupload.js');
    
   return gulp.src(jss)
        .pipe(uglify()) // 压缩
        .pipe(concat('main.min.js'))//合并
        .pipe(gulp.dest(base + '/js/plugins'));
}
function concatMarkdownJs() {
    var jss = [
        'js/require.js',
        'md/main.min.js',
    ];

    for (var i in jss) {
        jss[i] = base + '/' + jss[i];
    }

    return gulp
        .src(jss)
        .pipe(uglify()) // 压缩
        .pipe(concat('markdown.min.js'))
        .pipe(gulp.dest(base + '/js'));
}
function concatMarkdownJsV2() {
    var jss = [
        'js/require.js',
        'md/main-v2.min.js',
    ];

    for (var i in jss) {
        jss[i] = base + '/' + jss[i];
    }

    return gulp
        .src(jss)
        .pipe(uglify()) // 压缩
        .pipe(concat('markdown-v2.min.js'))
        .pipe(gulp.dest(base + '/js'));
}
// note-dev.html -> note.html, 替换css, js
// TODO 加?t=2323232, 强制浏览器更新, 一般只需要把app.min.js
function devToProHtml() {
    return gulp
        .src(noteDev)
        .pipe(replace(/<!-- dev -->[.\s\S]+?<!-- \/dev -->/g, '')) // 把dev 去掉
        .pipe(replace(/<!-- pro_dep_js -->/, '<script src="/js/dep.min.js"></script>')) // 替换
        .pipe(replace(/<!-- pro_app_js -->/, '<script src="/js/app.min.js"></script>')) // 替换
        // .pipe(replace(/<!-- pro_markdown_js -->/, '<script src="/js/markdown.min.js"></script>')) // 替换
        .pipe(replace(/<!-- pro_markdown_js -->/, '<script src="/js/markdown-v2.min.js"></script>')) // 替换
        .pipe(replace('/tinymce/tinymce.js', '/tinymce/tinymce.full.min.js')) // 替换
        .pipe(replace(/<!-- pro_tinymce_init_js -->/, "var tinyMCEPreInit = {base: '/public/tinymce', suffix: '.min'};")) // 替换
        .pipe(replace(/plugins\/main.js/, "plugins/main.min.js")) // 替换
        // 连续两个空行换成一个空行
        .pipe(replace(/\r\n\r\n/g, '\r\n'))
        .pipe(replace(/\r\n\r\n/g, '\r\n'))
        .pipe(replace(/\r\n\r\n/g, '\r\n'))
        .pipe(replace(/\r\n\r\n/g, '\r\n'))
        .pipe(replace(/\r\n\r\n/g, '\r\n'))
        .pipe(replace(/\r\n\r\n/g, '\r\n'))
        .pipe(replace('console.log(o);', ''))
        .pipe(replace('console.trace(o);', ''))
        // .pipe(minifyHtml()) // 不行, 压缩后golang报错
        .pipe(rename('note.html'))
        .pipe(gulp.dest(noteProBase));
}
// 只获取需要js i18n的key

function concatAlbumJs() {
    /*
       gulp.src(base + '/album/js/main.js')
           .pipe(uglify()) // 压缩
           .pipe(rename({suffix: '.min'}))
           .pipe(gulp.dest(base + '/album/js/'));
       */

    gulp.src(base + '/album/css/style.css')
        .pipe(rename({ suffix: '-min' }))
        .pipe(minifycss())
        .pipe(gulp.dest(base + '/album/css'));

    var jss = [
        'js/jquery-1.9.0.min.js',
        'js/bootstrap-min.js',
        'js/plugins/libs-min/fileupload.js',
        'js/jquery.pagination.js',
        'album/js/main.js',
    ];

    for (var i in jss) {
        jss[i] = base + '/' + jss[i];
    }

    return gulp
        .src(jss)
        .pipe(uglify()) // 压缩
        .pipe(concat('main.all.js'))
        .pipe(gulp.dest(base + '/album/js'));
}
function concatCss() {
    return gulp
        .src([markdownRaw + '/css/default.css', markdownRaw + '/css/md.css'])
        .pipe(concat('all.css'))
        .pipe(gulp.dest(markdownMin));
}
function minifycss() {
    gulp.src(base + '/css/bootstrap.css')
        .pipe(rename({ suffix: '-min' }))
        .pipe(minifycss())
        .pipe(gulp.dest(base + '/css'));

    gulp.src(base + '/css/font-awesome-4.2.0/css/font-awesome.css')
        .pipe(rename({ suffix: '-min' }))
        .pipe(minifycss())
        .pipe(gulp.dest(base + '/css/font-awesome-4.2.0/css'));

    gulp.src(base + '/css/zTreeStyle/zTreeStyle.css')
        .pipe(rename({ suffix: '-min' }))
        .pipe(minifycss())
        .pipe(gulp.dest(base + '/css/zTreeStyle'));

    gulp.src(base + '/md/themes/default.css')
        .pipe(rename({ suffix: '-min' }))
        .pipe(minifycss())
        .pipe(gulp.dest(base + '/md/themes'));

    return gulp.src(base + '/js/contextmenu/css/contextmenu.css')
        .pipe(rename({ suffix: '-min' }))
        .pipe(minifycss())
        .pipe(gulp.dest(base + '/js/contextmenu/css'));

    // theme
    // 用codekit
    // var as = ['default', 'simple', 'writting', /*'writting-overwrite', */ 'mobile'];

}

exports.concat = gulp.series(concatDepJs, concatAppJs,concatMarkdownJsV2);
exports.html = gulp.series(devToProHtml);
//exports.minifycss = minifycss;
exports.default = gulp.series(concat, plugins, minifycss, concatAlbumJs);



