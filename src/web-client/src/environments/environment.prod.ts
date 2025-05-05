const env = {
  BaseUrls: {
    authenticationBaseUrl: 'http://localhost:8082',
    casesBaseUrl: 'http://localhost:8080', // here refer internal in container
  },
  JwtTokenValidationPeriodInMinutesInMilliSeconds: 5 * 60 * 1000,
  JWTTokenValidationInitialWaitTimeInMilliSeconds: 25000,
} as const;

export default env;
