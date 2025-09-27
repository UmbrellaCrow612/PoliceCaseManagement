/**
 * Get the electron api and use it
 * @returns Electron api
 */
export function getApi(): ElectronAPI {
  return (window as unknown as EWindow).electronAPI;
}
