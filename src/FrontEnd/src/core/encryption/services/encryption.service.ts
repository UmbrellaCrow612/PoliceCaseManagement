import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root",
})
export class EncryptionService {

    private encoder = new TextEncoder();
    
    Encrypt(data: string): string {
        return ""
    }
    
    Decrypt(data: string): string {
        return ""
    }

    async Hash(data: string): Promise<string> {
        const encodedData = this.encoder.encode(data);
        const hashBuffer = await crypto.subtle.digest('SHA-256', encodedData);
        return Array.from(new Uint8Array(hashBuffer))
        .map(byte => byte.toString(16).padStart(2, '0'))
        .join('');
    }

    CompareHash(data: string, hash: string): boolean {
        return false
    }
}