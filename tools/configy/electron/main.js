const { app, BrowserWindow, ipcMain } = require("electron");
const path = require("path");
const {
  handleOpenDirectory,
  handleReadFiles,
  handleReadFile,
  handleOverwriteFileContent,
  handleWorks,
  handleOpenFile,
} = require("./ipcFuncs.js");
const { loadEnv, hasEnv } = require("./utils.js");

loadEnv();

let mainWindow;

function createWindow() {
  mainWindow = new BrowserWindow({
    width: 1200,
    height: 800,
    webPreferences: {
      preload: path.join(__dirname, "preload.js"),
    },
  });

  if (!hasEnv("NODE_ENV")) {
    throw new Error("NODE_ENV not set");
  }

  if (process.env.NODE_ENV === "dev") {
    // 🚀 In dev: use Angular CLI server
    mainWindow.loadURL("http://localhost:4200");
  } else {
    // 📦 In prod: Use build script and then cd app and terminal run .\electron.exe ./
    const indexPath = path.join(__dirname, "index.html");
    mainWindow.loadFile(indexPath);

    mainWindow.removeMenu();
    mainWindow.webContents.on("devtools-opened", () => {
      mainWindow.webContents.closeDevTools();
    });
  }
}

app.whenReady().then(() => {
  ipcMain.handle("works", handleWorks);
  ipcMain.handle("dialog:openDirectory", handleOpenDirectory);
  ipcMain.handle("files:read", handleReadFiles);
  ipcMain.handle("file:read", handleReadFile);
  ipcMain.handle("file:overwrite", handleOverwriteFileContent);
  ipcMain.handle("file:open", handleOpenFile);

  createWindow();
});

app.on("window-all-closed", () => {
  if (process.platform !== "darwin") {
    app.quit();
  }
});
