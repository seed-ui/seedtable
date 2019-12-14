const electron = require('electron');
const app = electron.app;
const BrowserWindow = electron.BrowserWindow;
let mainWindow;

app.on('window-all-closed', () => {
    app.quit();
});

app.on('ready', () => {
    // お使いの画面解像度に応じて調整してください
    mainWindow = new BrowserWindow({
        width: 600,
        height: 400,
        webPreferences: {
            nodeIntegration: true
        }
    });

    mainWindow.loadURL('file://' + __dirname + '/index.html');
    // mainWindow.webContents.openDevTools(); // test
    mainWindow.on('closed', ()  => {
        mainWindow = null;
    });
});