const env = {
  BaseUrls: {
    authenticationBaseUrl: 'https://localhost:7101/identity-api',
    casesBaseUrl: 'https://localhost:7101/cases-api', 
    evidenceBaseUrl: 'https://localhost:7101/evidence-api', 
  },
 
} as const;

export default env;
