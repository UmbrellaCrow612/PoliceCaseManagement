const { app, BrowserWindow, ipcMain, dialog } = require("electron");
const path = require("path");
const fs = require("fs").promises;

async function handleOpenDirectory() {
  var res = await dialog.showOpenDialog({ properties: ["openDirectory"] });
  return res;
}

/**
 * Recursively searches for and reads appsettings.json and launchSettings.json files within a directory,
 * ignoring specified "filler" folders common in .NET projects.
 *
 * @param {string} directoryPath The path to the directory to search.
 * @returns {Promise<Array<{fileName: string, filePath: string, fileContent: string}>>} A promise that resolves to a list of found files with their name, path, and content.
 */
async function readDotNetSettingsFiles(event, directoryPath) {
  const settingsFiles = [];
  // List of folder names to ignore (case-insensitive)
  const ignoredFolders = new Set([
    "obj",
    "bin",
    "node_modules",
    ".git",
    ".vs",
    ".vscode",
  ]);

  async function searchDirectory(currentPath) {
    let files;
    try {
      files = await fs.readdir(currentPath, { withFileTypes: true });
    } catch (error) {
      console.error(`Error reading directory ${currentPath}: ${error.message}`);
      return;
    }

    for (const file of files) {
      const fullPath = path.join(currentPath, file.name);

      if (file.isDirectory()) {
        // Check if the directory name is in our ignore list
        if (!ignoredFolders.has(file.name.toLowerCase())) {
          await searchDirectory(fullPath); // Recurse into subdirectories
        } else {
          console.log(`Ignoring directory: ${fullPath}`);
        }
      } else if (file.isFile()) {
        const lowerCaseFileName = file.name.toLowerCase();
        // Check for appsettings.json, appsettings.development.json, and launchSettings.json
        if (
          lowerCaseFileName.startsWith("appsettings") &&
          lowerCaseFileName.endsWith(".json")
        ) {
          try {
            const content = await fs.readFile(fullPath, "utf8");
            settingsFiles.push({
              fileName: file.name,
              filePath: fullPath,
              fileContent: content,
            });
          } catch (error) {
            console.error(`Error reading file ${fullPath}: ${error.message}`);
          }
        } else if (lowerCaseFileName === "launchsettings.json") {
          try {
            const content = await fs.readFile(fullPath, "utf8");
            settingsFiles.push({
              fileName: file.name,
              filePath: fullPath,
              fileContent: content,
            });
          } catch (error) {
            console.error(`Error reading file ${fullPath}: ${error.message}`);
          }
        }
      }
    }
  }

  await searchDirectory(directoryPath);
  return settingsFiles;
}

let mainWindow;

function createWindow() {
  mainWindow = new BrowserWindow({
    width: 1200,
    height: 800,
    webPreferences: {
      contextIsolation: true,
      preload: path.join(__dirname, "preload.js"),
    },
  });

  if (process.env.NODE_ENV === "development") {
    // 🚀 In dev: use Angular CLI server
    mainWindow.loadURL("http://localhost:4200");
  } else {
    // 📦 In prod: load the built Angular files
    const indexPath = path.join(__dirname, "render", "dist", "index.html");
    mainWindow.loadFile(indexPath);
  }
}

app.whenReady().then(() => {
  ipcMain.handle("dialog:openDirectory", handleOpenDirectory);
  ipcMain.handle("directory:read:dotnet", readDotNetSettingsFiles);

  createWindow();
});

app.on("window-all-closed", () => {
  if (process.platform !== "darwin") {
    app.quit();
  }
});
