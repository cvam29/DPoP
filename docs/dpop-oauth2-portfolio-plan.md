# DPoP OAuth 2.0 Portfolio Project Plan

## Summary

Build a self-contained portfolio repo with `src/backend` for a .NET 10 Azure Functions isolated-worker API and `src/frontend` for a Next.js client.

The project demonstrates OAuth 2.0 Authorization Code + PKCE with DPoP-bound access and refresh tokens, plus caching, distributed replay protection, rate limiting, authentication, authorization, audit logging, and observability.

Use Azure Functions isolated worker because .NET 10 is supported there. For Linux deployment, use Flex Consumption instead of Linux Consumption.

References:

- Azure Functions .NET support: https://learn.microsoft.com/en-us/azure/azure-functions/supported-languages
- Azure Functions isolated worker guide: https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide
- DPoP RFC 9449: https://www.rfc-editor.org/info/rfc9449/

## Architecture

- Backend: .NET 10 Azure Functions isolated worker.
- Local orchestration: .NET Aspire AppHost with ServiceDefaults, Azurite, and Redis resources.
- Frontend: Next.js App Router.
- Local durable storage: Azurite/Azure Table Storage.
- Local distributed cache: Redis-compatible cache.
- Deployment target: Azure Functions Flex Consumption, Azure Cache for Redis, Azure Storage, and a separately hosted Next.js frontend.

## Backend Use-Case Structure

- `Features/Identity`: demo login, logout, session cookie, seeded users.
- `Features/OAuth`: authorize, token, refresh, revoke, JWKS, metadata.
- `Features/Dpop`: proof validation, JWK thumbprint, replay detection, token binding.
- `Features/Todos`: protected user API showing scope and ownership authorization.
- `Features/Admin`: admin-only audit endpoint.
- `Features/PublicCatalog`: cached public endpoint.
- `Shared`: configuration, storage, rate limiting, errors, telemetry.
- `DpopPortfolio.AppHost`: Aspire orchestration for the backend runtime and local infrastructure.
- `DpopPortfolio.ServiceDefaults`: shared telemetry, resilience, service discovery, and health-check registration.

## Frontend Use-Case Structure

- `features/auth`: PKCE, OAuth state, callback handling, token lifecycle.
- `features/dpop`: WebCrypto keypair, DPoP proof JWT creation, IndexedDB key storage.
- `features/todos`: protected resource workflow.
- `features/admin`: role-based authorization demo.
- `features/playground`: inspect generated DPoP proofs and API responses.

## Public Interfaces

OAuth and identity endpoints:

- `POST /identity/login`
- `POST /identity/logout`
- `GET /oauth/authorize`
- `POST /oauth/token`
- `POST /oauth/revoke`
- `GET /.well-known/oauth-authorization-server`
- `GET /.well-known/jwks.json`

Protected APIs:

- `GET /api/profile`
- `GET /api/todos`
- `POST /api/todos`
- `PATCH /api/todos/{id}`
- `GET /api/admin/audit`
- `GET /api/public/catalog`

Access tokens are JWTs with `iss`, `aud`, `sub`, `scope`, `roles`, `exp`, and `cnf.jkt`.

Protected requests require `Authorization: DPoP <access_token>` and a `DPoP` proof JWT with validated `htm`, `htu`, `iat`, `jti`, `ath`, signature, and JWK thumbprint match.

Rate-limited responses return `429` with `Retry-After` and rate-limit headers.

## Implementation Milestones

1. Scaffold backend and frontend projects.
2. Add demo identity and OAuth authorization-code flow with PKCE.
3. Add DPoP key binding and proof validation.
4. Add protected Todo and Admin APIs.
5. Add Redis-backed replay cache and rate-limit policies.
6. Add public endpoint caching.
7. Add frontend DPoP proof generation with WebCrypto and IndexedDB.
8. Add observability, portfolio documentation, and deployment notes.

## Test Plan

- Backend unit tests for PKCE validation, DPoP proof validation, JWK thumbprint calculation, JWT claims, replay rejection, scope/role authorization, and rate-limit behavior.
- Backend integration tests for full authorize-code-token flow, token refresh, revoked refresh token rejection, protected API success, stolen-token-without-key failure, and replayed proof failure.
- Frontend tests for PKCE generation, DPoP key persistence, proof creation, callback handling, token refresh, and API client error states.
- End-to-end demo flow: login, obtain DPoP-bound token, call protected todo API, inspect DPoP proof in playground, trigger rate limit, verify admin denial/allow paths.

## Assumptions

- The project is educational and self-contained; the authorization server is a portfolio demo, not a production OAuth server.
- Demo users are seeded locally with hashed passwords and fixed roles: one user and one admin.
- Refresh tokens are stored hashed server-side and DPoP-bound to the same key thumbprint as the access token.
- Azure Functions Core Tools installation is documented but not installed automatically.
- Git is initialized and synced to GitHub; deployment remotes are not configured yet.
