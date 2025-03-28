/**
 * Generates a unique device fingerprint based on various browser and device characteristics.
 * This function attempts to gather various pieces of information and provides fallbacks
 * if an API is unavailable or throws an error, ensuring it doesn't crash the script.
 *
 * @returns {string} A string representing the device's unique fingerprint, using 'Unknown' for unavailable data points.
 * @remarks
 * This function collects multiple device and browser attributes to create a distinctive identifier.
 * It works across different browsers, providing fallback values when information is unavailable or errors occur.
 * All operations are synchronous.
 */
export function ComputeFingerPrint(): string {
  // Helper function to safely get properties, returning 'Unknown' on error or absence
  const safeGet = <T>(
    getter: () => T,
    defaultValue: string = 'Unknown'
  ): T | string => {
    try {
      const value = getter();
      // Handle null, undefined explicitly, although ?? usually covers undefined/null
      return value !== null && value !== undefined ? value : defaultValue;
    } catch (error) {
      // Optional: Log the error for debugging purposes if needed
      // console.warn(`Fingerprint: Could not retrieve value. Error: ${error}`);
      return defaultValue;
    }
  };

  // --- Gather Components Safely ---

  // Screen properties
  const screenPixelDepth = safeGet(() => window?.screen?.pixelDepth);
  const screenColorDepth = safeGet(() => window?.screen?.colorDepth);

  // Navigator properties
  // Note: deviceMemory is non-standard and might not exist. Casting to 'any' to access.
  const ram = safeGet(() => (navigator as any)?.deviceMemory);
  const cpuCores = safeGet(() => navigator?.hardwareConcurrency);
  const platform = safeGet(() => navigator?.platform);

  // Timezone
  const timeZone = safeGet(
    () => Intl?.DateTimeFormat()?.resolvedOptions()?.timeZone
  );

  // WebGL properties (GPU Info) - Requires more complex handling
  let gpuVendor: string = 'Unknown';
  let gpuRenderer: string = 'Unknown';

  try {
    // Check if running in a browser environment with document support
    if (typeof document !== 'undefined' && document.createElement) {
      const canvas = document.createElement('canvas');
      if (canvas.getContext) {
        const gl: any =
          canvas.getContext('webgl') || canvas.getContext('experimental-webgl');

        if (gl) {
          const debugInfo = gl.getExtension('WEBGL_debug_renderer_info');
          if (debugInfo) {
            // Use safeGet within the WebGL block as well
            gpuVendor = safeGet(
              () => gl.getParameter(debugInfo.UNMASKED_VENDOR_WEBGL),
              'Unknown'
            );
            gpuRenderer = safeGet(
              () => gl.getParameter(debugInfo.UNMASKED_RENDERER_WEBGL),
              'Unknown'
            );
          } else {
            console.warn(
              'Fingerprint: WEBGL_debug_renderer_info extension not available.'
            );
          }
        } else {
          console.warn('Fingerprint: WebGL context not available.');
        }
      } else {
        console.warn('Fingerprint: canvas.getContext not available.');
      }
    } else {
      console.warn(
        'Fingerprint: document context not available for canvas creation.'
      );
    }
  } catch (error) {}

  // --- Assemble Fingerprint ---

  const components = [
    screenPixelDepth,
    screenColorDepth,
    ram,
    cpuCores,
    platform,
    gpuVendor,
    gpuRenderer,
    timeZone,
  ];

  // Join components, ensuring everything is treated as a string.
  // Using String() handles potential numbers or other types safely.
  const joinedComponents = components.map(String).join('-');

  return joinedComponents;
}
