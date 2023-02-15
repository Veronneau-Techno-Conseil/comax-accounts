var browserify = require('browserify');
var tsify = require('tsify');
var fs = require('fs');

browserify()
    .add('./node_modules/select-picker/dist/picker.min.js')
    .add('./node_modules/bulma-calendar/dist/js/bulma-calendar.min.js')
    .add('./assets/ts/site.ts') // main entry of an application
    .plugin(tsify, { noImplicitAny: true })
    //.plugin(watchify)
    .bundle()
    .on('error', function (error) { console.error(error.toString()); })
    .pipe(fs.createWriteStream("./wwwroot/js/site.js"));
