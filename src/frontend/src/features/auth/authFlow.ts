export const authFlow = [
  {
    order: "01",
    title: "Demo session",
    detail: "A seeded user signs in and receives a secure same-site session cookie.",
    endpoint: "POST /identity/login",
  },
  {
    order: "02",
    title: "Authorization request",
    detail: "The browser starts Authorization Code + PKCE with a public-client code challenge.",
    endpoint: "GET /oauth/authorize",
  },
  {
    order: "03",
    title: "DPoP token exchange",
    detail: "The token request includes a signed proof so issued tokens are bound to the browser key.",
    endpoint: "POST /oauth/token",
  },
  {
    order: "04",
    title: "Protected resource",
    detail: "Every API call presents the bound token and a fresh DPoP proof JWT.",
    endpoint: "GET /api/todos",
  },
] as const;
