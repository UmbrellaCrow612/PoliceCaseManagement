const path = require("path");
const fs = require("node:fs");

/**
 * Loads .env into node process
 */
function loadEnv() {
  const envPath = path.resolve(__dirname, ".env");
  if (!fs.existsSync(envPath)) {
    throw new Error("Could not find .env file to load");
  }

  const envFile = fs.readFileSync(envPath, "utf-8");
  envFile.split("\n").forEach((line) => {
    const [key, value] = line.split("=");
    if (key && value) {
      process.env[key.trim()] = value.trim();
    }
  });
}

/**
 * Check if the node process has a certain .env key
 * @param {string} key The key to check if it exists
 * @returns {boolean} True or false
 */
function hasEnv(key) {
  return Object.prototype.hasOwnProperty.call(process.env, key);
}

module.exports = { loadEnv, hasEnv };
