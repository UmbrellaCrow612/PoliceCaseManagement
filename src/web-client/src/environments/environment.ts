const env = {
  BaseUrls: {
    authenticationBaseUrl: 'https://localhost:7058',
    casesBaseUrl: 'https://localhost:7123',
  },
  JwtTokenValidationPeriodInMinutesInMilliSeconds: 5 * 60 * 1000,
  JWTTokenValidationInitialWaitTimeInMilliSeconds: 25000,
} as const;

export default env;
