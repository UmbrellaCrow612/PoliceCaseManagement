// preload.js
const { contextBridge, ipcRenderer } = require("electron");

/**
 * Api exposed in main world with real impl
 * @type {ElectronAPI}
 */
const api = {
  works: () => ipcRenderer.invoke("works"),
  openDirectory: () => ipcRenderer.invoke("dialog:openDirectory"),
  readFiles: (dir, options) => ipcRenderer.invoke("files:read", dir, options),
  readFile: (fp) => ipcRenderer.invoke("file:read", fp),
  overWriteFile: (fp, newContent) =>
    ipcRenderer.invoke("file:overwrite", fp, newContent),
  openFile: (options) => ipcRenderer.invoke("file:open", options),
};

contextBridge.exposeInMainWorld("electronAPI", api);
