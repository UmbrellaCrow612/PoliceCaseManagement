import { LogLevel } from '../core/app/services/loggingService.service';

const env = {
  BaseUrls: {
    authenticationBaseUrl: 'http://localhost:5185',
  },
  JwtTokenValidationPeriodInMinutes: 3 * 60 * 1000,
  LogLevel: LogLevel.Error,
} as const;

export default env;
