import { inject, Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

/**
 * A pipe that sanitizes a URL for safe use in resource URL bindings (e.g., in iframes).
 *
 * This is useful when embedding URLs that Angular's built-in security would otherwise block.
 * It uses Angular's DomSanitizer to bypass security restrictions safely.
 *
 * @example
 * <iframe [src]="url | safePipe"></iframe>
 */
@Pipe({
  name: 'safePipe',
})
export class SafePipe implements PipeTransform {
  /**
   * Angular's DomSanitizer service for bypassing security trust on resource URLs.
   * Injected using Angular's `inject()` function.
   */
  private sanitizer = inject(DomSanitizer);

  /**
   * Transforms a string URL into a trusted SafeResourceUrl.
   *
   * @param value - The URL string to sanitize.
   * @returns A SafeResourceUrl that can be used in the DOM.
   */
  transform(value: string): SafeResourceUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl(value);
  }
}
