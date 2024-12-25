import { CookieOptions } from './types';
import { isCookieOptions } from './validator';

describe('isCookieOptions', () => {
  describe('Basic type checking', () => {
    it('should return false for null', () => {
      expect(isCookieOptions(null)).toBeFalse();
    });

    it('should return false for undefined', () => {
      expect(isCookieOptions(undefined)).toBeFalse();
    });

    it('should return false for non-objects', () => {
      expect(isCookieOptions('string')).toBeFalse();
      expect(isCookieOptions(123)).toBeFalse();
      expect(isCookieOptions(true)).toBeFalse();
    });

    it('should return true for empty object', () => {
      expect(isCookieOptions({})).toBeTrue();
    });
  });

  describe('Core options validation', () => {
    it('validates expires property', () => {
      const validDate = new Date();
      const validNumber = 7;

      expect(isCookieOptions({ expires: validDate })).toBeTrue();
      expect(isCookieOptions({ expires: validNumber })).toBeTrue();
      expect(isCookieOptions({ expires: 'invalid' })).toBeFalse();
      expect(isCookieOptions({ expires: {} })).toBeFalse();
    });

    it('validates maxAge property', () => {
      expect(isCookieOptions({ maxAge: 3600 })).toBeTrue();
      expect(isCookieOptions({ maxAge: 0 })).toBeTrue();
      expect(isCookieOptions({ maxAge: -1 })).toBeFalse();
      expect(isCookieOptions({ maxAge: 2147483648 })).toBeFalse(); // Over MAX_AGE
      expect(isCookieOptions({ maxAge: 'invalid' as any })).toBeFalse();
    });

    it('validates path property', () => {
      expect(isCookieOptions({ path: '/' })).toBeTrue();
      expect(isCookieOptions({ path: '/path' })).toBeTrue();
      expect(isCookieOptions({ path: 123 as any })).toBeFalse();
    });

    it('validates domain property', () => {
      expect(isCookieOptions({ domain: 'example.com' })).toBeTrue();
      expect(isCookieOptions({ domain: '.example.com' })).toBeTrue();
      expect(isCookieOptions({ domain: 123 as any })).toBeFalse();
    });

    it('validates secure property', () => {
      expect(isCookieOptions({ secure: true })).toBeTrue();
      expect(isCookieOptions({ secure: false })).toBeTrue();
      expect(isCookieOptions({ secure: 'true' as any })).toBeFalse();
    });

    it('validates httpOnly property', () => {
      expect(isCookieOptions({ httpOnly: true })).toBeTrue();
      expect(isCookieOptions({ httpOnly: false })).toBeTrue();
      expect(isCookieOptions({ httpOnly: 'true' as any })).toBeFalse();
    });
  });

  describe('SameSite validation', () => {
    it('validates sameSite property', () => {
      expect(isCookieOptions({ sameSite: 'Strict' })).toBeTrue();
      expect(isCookieOptions({ sameSite: 'Lax' })).toBeTrue();
      expect(isCookieOptions({ sameSite: 'None' })).toBeTrue();
      expect(isCookieOptions({ sameSite: 'Invalid' as any })).toBeFalse();
    });
  });

  describe('Priority validation', () => {
    it('validates priority property', () => {
      expect(isCookieOptions({ priority: 'Low' })).toBeTrue();
      expect(isCookieOptions({ priority: 'Medium' })).toBeTrue();
      expect(isCookieOptions({ priority: 'High' })).toBeTrue();
      expect(isCookieOptions({ priority: 'Invalid' as any })).toBeFalse();
    });
  });

  describe('Partitioned cookie validation', () => {
    it('validates partitioned property', () => {
      expect(isCookieOptions({ partitioned: true })).toBeTrue();
      expect(isCookieOptions({ partitioned: false })).toBeTrue();
      expect(isCookieOptions({ partitioned: 'true' as any })).toBeFalse();
    });
  });

  describe('Size constraints validation', () => {
    it('validates size object', () => {
      const validSize = {
        maxNameLength: 255,
        maxValueLength: 4096,
        maxTotalSize: 4096,
      };
      expect(isCookieOptions({ size: validSize })).toBeTrue();
      expect(
        isCookieOptions({
          size: { ...validSize, maxNameLength: 'invalid' } as any,
        })
      ).toBeFalse();
      expect(isCookieOptions({ size: { maxNameLength: 255 } })).toBeFalse(); // missing required fields
    });
  });

  describe('Persistence validation', () => {
    it('validates persistence property', () => {
      expect(isCookieOptions({ persistence: 'session' })).toBeTrue();
      expect(isCookieOptions({ persistence: 'permanent' })).toBeTrue();
      expect(isCookieOptions({ persistence: 'invalid' as any })).toBeFalse();
    });
  });

  describe('Cross-origin settings validation', () => {
    it('validates crossOrigin object', () => {
      expect(
        isCookieOptions({
          crossOrigin: {
            allowedOrigins: ['https://example.com'],
            policy: 'same-origin',
          },
        })
      ).toBeTrue();

      expect(
        isCookieOptions({
          crossOrigin: {
            allowedOrigins: ['https://example.com', 123 as any],
            policy: 'same-origin',
          },
        })
      ).toBeFalse();

      expect(
        isCookieOptions({
          crossOrigin: {
            policy: 'invalid-policy' as any,
          },
        })
      ).toBeFalse();
    });
  });

  describe('Validation rules checking', () => {
    it('validates validation object', () => {
      const validValidation = {
        minLength: 1,
        maxLength: 100,
        pattern: /^[a-z]+$/,
        validator: (value: string) => value.length > 0,
      };

      expect(isCookieOptions({ validation: validValidation })).toBeTrue();
      expect(isCookieOptions({ validation: { minLength: -1 } })).toBeFalse();
      expect(isCookieOptions({ validation: { minLength: "" } })).toBeFalse();
      expect(
        isCookieOptions({ validation: { pattern: 'invalid' as any } })
      ).toBeFalse();
      expect(
        isCookieOptions({ validation: { validator: 'not-a-function' as any } })
      ).toBeFalse();
    });
  });

  describe('Complex combinations', () => {
    it('validates complex valid cookie options', () => {
      const complexOptions: CookieOptions = {
        expires: new Date(),
        maxAge: 3600,
        path: '/api',
        domain: '.example.com',
        secure: true,
        httpOnly: true,
        sameSite: 'Strict',
        priority: 'High',
        partitioned: false,
        size: {
          maxNameLength: 255,
          maxValueLength: 4096,
          maxTotalSize: 4096,
        },
        persistence: 'permanent',
        crossOrigin: {
          allowedOrigins: ['https://api.example.com'],
          policy: 'same-site',
        },
        validation: {
          minLength: 1,
          maxLength: 100,
          pattern: /^[a-z]+$/,
          validator: (value: string) => value.length > 0,
        },
      };

      expect(isCookieOptions(complexOptions)).toBeTrue();
    });

    it('invalidates complex invalid cookie options', () => {
      const invalidOptions = {
        expires: 'invalid-date',
        maxAge: -1,
        path: 123,
        secure: 'true',
        sameSite: 'Invalid',
        priority: 'Unknown',
        size: {
          maxNameLength: 'invalid',
        },
        crossOrigin: {
          allowedOrigins: [123],
          policy: 'invalid',
        },
        validation: {
          minLength: 'invalid',
          pattern: 'not-regex',
        },
      };

      expect(isCookieOptions(invalidOptions as any)).toBeFalse();
    });
  });
});
