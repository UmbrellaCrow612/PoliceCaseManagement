const { contextBridge, ipcRenderer } = require("electron");

contextBridge.exposeInMainWorld("electronAPI", {
  works: () => true,
  openDirectory: () => ipcRenderer.invoke("dialog:openDirectory"),

  /**
   * @param {string} dir - The folder to scan
   * @param {Array<string>} extensions - File extensions to look for
   */
  readFiles: (dir, extensions = []) =>
    ipcRenderer.invoke("file:read", dir, extensions),
});
