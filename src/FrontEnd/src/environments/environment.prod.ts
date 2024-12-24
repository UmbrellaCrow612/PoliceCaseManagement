import { LogLevel } from '../core/app/services/loggingService.service';

export const env = {
  production: true,
  config: {
    EncryptionKey: 'production-key',
    EncryptionAlgorithm: 'SHA-512',
    BaseUrls: {
      authenticationBaseUrl: 'https://api.example.com',
    },
    TokenCheckInterval: 3000,
    LogLevel: LogLevel.Warn,
  },
};
