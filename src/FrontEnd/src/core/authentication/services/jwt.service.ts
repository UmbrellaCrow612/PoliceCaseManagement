import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { JwtPayload } from '../types';
@Injectable({
  providedIn: 'root'
})
export class JwtService {
  /**
   * Decodes a JWT token and returns the payload
   * @template T The expected type of the JWT payload, extends JwtPayload
   * @param {string} token The JWT token to decode
   * @returns {T} The decoded token payload
   * @throws {Error} If the token is invalid or cannot be decoded
   * 
   * @example
   * interface MyCustomPayload extends JwtPayload {
   *   customField: string;
   *   userId: number;
   * }
   * 
   * const payload = jwtService.decodeToken<MyCustomPayload>(token);
   * console.log(payload.customField);
   */
  decodeToken<T extends JwtPayload>(token: string): T {
    try {
      return jwtDecode<T>(token);
    } catch (error) {
      throw new Error('Failed to decode JWT token: ' + (error as Error).message);
    }
  }
}