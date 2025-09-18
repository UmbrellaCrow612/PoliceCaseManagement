// main.js (CommonJS)
const { app, BrowserWindow, ipcMain, dialog } = require("electron");
const fs = require('fs');
const path = require("node:path");

async function handleOpenFolderDirectory() {
  var value = await dialog.showOpenDialog({ properties: ["openDirectory"] });
  return value;
}

/**
 * Reads .NET JSON configuration files from a folder and its subfolders,
 * including environment-specific appsettings files (e.g., appsettings.Development.json),
 * while ignoring irrelevant folders.
 * Returns an array of objects with filePath, fileName, and fileContent (string).
 * @param {Electron.IpcMainEvent} event
 * @param {string} folderPath
 * @returns {Array<{filePath: string; fileName: string; fileContent: string;}>}
 */
function handleReadDotNetJsonFiles(event, folderPath) {
  const result = [];
  const IGNORED_FOLDERS = ['node_modules', 'bin', 'obj', '.git', 'dist'];

  function readFolderRecursively(currentPath) {
    const filesAndDirs = fs.readdirSync(currentPath);

    filesAndDirs.forEach(name => {
      const fullPath = path.join(currentPath, name);
      const stats = fs.statSync(fullPath);

      if (stats.isDirectory()) {
        if (!IGNORED_FOLDERS.includes(name)) {
          readFolderRecursively(fullPath);
        }
      } else if (stats.isFile() && path.extname(fullPath).toLowerCase() === '.json') {
        const lowerName = name.toLowerCase();
        const isAppSettings = lowerName === 'appsettings.json' || lowerName.startsWith('appsettings.');
        const isLaunchSettings = lowerName === 'launchsettings.json';

        if (isAppSettings || isLaunchSettings) {
          let content = null;
          try {
            content = fs.readFileSync(fullPath, 'utf-8'); // Read as string
          } catch (err) {
            console.warn(`Failed to read JSON for file: ${fullPath}`);
          }

          result.push({
            filePath: fullPath,
            fileName: name,
            fileContent: content
          });
        }
      }
    });
  }

  readFolderRecursively(folderPath);
  return result;
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
  ipcMain.handle("read-json-files-from-dir", handleReadDotNetJsonFiles)

  
  createStartWindow();
});

app.on("window-all-closed", () => {
  if (process.platform !== "darwin") app.quit();
});
