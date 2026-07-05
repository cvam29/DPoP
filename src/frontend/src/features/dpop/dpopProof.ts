export const dpopProofChecks = [
  {
    claim: "htu",
    validation: "Matches the normalized target URI for the current request.",
  },
  {
    claim: "htm",
    validation: "Matches the HTTP method used by the protected API call.",
  },
  {
    claim: "iat",
    validation: "Falls within the accepted proof age window.",
  },
  {
    claim: "jti",
    validation: "Is cached once and rejected on replay.",
  },
  {
    claim: "ath",
    validation: "Binds the proof to the access token hash.",
  },
  {
    claim: "cnf.jkt",
    validation: "Matches the JWK thumbprint from the proof header.",
  },
] as const;
