const env = {
  BaseUrls: {
    authenticationBaseUrl: 'http://localhost:8088/identity',
    casesBaseUrl: 'http://localhost:8088/cases', 
  },
  JwtTokenValidationPeriodInMinutesInMilliSeconds: 5 * 60 * 1000,
  JWTTokenValidationInitialWaitTimeInMilliSeconds: 25000,
} as const;

export default env;
