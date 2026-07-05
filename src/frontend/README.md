# DPoP OAuth 2.0 Frontend

Next.js App Router frontend for the DPoP OAuth 2.0 portfolio project.

## Use-Case Structure

- `src/features/auth` - PKCE and OAuth callback flow.
- `src/features/dpop` - browser key management and proof JWT creation.
- `src/features/todos` - protected resource API workflow.
- `src/features/admin` - role-gated audit surface.
- `src/features/playground` - replay, stolen-token, and rate-limit demonstrations.

## Local Development

First, run the development server:

```bash
npm run dev
```

Open `http://localhost:3000` with your browser to see the app.

Copy `.env.example` to `.env.local` and update `NEXT_PUBLIC_BACKEND_URL` if the Azure Functions host uses a different port.

The generated app targets Next.js 16 and uses the local docs in `node_modules/next/dist/docs` for version-specific conventions.
