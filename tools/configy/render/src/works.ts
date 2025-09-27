/**
 * Verify that electronAPI exists and all functions are callable
 * @returns {Promise<boolean>} true if all functions exist and are callable
 */
export async function checkApi(): Promise<boolean> {
  const api: any = (window as unknown as EWindow).electronAPI;

  if (!api) {
    console.error('window.electronAPI is not defined!');
    return false;
  }

  try {
    const availableFunctions: string[] = await api.works();
    const missingFunctions = availableFunctions.filter(
      (fn) => typeof api[fn] !== 'function'
    );

    if (missingFunctions.length > 0) {
      console.error('Missing functions on electronAPI:', missingFunctions);
      return false;
    }

    console.log('All functions are correctly attached:', availableFunctions);
    return true;
  } catch (err) {
    console.error('Error checking electron API:', err);
    return false;
  }
}
