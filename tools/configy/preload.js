const { contextBridge, ipcRenderer, ipcMain } = require("electron");

contextBridge.exposeInMainWorld("electronAPI", {
  works: () => true,
  openDirectory: () => ipcRenderer.invoke("dialog:openDirectory"),
  readDotNetFiles: (/** @type {string} */ dir) =>
    ipcRenderer.invoke("directory:read:dotnet", dir),
});
