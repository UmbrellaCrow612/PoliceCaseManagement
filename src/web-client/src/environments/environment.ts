import { LogLevel } from '../core/app/services/loggingService.service';

const env = {
  EncryptionKey: 'development',
  EncryptionAlgorithm: 'SHA-256',
  BaseUrls: {
    authenticationBaseUrl: 'https://localhost:7058',
  },
  TokenCheckInterval: 5000,
  LogLevel: LogLevel.Error,
} as const;

export default env;
