# DPoP OAuth 2.0 Portfolio

Self-contained portfolio project demonstrating OAuth 2.0 Demonstrating Proof of Possession (DPoP) with a .NET 10 Azure Functions backend and a Next.js frontend.

## Repo Layout

- `docs/` - architecture, implementation, and portfolio notes.
- `DpopPortfolio.sln` - backend solution with Functions app and tests.
- `src/backend/` - .NET 10 Azure Functions isolated-worker backend.
- `src/frontend/` - Next.js App Router frontend.

## Concepts To Showcase

- OAuth 2.0 Authorization Code + PKCE.
- DPoP-bound access and refresh tokens.
- Authentication and authorization.
- Replay protection with cache-backed `jti` tracking.
- Rate limiting for token and API endpoints.
- Public data caching.
- Audit logging and observability.

## Local Tooling

Installed on this machine:

- .NET SDK `10.0.301`
- Node.js `v24.18.0`
- npm `11.16.0`

Still needed for full Azure Functions local execution:

- Azure Functions Core Tools v4.
- Docker Desktop or compatible Docker runtime for `docker compose`.
- Azurite for local Azure Storage emulation.
- Redis or compatible cache for distributed replay/rate-limit demonstrations.

## Getting Started

Backend build and tests:

```powershell
dotnet build .\DpopPortfolio.sln
dotnet test .\DpopPortfolio.sln
```

Frontend build:

```powershell
cd .\src\frontend
npm run build
```

Azure Functions runtime execution requires Azure Functions Core Tools:

```powershell
func start
```

Local Azurite and Redis infrastructure can be started with:

```powershell
docker compose up -d
```

See [docs/local-development.md](docs/local-development.md) for local configuration, health checks, and infrastructure notes.
