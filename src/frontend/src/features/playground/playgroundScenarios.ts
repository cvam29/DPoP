export const playgroundScenarios = [
  {
    name: "Valid DPoP request",
    result: "Shows the proof JWT, access token binding, and protected API response.",
  },
  {
    name: "Replayed proof",
    result: "Reuses the same jti and receives a replay rejection.",
  },
  {
    name: "Stolen token",
    result: "Uses a token without the matching private key and receives invalid_token.",
  },
  {
    name: "Rate limit",
    result: "Triggers 429 with Retry-After and rate-limit headers.",
  },
] as const;
