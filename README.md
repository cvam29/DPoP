# DPoP OAuth 2.0 Portfolio

Self-contained portfolio project demonstrating OAuth 2.0 Demonstrating Proof of Possession (DPoP) with a .NET 10 Azure Functions backend, .NET Aspire orchestration, and a Next.js frontend.

## Repo Layout

- `docs/` - architecture, implementation, and portfolio notes.
- `src/backend/DpopPortfolio.sln` - backend-only solution with Functions, Aspire AppHost, ServiceDefaults, and tests.
- `src/backend/DpopPortfolio.AppHost` - Aspire orchestration for Functions, Azurite, and Redis.
- `src/backend/DpopPortfolio.Functions` - .NET 10 Azure Functions isolated-worker backend.
- `src/backend/DpopPortfolio.Functions.Tests` - backend xUnit tests.
- `src/backend/DpopPortfolio.ServiceDefaults` - Aspire service defaults for telemetry, resilience, and service discovery.
- `src/frontend/` - Next.js App Router frontend.

## Architecture

```mermaid
flowchart LR
    Browser[Browser]
    Frontend[Next.js App Router]
    AppHost[.NET Aspire AppHost]
    Functions[.NET 10 Azure Functions]
    OAuth[OAuth + Identity use cases]
    DPoP[DPoP validation use case]
    Api[Protected resource APIs]
    Cache[Cache + replay tracking]
    RateLimit[Rate limiting]
    Storage[Azurite or Azure Storage]
    Redis[Redis]

    Browser --> Frontend
    Frontend -->|Authorization Code + PKCE| OAuth
    Frontend -->|Authorization: DPoP + proof JWT| Functions
    AppHost --> Functions
    AppHost --> Storage
    AppHost --> Redis
    Functions --> OAuth
    Functions --> DPoP
    Functions --> Api
    OAuth --> Storage
    DPoP --> Cache
    Api --> RateLimit
    Cache --> Redis
```

## DPoP Flow

```mermaid
sequenceDiagram
    participant User
    participant Next as Next.js frontend
    participant Auth as Azure Functions OAuth endpoints
    participant Api as Protected APIs
    participant Replay as Replay cache

    User->>Next: Start demo login
    Next->>Next: Generate PKCE verifier and DPoP key
    Next->>Auth: GET /oauth/authorize
    Auth-->>Next: Authorization code
    Next->>Auth: POST /oauth/token with DPoP proof
    Auth->>Replay: Store proof jti
    Auth-->>Next: DPoP-bound access token with cnf.jkt
    Next->>Api: Call API with Authorization: DPoP and proof JWT
    Api->>Replay: Reject replayed proof jti
    Api-->>Next: Protected response or 401/403
```

## Concepts To Showcase

- OAuth 2.0 Authorization Code + PKCE.
- DPoP-bound access and refresh tokens.
- Authentication and authorization.
- Replay protection with cache-backed `jti` tracking.
- Rate limiting for token and API endpoints.
- Public data caching.
- Aspire dashboard observability and local orchestration.
- Audit logging and portfolio-focused security documentation.

## Local Tooling

Installed on this machine:

- .NET SDK `10.0.301`
- Node.js `v24.18.0`
- npm `11.16.0`

Still needed for full local runtime execution:

- Azure Functions Core Tools v4.
- Docker Desktop or compatible Docker runtime for Aspire containers or `docker compose`.
- Azurite for local Azure Storage emulation.
- Redis or compatible cache for distributed replay/rate-limit demonstrations.

## Getting Started

Backend build and tests:

```powershell
dotnet build .\src\backend\DpopPortfolio.sln
dotnet test .\src\backend\DpopPortfolio.sln
```

Run the Aspire AppHost:

```powershell
dotnet run --project .\src\backend\DpopPortfolio.AppHost\DpopPortfolio.AppHost.csproj
```

Frontend build:

```powershell
cd .\src\frontend
npm run build
```

Azure Functions direct runtime execution still requires Azure Functions Core Tools:

```powershell
cd .\src\backend\DpopPortfolio.Functions
func start
```

Local Azurite and Redis infrastructure can also be started without Aspire:

```powershell
docker compose up -d
```

See [docs/local-development.md](docs/local-development.md) for local configuration, health checks, and infrastructure notes.
