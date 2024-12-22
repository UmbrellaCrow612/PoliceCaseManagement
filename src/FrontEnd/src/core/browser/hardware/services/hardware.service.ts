import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class HardwareService {
  /**
   * Returns basic hardware-related information.
   * Assumes user permissions are already granted where needed.
   */
  getHardwareInfo(): { memory: number | undefined; cores: number | undefined } {
    const memory = (navigator as any).deviceMemory || undefined; // Memory in GB (if supported)
    const cores = navigator.hardwareConcurrency || undefined; // Number of logical cores

    return { memory, cores };
  }
}
