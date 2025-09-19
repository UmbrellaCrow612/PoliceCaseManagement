const { contextBridge, ipcRenderer } = require("electron");

contextBridge.exposeInMainWorld("electronAPI", {
  works: () => true,
  openDirectory: () => ipcRenderer.invoke("dialog:openDirectory"),
});
