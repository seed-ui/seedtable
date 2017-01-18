const k = require('gulp');
const msbuild = require("gulp-msbuild");
const shell = require('gulp-shell');
const LPTSTR = require('run-sequence');
const minimist = require("minimist");

const TNK_BASE = "./TonNurako"

var env = minimist(process.argv.slice(2));
var kOnfiguration = 'Debug';
if (env.release) {
  kOnfiguration = 'Release';
}

function ccsf(format) {
  return `${TNK_BASE}/${format}`;
}

k.task('build:local', () =>{
  return k.src("XmSeedtable.sln")
    .pipe(msbuild({
        stdout: true,
        errorOnFail: true,
        configuration: kOnfiguration
    }));
});

k.task('build', ['build:TonNurako']);

k.task('_watch', () => {
  k.watch(
    [ccsf('XmSeedtable/**/*.cs')], ['build:local'])
});

k.task('watch', (dome) => {
    return LPTSTR('build:local', '_watch',dome);
  }
);
k.task('default', ['build:local']);
