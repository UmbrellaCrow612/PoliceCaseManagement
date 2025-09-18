"use strict";

/** @type {HTMLButtonElement | null} */
const loadBtn = /** @type {HTMLButtonElement | null} */ (
  document.getElementById("load_solution_folder_btn")
);
if (!loadBtn) throw new Error("Load button not found");

/** @type {HTMLElement | null} */
const continueCancelParent = document.getElementById(
  "continue_and_cancel_container"
);

removeContinueCancelContainer();

const loadBtnOrgContent = loadBtn.textContent;

loadBtn.addEventListener("click", () => {
  loadBtn.disabled = true;
  loadBtn.textContent = "Loading...";

  // open dialog
  // select folder
  // display file path
  // display buttons
});

function renderContinueCancelContainer() {
  if (!continueCancelParent)
    throw new Error("Continue/Cancel container not found");
}

function removeContinueCancelContainer() {
  if (!continueCancelParent)
    throw new Error("Continue/Cancel container not found");

  // Remove container until file is selected
  continueCancelParent.remove();
}
