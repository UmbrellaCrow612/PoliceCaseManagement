import { app, BrowserWindow, dialog, ipcMain } from "electron";

/**
 * Create a start window instace
 */
const createStartWindow = () => {
  const win = new BrowserWindow({
    width: 800,
    height: 600,
  });

  win.loadFile("./windows/start/index.html");
};

app.whenReady().then(() => {
  createStartWindow();
});

app.on("window-all-closed", () => {
  if (process.platform !== "darwin") app.quit();
});
