"use strict";

/**
 * Is the base window but we have todo this to acess the string extra api using ipc process from preload js
 * @type {any}
 */
const _window = window;
if (!_window.electronAPI.works()) {
  throw new Error("electronAPI dosent work from preload");
}

/**
 * @type {string | null}
 */
var folderPathSelected = null;

const selectedFolderContainer = document.getElementById("selected_folder_path");
if (!selectedFolderContainer)
  throw new Error("Could not find selected_folder_path");

/** @type {HTMLButtonElement | null} */
const loadBtn = /** @type {HTMLButtonElement | null} */ (
  document.getElementById("load_solution_folder_btn")
);
if (!loadBtn) throw new Error("Load button not found");

const continueCancelParent = document.getElementById(
  "continue_and_cancel_container"
);

/** @type {HTMLButtonElement | null} */
const continueButton = /** @type {HTMLButtonElement | null} */ (
  document.getElementById("continue_button")
);
/** @type {HTMLButtonElement | null} */
const cancelButton = /** @type {HTMLButtonElement | null} */ (
  document.getElementById("cancel_button")
);

removeContinueCancelContainer();
removeSelectedFolderPathContainer();

const loadBtnOrgContent = loadBtn.textContent;

loadBtn.addEventListener("click", async () => {
  loadBtn.disabled = true;
  loadBtn.textContent = "Loading...";

  /** @type {Electron.OpenDialogReturnValue} */
  const resValue = await _window.electronAPI.OpenFolderDirectory();
  if (resValue.canceled) {
    loadBtn.disabled = false;
    loadBtn.textContent = loadBtnOrgContent;
    return;
  }

  const folderPath = resValue.filePaths[0];
  folderPathSelected = folderPath;
  console.log(folderPath);

  // Render selected folder path
  renderSelectedFolderPath(folderPath);

  // Render Continue/Cancel buttons
  renderContinueCancelContainer();

  // Reset button text
  loadBtn.disabled = false;
  loadBtn.textContent = loadBtnOrgContent;
});

// Add listeners for Continue and Cancel
continueButton?.addEventListener("click", async () => {
  console.log("Continue clicked! Folder is selected.");
  if (!folderPathSelected) {
    throw new Error("folderPathSelected is null ");
  }

  /**
   * @type {Array<{filePath: string; fileName: string; fileContent: string;}>}
   */
  var files = await _window.electronAPI.ReadAllJsonFilesFromDir(
    folderPathSelected
  );
  console.log(files);
});

cancelButton?.addEventListener("click", () => {
  removeSelectedFolderPathContainer();
  removeContinueCancelContainer();
});

function renderContinueCancelContainer() {
  if (!continueCancelParent)
    throw new Error("Continue/Cancel container not found");

  // Append container back to the body if it's not already there
  if (!document.body.contains(continueCancelParent)) {
    document.body.appendChild(continueCancelParent);
  }
}

function removeContinueCancelContainer() {
  if (!continueCancelParent)
    throw new Error("Continue/Cancel container not found");

  // Remove container until file is selected
  continueCancelParent.remove();
}

/**
 *
 * @param {string} path
 */
function renderSelectedFolderPath(path) {
  if (!selectedFolderContainer)
    throw new Error("selectedFolderContainer missing from DOM");

  selectedFolderContainer.textContent = `Selected folder: ${path}`;

  // Append container back to the body if it's not already there
  if (!document.body.contains(selectedFolderContainer)) {
    document.body.insertBefore(selectedFolderContainer, loadBtn.nextSibling);
  }
}

function removeSelectedFolderPathContainer() {
  if (!selectedFolderContainer)
    throw new Error("selectedFolderContainer missing from DOM");

  selectedFolderContainer.remove();
}
