const { contextBridge, ipcRenderer } = require("electron");

contextBridge.exposeInMainWorld("electronAPI", {
  works: () => true,
  openDirectory: () => ipcRenderer.invoke("dialog:openDirectory"),
  readFiles: (dir, options) =>
    ipcRenderer.invoke("files:read", dir, options),
  readFile: (fp) => ipcRenderer.invoke("file:read", fp),
  overWriteFile: (fp, newContent) =>
    ipcRenderer.invoke("file:overwrite", fp, newContent),
});
