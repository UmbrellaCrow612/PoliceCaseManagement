const fs = require("fs");
const path = require("path");
const { execSync } = require("child_process");
const unzipper = require("unzipper");
const { pipeline } = require("stream/promises");
const { Readable } = require("stream");
const { fetch } = require("undici");
const asar = require("asar"); 

const rootPath = path.join(__dirname, "..");
const distPath = path.join(rootPath, "dist");
const appPath = path.join(distPath, "resources", "app");
const angularProjectPath = path.join(rootPath, "render");
const angularProjectDistPath = path.join(angularProjectPath, "dist", "browser");
const packageName = "electron-v38.1.2-win32-x64.zip";
const electronPackageReleaseVersion = "v38.1.2";
const fetchElectronZipUrl = `https://github.com/electron/electron/releases/download/${electronPackageReleaseVersion}/${packageName}`;
const rootFilesToCopy = [".env", "package.json"];

/**
 * Helper to print errors to console
 * @param {string} str Line to print
 */
function logErr(str) {
  console.log("Error: " + str);
}

/**
 * Helper to log information
 * @param {string} str Line to print
 */
function log(str) {
  console.log("Information: " + str);
}

/**
 * Helper to stop running
 */
function exit() {
  process.exit();
}

async function downloadAndExtract() {
  const zipDownloadPath = path.join(rootPath, packageName);

  log("Fetching electron binaries...");
  const response = await fetch(fetchElectronZipUrl);
  if (!response.ok) {
    logErr("Failed to fetch electron binaries at " + fetchElectronZipUrl);
    exit();
  }
  log("Fetched electron binaries, saving to " + zipDownloadPath);

  // Save zip file temporarily
  const fileStream = fs.createWriteStream(zipDownloadPath);
  await pipeline(Readable.fromWeb(response.body), fileStream);

  log("Extracting electron binaries directly into " + distPath);
  await fs
    .createReadStream(zipDownloadPath)
    .pipe(unzipper.Extract({ path: distPath }))
    .promise();

  log("Cleaning up zip file...");
  fs.unlinkSync(zipDownloadPath);
}

if (!fs.existsSync(angularProjectPath)) {
  logErr("Angular project not found");
  exit();
}

if (fs.existsSync(distPath)) {
  log("App build removing it " + distPath);
  fs.rmSync(distPath, { recursive: true });
}

(async () => {
  await downloadAndExtract();

  log("Building angular project " + angularProjectPath);
  execSync("npm ci", { cwd: angularProjectPath, stdio: "inherit" });
  execSync("npm run build", { cwd: angularProjectPath, stdio: "inherit" });

  log("Copying angular built files");
  if (!fs.existsSync(angularProjectDistPath)) {
    logErr("Angular build folder dose not exist");
    exit();
  }
  fs.cpSync(angularProjectDistPath, appPath, { recursive: true });

  const electronFolderPath = path.join(rootPath, "electron");
  if (!fs.existsSync(electronFolderPath)) {
    logErr("Electron folder not found at " + electronFolderPath);
    exit();
  }
  fs.cpSync(electronFolderPath, appPath, { recursive: true });
  log("Copied electron folder into " + appPath);

  rootFilesToCopy.forEach((fileName) => {
    const srcPath = path.join(rootPath, fileName);
    const destPath = path.join(appPath, fileName);

    if (fs.existsSync(srcPath)) {
      fs.copyFileSync(srcPath, destPath);
      log(`Copied ${fileName} into ${appPath}`);
    } else {
      logErr(`File ${fileName} not found at ${appPath}`);
      exit();
    }
  });

   const asarPath = path.join(distPath, "resources", "app.asar");
   log("Packaging app into asar...");
   await asar.createPackage(appPath, asarPath);
 
   log("Removing unpackaged app folder...");
   fs.rmSync(appPath, { recursive: true });
 
   log("Build completed with asar packaging");
   exit();
})();
