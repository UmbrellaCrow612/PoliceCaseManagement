const env = {
  BaseUrls: {
    authenticationBaseUrl: 'https://localhost:7101/identity-api',
    casesBaseUrl: 'https://localhost:7101/cases-api', 
  },
  JwtTokenValidationPeriodInMinutesInMilliSeconds: 5 * 60 * 1000,
  JWTTokenValidationInitialWaitTimeInMilliSeconds: 25000,
} as const;

export default env;
