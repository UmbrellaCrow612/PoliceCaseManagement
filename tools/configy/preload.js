const { contextBridge, ipcRenderer } = require("electron");

contextBridge.exposeInMainWorld("electronAPI", {
  works: () => true,
  openDirectory: () => ipcRenderer.invoke("dialog:openDirectory"),
  readFiles: (dir, extensions = []) =>
    ipcRenderer.invoke("files:read", dir, extensions),
  readFile: (fp) => ipcRenderer.invoke("file:read", fp),
  overWriteFile: (fp, newContent) =>
    ipcRenderer.invoke("file:overwrite", fp, newContent),
});
