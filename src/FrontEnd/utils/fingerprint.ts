export function GetFingerPrint() {
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
  } else {
    gPUVendor = 'Unknown';
    gPURenderer = 'Unknown';
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
  ];

  var joinedComponents = components.join('-');

  return joinedComponents;
}
