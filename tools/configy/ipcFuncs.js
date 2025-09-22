const { dialog } = require("electron");
const fs = require("node:fs").promises;
const path = require("node:path");

async function handleOpenDirectory() {
  const res = await dialog.showOpenDialog({ properties: ["openDirectory"] });
  return res;
}

/**
 * Read a list of files from a directory recursively, ignoring specified files and folders.
 * @param {import("electron").IpcMainInvokeEvent} event
 * @param {string} directory The directory to scan the files in
 * @param {Array<string>} extensions List of file extensions to look for (e.g., ['.js', '.txt'])
 * @returns {Promise<Array<ReadFileInfo>>} List of read file information objects array
 */
async function handleReadFiles(event, directory, extensions = []) {
  const files = [];

  // Directories to ignore (case-insensitive)
  const ignoredDirectories = new Set([
    "node_modules",
    ".git",
    ".svn",
    ".hg",
    "dist",
    "build",
    "out",
    "target",
    "bin",
    "obj",
    ".vscode",
    ".idea",
    "__pycache__",
    ".pytest_cache",
    ".coverage",
    "coverage",
    "vendor",
    "bower_components",
    ".next",
    ".nuxt",
    "tmp",
    "temp",
    "logs",
    "log",
  ]);

  // Files to ignore (case-insensitive)
  const ignoredFiles = new Set([
    ".ds_store",
    "thumbs.db",
    "desktop.ini",
    ".gitignore",
    ".gitkeep",
    ".npmignore",
    ".eslintrc",
    ".prettierrc",
    "package-lock.json",
    "yarn.lock",
    "pnpm-lock.yaml",
    ".env",
    ".env.local",
    ".env.development",
    ".env.production",
  ]);

  /**
   * Recursively scan directory for files
   * @param {string} currentDir Current directory being scanned
   */
  async function scanDirectory(currentDir) {
    try {
      const items = await fs.readdir(currentDir, { withFileTypes: true });

      for (const item of items) {
        const itemPath = path.join(currentDir, item.name);
        const itemNameLower = item.name.toLowerCase();

        if (item.isDirectory()) {
          if (ignoredDirectories.has(itemNameLower)) {
            continue; // Skip this directory and all its contents
          }
          // Recursively scan subdirectories if not ignored
          await scanDirectory(itemPath);
        } else if (item.isFile()) {
          if (ignoredFiles.has(itemNameLower)) {
            continue; // Skip this file
          }

          // Check if file matches extension filter (if provided)
          if (
            extensions.length === 0 ||
            matchesExtension(item.name, extensions)
          ) {
            files.push({
              fileName: item.name,
              filePath: itemPath,
              fileDirectory: currentDir,
            });
          }
        }
      }
    } catch (error) {
      // It's common to encounter permission errors, so we log and continue
      console.error(`Error reading directory ${currentDir}:`, error.message);
    }
  }

  /**
   * Check if file matches any of the provided extensions
   * @param {string} fileName Name of the file
   * @param {Array<string>} extensions Array of extensions to match
   * @returns {boolean} True if file matches extension filter
   */
  function matchesExtension(fileName, extensions) {
    const fileExt = path.extname(fileName).toLowerCase();
    return extensions.some((ext) => {
      // Normalize extension (ensure it starts with a dot)
      const normalizedExt = ext.startsWith(".")
        ? ext.toLowerCase()
        : `.${ext.toLowerCase()}`;
      return fileExt === normalizedExt;
    });
  }

  // Start scanning from the root directory
  await scanDirectory(directory);

  return files;
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
    if (!filePath || typeof filePath !== 'string') {
      throw new Error('Invalid file path provided');
    }
    
    if (newContent === undefined || newContent === null) {
      throw new Error('Content cannot be undefined or null');
    }
    
    // Write the content to the file (this will overwrite existing content)
    await fs.writeFile(filePath, newContent, 'utf8');
    
    return { success: true };
    
  } catch (error) {
    console.error('Error writing file:', error);
    return { 
      success: false, 
      error: error.message 
    };
  }
}

module.exports = { handleOpenDirectory, handleReadFiles, handleReadFile, handleOverwriteFileContent };
