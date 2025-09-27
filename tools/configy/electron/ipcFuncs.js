// File contaisn all handlers for ipc process

const { dialog } = require("electron");
const fs = require("node:fs/promises");
const path = require("node:path");


// In main process
const electronAPIFunctions = [
  "works",
  "openDirectory",
  "readFiles",
  "readFile",
  "overWriteFile",
];

/**
 * Tells clients who use the electron API which functions are available
 * @param {import("electron").IpcMainInvokeEvent} event Electron event
 */
function handleWorks(event) {
  // Return the list of functions to the renderer
  return electronAPIFunctions;
}

/**
 * Handles openign directory selection
 */
async function handleOpenDirectory() {
  const res = await dialog.showOpenDialog({ properties: ["openDirectory"] });
  return res;
}

/**
 * Read a list a list of files from a directory
 * @param {import("electron").IpcMainInvokeEvent} event Electron event
 * @param {string} dir The directory to read the files from
 * @param {ReadFileOptions} options
 * @returns {Promise<Array<ReadFileInfo>>} List of files info objects
 */
async function handleReadFiles(
  event,
  dir,
  options = {
    fileExts: new Set(),
    fileNames: new Set(),
    ignoreDirs: new Set(),
    ignoreFileNames: new Set(),
  }
) {
  /**
   * @type {Array<ReadFileInfo>}
   */
  const files = [];

  const normalizedIgnoreDirs = new Set();
  options.ignoreDirs.forEach((element) => {
    normalizedIgnoreDirs.add(element.toLocaleLowerCase().trim());
  });
  // Common ignores
  normalizedIgnoreDirs.add("node_modules");

  // C# ignores
  normalizedIgnoreDirs.add("bin");
  normalizedIgnoreDirs.add("obj");
  normalizedIgnoreDirs.add(".vs");
  normalizedIgnoreDirs.add("packages");

  // Angular/JS/TS ignores
  normalizedIgnoreDirs.add("dist");
  normalizedIgnoreDirs.add("out-tsc");
  normalizedIgnoreDirs.add(".angular");
  normalizedIgnoreDirs.add("coverage");

  // Git
  normalizedIgnoreDirs.add(".git");

  const normalizedIgnoreFilenames = new Set();
  options.ignoreFileNames.forEach((element) => {
    normalizedIgnoreFilenames.add(element.toLocaleLowerCase().trim());
  });

  const normalizedFileNames = new Set();
  options.fileNames.forEach((element) => {
    normalizedFileNames.add(element.toLowerCase().trim());
  });

  const normalizedFileExts = new Set();
  options.fileExts.forEach((element) => {
    normalizedFileExts.add(element.toLowerCase().trim());
  });

  /**
   * Recursive read a dir
   * @param {string} dir The directory to read
   */
  async function recursive(dir) {
    let items = await fs.readdir(dir, { withFileTypes: true });

    for (const item of items) {
      let name = item.name.toLowerCase().trim();
      let fullPath = path.join(dir, item.name);

      if (item.isDirectory()) {
        if (normalizedIgnoreDirs.has(name)) {
          continue;
        }

        await recursive(fullPath);
      }

      if (item.isFile()) {
        if (normalizedIgnoreFilenames.has(name)) {
          continue;
        }

        // Case 1: Match specific filenames
        if (normalizedFileNames.size > 0) {
          if (normalizedFileNames.has(name)) {
            files.push({
              fileDirectory: dir,
              fileName: name,
              filePath: fullPath,
            });
          }
          continue; // stop here so we don’t add non-matching files
        }

        // Case 2: Match extensions
        if (normalizedFileExts.size > 0) {
          if (hasFileExt(name, Array.from(normalizedFileExts))) {
            files.push({
              fileDirectory: dir,
              fileName: name,
              filePath: fullPath,
            });
          }
          continue;
        }

        // Case 3: No filters → add everything
        files.push({
          fileDirectory: dir,
          fileName: name,
          filePath: fullPath,
        });
      }
    }
  }

  await recursive(dir);

  return files;
}

/**
 * Check if a file name has any of the file extensions you're looking for
 * @param {string} fileName The name of the file
 * @param {Array<string>} exts The file extensions to look for
 * @returns {boolean} True if the file has one of the given extensions
 */
function hasFileExt(fileName, exts) {
  if (typeof fileName !== "string" || !Array.isArray(exts)) return false;

  // Extract the extension from the filename
  const dotIndex = fileName.lastIndexOf(".");
  if (dotIndex === -1 || dotIndex === fileName.length - 1) {
    return false; // no extension or ends with "."
  }

  const fileExt = fileName.slice(dotIndex + 1).toLowerCase();

  return exts.some((ext) => fileExt === ext.toLowerCase().replace(/^\./, ""));
}

/**
 * Read a specific file's content - if it does not exist or fails, returns an empty string
 * @param {import("electron").IpcMainInvokeEvent} event
 * @param {string} filePath - The path of the file to read
 * @returns {Promise<string>} Content as a string
 */
async function handleReadFile(event, filePath) {
  try {
    const data = await fs.readFile(filePath, "utf8");
    return data;
  } catch {
    return "";
  }
}

/**
 * Write the new content to a file
 * @param {import("electron").IpcMainInvokeEvent} event
 * @param {string} filePath The path to the file
 * @param {string} newContent the new content
 * @returns {Promise<{success: boolean, error?: string}>}
 */
async function handleOverwriteFileContent(event, filePath, newContent) {
  try {
    // Validate input parameters
    if (!filePath || typeof filePath !== "string") {
      throw new Error("Invalid file path provided");
    }

    if (newContent === undefined || newContent === null) {
      throw new Error("Content cannot be undefined or null");
    }

    // Write the content to the file (this will overwrite existing content)
    await fs.writeFile(filePath, newContent, "utf8");

    return { success: true };
  } catch (error) {
    console.error("Error writing file:", error);
    return {
      success: false,
      error: error.message,
    };
  }
}

module.exports = {
  handleOpenDirectory,
  handleReadFiles,
  handleReadFile,
  handleOverwriteFileContent,
  handleWorks
};
