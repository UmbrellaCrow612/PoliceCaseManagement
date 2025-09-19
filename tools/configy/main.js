const { app, BrowserWindow, ipcMain, dialog } = require("electron");
const path = require("path");

async function handleOpenDirectory() {
  var res = await dialog.showOpenDialog({ properties: ["openDirectory"] });
  return res;
}

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

  if (process.env.NODE_ENV === "development") {
    // 🚀 In dev: use Angular CLI server
    mainWindow.loadURL("http://localhost:4200");
  } else {
    // 📦 In prod: load the built Angular files
    const indexPath = path.join(__dirname, "render", "dist", "index.html");
    mainWindow.loadFile(indexPath);
  }
}

app.whenReady().then(() => {
  ipcMain.handle("dialog:openDirectory", handleOpenDirectory);

  createWindow();
});

app.on("window-all-closed", () => {
  if (process.platform !== "darwin") {
    app.quit();
  }
});
