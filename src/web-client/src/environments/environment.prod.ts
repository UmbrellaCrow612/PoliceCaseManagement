const env = {
  BaseUrls: {
    authenticationBaseUrl: 'https://localhost:7058',
  },
  JwtTokenValidationPeriodInMinutesInMilliSeconds: 3 * 60 * 1000,
  JWTTokenValidationInitialWaitTimeInMilliSeconds: 25000,
} as const;

export default env;
