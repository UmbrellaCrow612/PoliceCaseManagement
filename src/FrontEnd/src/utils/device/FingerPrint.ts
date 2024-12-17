/**
 * Generates a unique device fingerprint based on various browser and device characteristics.
 * @returns {string} A string representing the device's unique fingerprint.
 * @remarks
 * This function collects multiple device and browser attributes to create a distinctive identifier.
 * It works across different browsers, providing fallback values when information is unavailable.
 */
export function GetFingerPrint(): string {
    const nav: any = navigator;
  
    const ram = nav.deviceMemory;
  
    const cpuCores = navigator.hardwareConcurrency || 'Unknown';
  
    var gPUVendor = 'Unknown';
    var gPURenderer = 'Unknown';
  
    const canvas = document.createElement('canvas');
  
    const gl: any =
      canvas.getContext('webgl') || canvas.getContext('experimental-webgl');
  
    if (gl) {
      const debugInfo = gl.getExtension('WEBGL_debug_renderer_info');
      
      const gpuVendor = gl.getParameter(debugInfo.UNMASKED_VENDOR_WEBGL);
      const gpuRenderer = gl.getParameter(debugInfo.UNMASKED_RENDERER_WEBGL);
      
      gPUVendor = gpuVendor;
      gPURenderer = gpuRenderer;
    }
  
    const components = [
      navigator.webdriver,           
      navigator.pdfViewerEnabled,  
      window.screen.pixelDepth,      
      window.screen.colorDepth,     
      ram,                           
      cpuCores,                      
      navigator.platform,            
      gPUVendor,                    
      gPURenderer,                   
      navigator.doNotTrack,         
      nav.connection.effectiveType || 'Unknown', 
      Intl.DateTimeFormat().resolvedOptions().timeZone, 
    ];
  
    var joinedComponents = components.join('-');
  
    return joinedComponents;
  }