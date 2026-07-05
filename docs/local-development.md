# Local Development

## Tooling Status

This repo currently builds with the installed local SDKs:

- .NET SDK `10.0.301`
- Node.js `v24.18.0`
- npm `11.16.0`

Azure Functions Core Tools v4 is still required to run the backend host directly with `func start`. It is documented here but not installed automatically. The Aspire AppHost also needs Azure Functions Core Tools because it starts the Functions runtime as part of orchestration.

## Backend

Build and test the backend solution:

```powershell
dotnet build .\src\backend\DpopPortfolio.sln
dotnet test .\src\backend\DpopPortfolio.sln
```

Create a local Functions settings file from the sample when Core Tools is installed:

```powershell
Copy-Item .\src\backend\DpopPortfolio.Functions\local.settings.sample.json .\src\backend\DpopPortfolio.Functions\local.settings.json
```

Run the Aspire AppHost when Docker and Azure Functions Core Tools are available:

```powershell
dotnet run --project .\src\backend\DpopPortfolio.AppHost\DpopPortfolio.AppHost.csproj
```

The AppHost models the Functions backend, Azurite host storage, and Redis cache in one local dashboard.

Start the local infrastructure without Aspire:

```powershell
docker compose up -d
```

The compose file exposes:

- Azurite Blob: `http://localhost:10000`
- Azurite Queue: `http://localhost:10001`
- Azurite Table: `http://localhost:10002`
- Redis: `localhost:6379`

Run the backend once Azure Functions Core Tools v4 is installed:

```powershell
cd .\src\backend\DpopPortfolio.Functions
func start
```

Health check:

```powershell
Invoke-RestMethod http://localhost:7071/health
```

## Configuration

The backend uses typed configuration under the `DpopPortfolio` section:

- `Issuer`
- `Audience`
- `FrontendOrigin`
- `CacheProvider`
- `StorageProvider`
- `TokenLifetimes:AuthorizationCodeMinutes`
- `TokenLifetimes:AccessTokenMinutes`
- `TokenLifetimes:RefreshTokenDays`
- `TokenLifetimes:DpopProofSeconds`

The sample local settings use `DpopPortfolio__...` environment keys so the same shape works in Azure App Settings.

## Frontend

Run the frontend from `src/frontend`:

```powershell
npm install
npm run dev
```

The frontend expects the Functions backend at `NEXT_PUBLIC_BACKEND_URL=http://localhost:7071`.
