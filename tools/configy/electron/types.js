/**
 * Information of a file that way read
 * @typedef {Object} ReadFileInfo
 * @property {string} fileName - The name of the file including extension.
 * @property {string} filePath - The full path to the file.
 * @property {string} fileDirectory - The directory where the file is located.
 */

/**
 * List of optiosn you can pass when reading files from a directory
 * @typedef {Object} ReadFileOptions
 * @property {Set<string>} fileNames - List of file names to look for
 * @property {Set<string>} fileExts - List of file extensions you want to look for
 * @property {Set<string>} ignoreFileNames - List of file names to ignore
 * @property {Set<string>} ignoreDirs - List of folders to ignore
 */

/**
 * Result object
 * @typedef {Object} Result
 * @property {boolean} success If the operation was a success
 * @property {string | null} error A error message
 */

/**
 * Electron preload.js api we expose from the main process in preload.js
 * @typedef {Object} ElectronAPI
 * @property {function(): Promise<string[]>} works - Resolves with an array of strings.
 * @property {function(): Promise<import('electron').OpenDialogReturnValue>} openDirectory - Opens a directory dialog and resolves with the selected value.
 * @property {function(string, ReadFileOptions): Promise<ReadFileInfo[]>} readFiles - Reads files from a directory with options.
 * @property {function(string): Promise<string>} readFile - Reads the contents of a file as a string.
 * @property {function(string, string): Promise<{success: boolean, error?: string}>} overWriteFile - Overwrites a file with new content.
 */

/**
 * Electron API exposed on the window object
 * @typedef {Object} EWindow
 * @property {ElectronAPI} electronAPI - Exposes the electron API
 */
