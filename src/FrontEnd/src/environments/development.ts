const DevelopmentConfig = {
  EncryptionKey: 'development',
  EncryptionAlgorithm: 'SHA-256',
  BaseUrls: {
    authenticationBaseUrl: 'http://localhost:5185',
  },
} as const;

export default DevelopmentConfig;
