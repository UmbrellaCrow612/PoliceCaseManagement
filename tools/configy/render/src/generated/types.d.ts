/**
 * Information of a file that way read
 */
type ReadFileInfo = {
    /**
     * - The name of the file including extension.
     */
    fileName: string;
    /**
     * - The full path to the file.
     */
    filePath: string;
    /**
     * - The directory where the file is located.
     */
    fileDirectory: string;
};
/**
 * List of optiosn you can pass when reading files from a directory
 */
type ReadFileOptions = {
    /**
     * - List of file names to look for
     */
    fileNames: Set<string>;
    /**
     * - List of file extensions you want to look for
     */
    fileExts: Set<string>;
    /**
     * - List of file names to ignore
     */
    ignoreFileNames: Set<string>;
    /**
     * - List of folders to ignore
     */
    ignoreDirs: Set<string>;
};
/**
 * Result object
 */
type Result = {
    /**
     * If the operation was a success
     */
    success: boolean;
    /**
     * A error message
     */
    error: string | null;
};
/**
 * Electron preload.js api we expose from the main process in preload.js
 */
type ElectronAPI = {
    /**
     * - Resolves with an array of strings.
     */
    works: () => Promise<string[]>;
    /**
     * - Opens a directory dialog and resolves with the selected value.
     */
    openDirectory: () => Promise<import("electron").OpenDialogReturnValue>;
    /**
     * - Reads files from a directory with options.
     */
    readFiles: (arg0: string, arg1: ReadFileOptions) => Promise<ReadFileInfo[]>;
    /**
     * - Reads the contents of a file as a string.
     */
    readFile: (arg0: string) => Promise<string>;
    /**
     * - Overwrites a file with new content.
     */
    overWriteFile: (arg0: string, arg1: string) => Promise<{
        success: boolean;
        error?: string;
    }>;
};
/**
 * Electron API exposed on the window object
 */
type EWindow = {
    /**
     * - Exposes the electron API
     */
    electronAPI: ElectronAPI;
};
