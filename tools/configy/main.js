const { app, BrowserWindow, ipcMain } = require("electron");
const path = require("path");
const {
  handleOpenDirectory,
  handleReadFiles,
  handleReadFile,
  handleOverwriteFileContent,
  handleWorks,
} = require("./ipcFuncs.js");
const { loadEnv, hasEnv } = require("./util.js");

loadEnv();

let mainWindow;

function createWindow() {
  mainWindow = new BrowserWindow({
    width: 1200,
    height: 800,
    webPreferences: {
      contextIsolation: true,
      preload: path.join(__dirname, "preload.js"),
    },
  });

  if (!hasEnv("NODE_ENV")) {
    throw new Error("NODE_ENV not set");
  }

  if (process.env.NODE_ENV === "development") {
    // 🚀 In dev: use Angular CLI server
    mainWindow.loadURL("http://localhost:4200");
  } else {
    // 📦 In prod: load the built Angular files - run build script first
    const indexPath = path.join(
      __dirname,
      "render",
      "dist",
      "browser",
      "index.html"
    );
    mainWindow.loadFile(indexPath);
  }
}

app.whenReady().then(() => {
  ipcMain.handle("works", handleWorks);
  ipcMain.handle("dialog:openDirectory", handleOpenDirectory);
  ipcMain.handle("files:read", handleReadFiles);
  ipcMain.handle("file:read", handleReadFile);
  ipcMain.handle("file:overwrite", handleOverwriteFileContent);

  createWindow();
});

app.on("window-all-closed", () => {
  if (process.platform !== "darwin") {
    app.quit();
  }
});
