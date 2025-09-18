// preload.js (CommonJS)
const { contextBridge, ipcRenderer } = require("electron");

contextBridge.exposeInMainWorld("electronAPI", {
  works: () => true,
  OpenFolderDirectory: () => ipcRenderer.invoke("open-folder-dir"),
});
