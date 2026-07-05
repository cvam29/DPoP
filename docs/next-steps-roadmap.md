# Next Steps Roadmap

## Recommended Roadmap

1. Backend Foundation
   - Add a `.sln` file and backend test project.
   - Add typed configuration for issuer, audience, frontend origin, token lifetimes, cache provider, and storage provider.
   - Add CORS, shared error responses, request logging, and a health endpoint.
   - Add `docker-compose.yml` for Azurite and Redis.
   - Add Aspire AppHost and ServiceDefaults for backend-local orchestration.
   - Status: completed in the backend foundation milestone.
   - Acceptance: backend builds, tests run, local dependencies are documented.

2. Identity And OAuth Code Flow
   - Seed demo users: regular user and admin.
   - Implement `POST /identity/login` and `POST /identity/logout`.
   - Implement `GET /oauth/authorize` with Authorization Code + PKCE.
   - Store auth codes with short TTL and one-time use.
   - Acceptance: frontend can start the auth flow and receive an authorization code.

3. Token Issuance
   - Implement `POST /oauth/token`.
   - Validate the PKCE verifier.
   - Require a DPoP proof on token requests.
   - Issue JWT access tokens with `cnf.jkt`.
   - Issue hashed refresh tokens bound to the same DPoP key.
   - Acceptance: token response returns a DPoP-bound access token and refresh token.

4. DPoP Validation Core
   - Implement proof JWT validation: signature, `htu`, `htm`, `iat`, `jti`, and `ath`.
   - Implement JWK thumbprint calculation.
   - Add Redis-backed replay cache for `jti`.
   - Add backend unit tests for valid proof, replayed proof, mismatched method, mismatched URL, stale proof, and stolen token.
   - Acceptance: protected APIs reject bearer-style or replayed requests.

5. Protected Resource APIs
   - Implement `GET /api/profile`.
   - Implement Todo CRUD with `todos.read` and `todos.write`.
   - Implement `GET /api/admin/audit` with role and scope checks.
   - Add rate limiting to token and protected APIs.
   - Acceptance: user/admin/scopes produce correct `200`, `401`, and `403` behavior.

6. Frontend Auth Experience
   - Add WebCrypto DPoP key generation.
   - Persist the browser key in IndexedDB.
   - Add PKCE generation and OAuth callback handling.
   - Add token storage strategy and DPoP-signed API client.
   - Build UI flows for login, todos, admin denial/success, and proof playground.
   - Acceptance: browser can complete login, get a token, call a protected API, and show replay/stolen-token demos.

7. Portfolio Polish
   - Add architecture diagrams to `docs`.
   - Add threat model and security tradeoffs.
   - Add deployment guide for Azure Functions Flex Consumption, Azure Storage, Redis, and frontend hosting.
   - Add screenshots or GIFs of the DPoP flow.
   - Acceptance: repo reads like a professional portfolio project, not just a code sample.

## Best Next Commit

Backend foundation plus test project should come next because every later OAuth/DPoP piece depends on clean configuration, test structure, storage/cache abstractions, and local infrastructure.
