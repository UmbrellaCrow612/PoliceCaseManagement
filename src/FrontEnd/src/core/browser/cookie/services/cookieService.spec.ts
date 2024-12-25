import { CookieOptions } from '../types';
import { CookieService } from './cookie.service';
import { TestBed } from '@angular/core/testing';

describe('CookieService', () => {
  let service: CookieService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CookieService],
    });

    service = TestBed.inject(CookieService);
  });

  describe('default options', () => {
    it('should have correct initial default options', () => {
      expect(service['defaultOptions']).toEqual({
        secure: true,
        sameSite: 'Lax',
        path: '/',
        priority: 'Medium',
        persistence: 'permanent',
        validation: {
          maxLength: 4096,
          minLength: 1,
          validator: jasmine.any(Function),
        },
        crossOrigin: {
          policy: 'same-origin',
        },
      });

      const defaultValidator = service['defaultOptions'].validation?.validator;
      if (defaultValidator) {
        expect(defaultValidator('')).toBeFalse(); // too short
      }
      if (defaultValidator) {
        expect(defaultValidator('a')).toBeTrue(); // valid
      }
      if (defaultValidator) {
        expect(defaultValidator('a'.repeat(4097))).toBeFalse(); // too long
      }
    });
  });

  describe('setDefaultOptions', () => {
    it('should throw error for invalid options', () => {
      expect(() =>
        service.setDefaultOptions({ secure: 'invalid' as any })
      ).toThrowError('Invalid cookie options provided');
    });

    it('should throw error when secure is false with sameSite None', () => {
      expect(() =>
        service.setDefaultOptions({
          secure: false,
          sameSite: 'None',
        })
      ).toThrowError("secure must be true when sameSite is 'None'");

      // Also test when existing default sameSite is 'None'
      service['defaultOptions'].sameSite = 'None';
      expect(() =>
        service.setDefaultOptions({
          secure: false,
        })
      ).toThrowError("secure must be true when sameSite is 'None'");
    });

    it('should merge options correctly with defaults', () => {
      const newOptions: Partial<CookieOptions> = {
        path: '/new',
        priority: 'High',
        validation: {
          maxLength: 1000,
        },
        crossOrigin: {
          allowedOrigins: ['https://example.com'],
        },
      };

      service.setDefaultOptions(newOptions);

      expect(service['defaultOptions']).toEqual({
        secure: true, // preserved from defaults
        sameSite: 'Lax', // preserved from defaults
        path: '/new', // overridden
        priority: 'High', // overridden
        persistence: 'permanent', // preserved from defaults
        validation: {
          maxLength: 1000, // overridden
          minLength: 1, // preserved from defaults
          validator: jasmine.any(Function), // preserved from defaults
        },
        crossOrigin: {
          policy: 'same-origin', // preserved from defaults
          allowedOrigins: ['https://example.com'], // added
        },
      });
    });

    it('should preserve existing validation function when not provided', () => {
      const originalValidator = service['defaultOptions'].validation?.validator;

      service.setDefaultOptions({
        validation: {
          maxLength: 2000,
        },
      });

      expect(service['defaultOptions'].validation?.validator).toBe(
        originalValidator
      );
    });

    it('should allow updating validation function', () => {
      const newValidator = (value: string) => value.startsWith('test');

      service.setDefaultOptions({
        validation: {
          validator: newValidator,
        },
      });

      const validator = service['defaultOptions'].validation?.validator;
      if (validator) {
        expect(validator('test123')).toBeTrue();
        expect(validator('invalid')).toBeFalse();
      }
    });

    it('should preserve partial crossOrigin settings', () => {
      // First set some crossOrigin options
      service.setDefaultOptions({
        crossOrigin: {
          allowedOrigins: ['https://example.com'],
          policy: 'same-site',
        },
      });

      // Then update only the policy
      service.setDefaultOptions({
        crossOrigin: {
          policy: 'same-origin',
        },
      });

      expect(service['defaultOptions'].crossOrigin).toEqual({
        allowedOrigins: ['https://example.com'],
        policy: 'same-origin',
      });
    });

    it('should handle undefined optional properties', () => {
      const newOptions: Partial<CookieOptions> = {
        path: '/new',
        validation: undefined,
        crossOrigin: undefined,
      };

      service.setDefaultOptions(newOptions);

      // Should keep existing validation and crossOrigin settings
      expect(service['defaultOptions'].validation).toEqual({
        maxLength: 4096,
        minLength: 1,
        validator: jasmine.any(Function),
      });
      expect(service['defaultOptions'].crossOrigin).toEqual({
        policy: 'same-origin',
      });
    });

    it('should maintain type safety for sameSite options', () => {
      service.setDefaultOptions({ sameSite: 'Strict' });
      expect(service['defaultOptions'].sameSite).toBe('Strict');

      service.setDefaultOptions({ sameSite: 'Lax' });
      expect(service['defaultOptions'].sameSite).toBe('Lax');

      service.setDefaultOptions({ sameSite: 'None', secure: true });
      expect(service['defaultOptions'].sameSite).toBe('None');

      expect(() =>
        service.setDefaultOptions({
          sameSite: 'Invalid' as any,
        })
      ).toThrowError();
    });
  });

  describe('validateSameSite', () => {
    it('should return undefined for undefined input', () => {
      const result = service['validateSameSite'](undefined);
      expect(result).toBeUndefined();
    });

    it('should return valid sameSite values unchanged', () => {
      expect(service['validateSameSite']('Strict')).toBe('Strict');
      expect(service['validateSameSite']('Lax')).toBe('Lax');
      expect(service['validateSameSite']('None')).toBe('None');
    });

    it('should throw error for invalid sameSite values', () => {
      expect(() => service['validateSameSite']('Invalid' as any)).toThrowError(
        "sameSite must be 'Strict', 'Lax', or 'None'"
      );

      expect(() => service['validateSameSite']('strict' as any)).toThrowError(
        "sameSite must be 'Strict', 'Lax', or 'None'"
      );

      expect(() => service['validateSameSite']('LAX' as any)).toThrowError(
        "sameSite must be 'Strict', 'Lax', or 'None'"
      );
    });

    it('should be case sensitive in validation', () => {
      // These should throw as the casing is incorrect
      expect(() => service['validateSameSite']('STRICT' as any)).toThrowError(
        "sameSite must be 'Strict', 'Lax', or 'None'"
      );

      expect(() => service['validateSameSite']('lax' as any)).toThrowError(
        "sameSite must be 'Strict', 'Lax', or 'None'"
      );
    });

    it('should handle non-string inputs', () => {
      expect(() => service['validateSameSite'](123 as any)).toThrowError(
        "sameSite must be 'Strict', 'Lax', or 'None'"
      );

      expect(() => service['validateSameSite'](true as any)).toThrowError(
        "sameSite must be 'Strict', 'Lax', or 'None'"
      );

      expect(() => service['validateSameSite']({} as any)).toThrowError(
        "sameSite must be 'Strict', 'Lax', or 'None'"
      );
    });

    it('should work correctly in context of setDefaultOptions', () => {
      // Test integration with setDefaultOptions
      expect(() =>
        service.setDefaultOptions({ sameSite: 'Strict' })
      ).not.toThrow();

      expect(() =>
        service.setDefaultOptions({ sameSite: 'Invalid' as any })
      ).toThrowError();

      // Verify the value was actually set
      expect(service['defaultOptions'].sameSite).toBe('Strict');
    });
  });

  describe('validateOptions', () => {
    it('should not throw for valid options', () => {
      const validOptions: CookieOptions = {
        path: '/test',
        domain: 'localhost',
        secure: true,
        sameSite: 'Lax',
      };

      expect(() => service['validateOptions'](validOptions)).not.toThrow();
    });

    describe('expires and maxAge validation', () => {
      it('should throw when both expires and maxAge are set', () => {
        const options: CookieOptions = {
          expires: new Date(),
          maxAge: 3600,
        };

        expect(() => service['validateOptions'](options)).toThrowError(
          'Cannot set both expires and maxAge'
        );
      });

      it('should allow expires without maxAge', () => {
        const options: CookieOptions = {
          expires: new Date(),
        };

        expect(() => service['validateOptions'](options)).not.toThrow();
      });

      it('should allow maxAge without expires', () => {
        const options: CookieOptions = {
          maxAge: 3600,
        };

        expect(() => service['validateOptions'](options)).not.toThrow();
      });
    });

    describe('maxAge validation', () => {
      it('should throw for negative maxAge', () => {
        const options: CookieOptions = {
          maxAge: -1,
        };

        expect(() => service['validateOptions'](options)).toThrowError(
          'maxAge must be positive'
        );
      });

      it('should throw for zero maxAge', () => {
        const options: CookieOptions = {
          maxAge: 0,
        };

        expect(() => service['validateOptions'](options)).toThrowError(
          'maxAge must be positive'
        );
      });

      it('should allow positive maxAge', () => {
        const options: CookieOptions = {
          maxAge: 1,
        };

        expect(() => service['validateOptions'](options)).not.toThrow();
      });
    });

    describe('domain validation', () => {
      it('should throw for invalid domains', () => {
        const invalidDomains = [
          'invalid domain',
          'invalid@domain',
          '-invalid.com',
          'invalid-.com',
          'inva..lid.com',
        ];

        invalidDomains.forEach((domain) => {
          const options: CookieOptions = { domain };
          expect(() => service['validateOptions'](options)).toThrowError(
            'Invalid domain provided'
          );
        });
      });

      it('should allow valid domains', () => {
        const validDomains = ['localhost'];

        validDomains.forEach((domain) => {
          const options: CookieOptions = { domain };
          expect(() => service['validateOptions'](options)).not.toThrow();
        });
      });

      it('should allow options without domain', () => {
        const options: CookieOptions = {
          path: '/test',
        };

        expect(() => service['validateOptions'](options)).not.toThrow();
      });
    });

    describe('path validation', () => {
      it('should throw for paths not starting with /', () => {
        const invalidPaths = ['test', 'test/path', 'path/', '../path', './'];

        invalidPaths.forEach((path) => {
          const options: CookieOptions = { path };
          expect(() => service['validateOptions'](options)).toThrowError(
            'Path must start with /'
          );
        });
      });

      it('should allow valid paths', () => {
        const validPaths = [
          '/',
          '/test',
          '/test/path',
          '/test/path/',
          '/test-path',
          '/test_path',
        ];

        validPaths.forEach((path) => {
          const options: CookieOptions = { path };
          expect(() => service['validateOptions'](options)).not.toThrow();
        });
      });

      it('should allow options without path', () => {
        const options: CookieOptions = {
          domain: 'localhost',
        };

        expect(() => service['validateOptions'](options)).not.toThrow();
      });
    });

    describe('combined validations', () => {
      it('should validate multiple properties together', () => {
        const options: CookieOptions = {
          path: '/test',
          domain: 'localhost',
          maxAge: 3600,
          secure: true,
          sameSite: 'Lax',
        };

        expect(() => service['validateOptions'](options)).not.toThrow();
      });

      it('should throw on first validation failure in combined options', () => {
        const options: CookieOptions = {
          path: 'invalid',
          domain: 'invalid domain',
          maxAge: -1,
          expires: new Date(), // Multiple invalid properties
        };

        // Should throw the first error it encounters
        expect(() => service['validateOptions'](options)).toThrowError(
          'Cannot set both expires and maxAge'
        );
      });
    });
  });

  describe('Testing getCookie function', () => {
    it('should return the value of a valid cookie', () => {
      document.cookie =
        encodeURIComponent('testCookie') +
        '=' +
        encodeURIComponent('testValue');

      const result = service.getCookie('testCookie');
      expect(result).toBe('testValue');
    });

    it('should return null if the cookie does not exist', () => {
      const result = service.getCookie('nonExistentCookie');
      expect(result).toBeNull();
    });

    it('should throw an error if the name is an empty string', () => {
      expect(() => service.getCookie('')).toThrowError(
        'Cookie name must be a non-empty string'
      );
    });

    it('should handle multiple cookies and return the correct one', () => {
      document.cookie =
        encodeURIComponent('cookie1') + '=' + encodeURIComponent('value1');
      document.cookie =
        encodeURIComponent('cookie2') + '=' + encodeURIComponent('value2');

      const result = service.getCookie('cookie2');
      expect(result).toBe('value2');
    });
  });
});
