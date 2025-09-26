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
