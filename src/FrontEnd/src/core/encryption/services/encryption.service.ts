import { Injectable } from "@angular/core";
import DevelopmentConfig from "../../../environments/development";

@Injectable({
  providedIn: "root",
})
export class EncryptionService {

    private encoder = new TextEncoder();

    async Hash(data: string): Promise<string> {
        const encodedData = this.encoder.encode(data);
        const hashBuffer = await crypto.subtle.digest(DevelopmentConfig.EncryptionAlgorithm, encodedData);
        return Array.from(new Uint8Array(hashBuffer))
        .map(byte => byte.toString(16).padStart(2, '0'))
        .join('');
    }

    async CompareHash(data: string, hash: string): Promise<boolean> {
        var hashData = await this.Hash(data);
        return hashData === hash;
    }
}