import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ScreenService {
  /**
   * Returns comprehensive information about the window and screen properties
   * @returns {Object} An object containing various screen and window measurements and properties
   * 
   * @property {number} innerWidth - The inner width of the browser window in pixels
   * @property {number} innerHeight - The inner height of the browser window in pixels
   * @property {number} outerWidth - The outer width of the browser window in pixels
   * @property {number} outerHeight - The outer height of the browser window in pixels
   * @property {number} screenWidth - The width of the user's screen in pixels
   * @property {number} screenHeight - The height of the user's screen in pixels
   * @property {number} availScreenWidth - The available width of the user's screen in pixels
   * @property {number} availScreenHeight - The available height of the user's screen in pixels
   * @property {number} screenLeft - The distance from the left edge of the screen to the browser window
   * @property {number} screenTop - The distance from the top edge of the screen to the browser window
   * @property {number} scrollX - The number of pixels scrolled horizontally
   * @property {number} scrollY - The number of pixels scrolled vertically
   * @property {number} devicePixelRatio - The ratio of physical pixels to logical pixels
   * @property {string} orientation - The current screen orientation
   * @property {boolean} isFullscreen - Whether the browser is in fullscreen mode
   * @property {boolean} isMaximized - Whether the browser window is maximized
   */
  getScreenInfo() {
    return {
      // Window dimensions
      innerWidth: window.innerWidth,
      innerHeight: window.innerHeight,
      outerWidth: window.outerWidth,
      outerHeight: window.outerHeight,

      // Screen dimensions
      screenWidth: window.screen.width,
      screenHeight: window.screen.height,
      availScreenWidth: window.screen.availWidth,
      availScreenHeight: window.screen.availHeight,

      // Window position
      screenLeft: window.screenLeft,
      screenTop: window.screenTop,

      // Scroll position
      scrollX: window.scrollX,
      scrollY: window.scrollY,

      // Display properties
      devicePixelRatio: window.devicePixelRatio,
      orientation: screen.orientation.type,

      // Window state
      isFullscreen: document.fullscreenElement !== null,
      isMaximized: window.outerWidth === window.screen.availWidth && 
                   window.outerHeight === window.screen.availHeight
    };
  }
}