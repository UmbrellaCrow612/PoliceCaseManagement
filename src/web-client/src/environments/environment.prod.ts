const env = {
  BaseUrls: {
    authenticationBaseUrl: 'http://localhost:8082',
    casesBaseUrl: 'http://localhost:8080', // here refer internal in container
  },
} as const;

export default env;
