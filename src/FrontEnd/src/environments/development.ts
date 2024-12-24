import { LogLevel } from "../core/app/services/loggingService.service";

const DevelopmentConfig = {
  EncryptionKey: 'development',
  EncryptionAlgorithm: 'SHA-256',
  BaseUrls: {
    authenticationBaseUrl: 'http://localhost:5185',
  },
  TokenCheckInterval: 5000,
  LogLevel: LogLevel.Error,
} as const;

export default DevelopmentConfig;
