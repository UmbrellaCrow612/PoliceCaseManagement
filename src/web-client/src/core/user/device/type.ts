export interface SendDeviceChallengeAttemptRequestBody {
  email: string;
}

export interface ValidateDeviceChallengeCode {
  email: string;
  code: string;
}
