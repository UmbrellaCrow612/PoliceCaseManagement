const env = {
  BaseUrls: {
    authenticationBaseUrl: 'https://localhost:55724',
    casesBaseUrl: 'https://localhost:7123',
  },
  JwtTokenValidationPeriodInMinutesInMilliSeconds: 5 * 60 * 1000,
  JWTTokenValidationInitialWaitTimeInMilliSeconds: 25000,
} as const;

export default env;
