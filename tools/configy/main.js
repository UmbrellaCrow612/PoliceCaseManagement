// main.js (CommonJS)
const { app, BrowserWindow, ipcMain, dialog } = require("electron");

const path = require("node:path");

async function handleOpenFolderDirectory() {
  var value = await dialog.showOpenDialog({ properties: ["openDirectory"] });
  return value;
}

/**
 * Create a start window instance
 */
const createStartWindow = () => {
  const win = new BrowserWindow({
    width: 800,
    height: 600,
    webPreferences: {
      preload: path.join(__dirname, "windows/start/preload.js"),
    },
  });

  win.loadFile("./windows/start/index.html");
};

app.whenReady().then(() => {
  ipcMain.handle("open-folder-dir", handleOpenFolderDirectory);

  
  createStartWindow();
});

app.on("window-all-closed", () => {
  if (process.platform !== "darwin") app.quit();
});
