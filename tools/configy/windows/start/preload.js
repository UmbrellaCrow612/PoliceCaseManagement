// preload.js (CommonJS)
const { contextBridge, ipcRenderer } = require("electron");

contextBridge.exposeInMainWorld("electronAPI", {
  works: () => true,
  OpenFolderDirectory: () => ipcRenderer.invoke("open-folder-dir"),
  ReadAllJsonFilesFromDir: (/** @type {string} */ dir) =>
    ipcRenderer.invoke("read-json-files-from-dir", dir),
});
