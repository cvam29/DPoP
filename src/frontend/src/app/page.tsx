import { adminAuditChecks } from "@/features/admin/adminAudit";
import { authFlow } from "@/features/auth/authFlow";
import { dpopProofChecks } from "@/features/dpop/dpopProof";
import { playgroundScenarios } from "@/features/playground/playgroundScenarios";
import { todoApiSurface } from "@/features/todos/todoApi";

const conceptRows = [
  { label: "Backend", value: ".NET 10 Azure Functions", tone: "teal" },
  { label: "Frontend", value: "Next.js App Router", tone: "coral" },
  { label: "Storage", value: "Azurite to Azure Storage", tone: "gold" },
  { label: "Cache", value: "Memory to Redis", tone: "green" },
] as const;

const implementationSlices = [
  "Authorization Code + PKCE",
  "DPoP-bound JWT access tokens",
  "Refresh-token rotation and revocation",
  "Replay protection through jti cache",
  "Rate limits for token and API routes",
  "Scope, role, and ownership checks",
] as const;

const apiGroups = [
  {
    name: "Identity",
    routes: ["POST /identity/login", "POST /identity/logout", "GET /identity/demo-users"],
  },
  {
    name: "OAuth",
    routes: [
      "GET /oauth/authorize",
      "POST /oauth/token",
      "POST /oauth/revoke",
      "GET /.well-known/oauth-authorization-server",
    ],
  },
  {
    name: "Resources",
    routes: ["GET /api/public/catalog", "GET /api/todos", "POST /api/todos", "GET /api/admin/audit"],
  },
] as const;

export default function Home() {
  const backendBaseUrl = process.env.NEXT_PUBLIC_BACKEND_URL ?? "http://localhost:7071";

  return (
    <main className="min-h-screen bg-[#f6f7f9] text-[#161a1d]">
      <section className="border-b border-[#d8dee4] bg-white">
        <div className="mx-auto flex w-full max-w-7xl flex-col gap-10 px-5 py-6 sm:px-8 lg:px-10">
          <header className="flex flex-col gap-5 border-b border-[#e3e7eb] pb-6 lg:flex-row lg:items-center lg:justify-between">
            <div>
              <p className="text-sm font-semibold uppercase text-[#087f8c]">
                DPoP OAuth 2.0 Portfolio
              </p>
              <h1 className="mt-3 max-w-4xl text-4xl font-semibold leading-tight text-[#111517] sm:text-5xl">
                Sender-constrained OAuth flow with caching, rate limiting, and protected APIs.
              </h1>
            </div>
            <nav aria-label="Project links" className="flex flex-wrap gap-2 text-sm font-medium">
              <a className="rounded-md border border-[#cfd6dd] px-3 py-2 text-[#1f2933] transition hover:border-[#087f8c] hover:text-[#087f8c]" href={`${backendBaseUrl}/api/public/catalog`}>
                Catalog API
              </a>
              <a className="rounded-md border border-[#cfd6dd] px-3 py-2 text-[#1f2933] transition hover:border-[#c2410c] hover:text-[#c2410c]" href={`${backendBaseUrl}/.well-known/oauth-authorization-server`}>
                Metadata
              </a>
            </nav>
          </header>

          <div className="grid gap-6 lg:grid-cols-[1.05fr_0.95fr]">
            <section className="self-start rounded-lg border border-[#d8dee4] bg-[#fbfcfd] p-5">
              <div className="grid gap-3 sm:grid-cols-2">
                {conceptRows.map((row) => (
                  <div key={row.label} className="min-h-28 rounded-md border border-[#dbe1e7] bg-white p-4">
                    <p className="text-sm font-semibold text-[#56616f]">{row.label}</p>
                    <p className="mt-3 text-xl font-semibold text-[#161a1d]">{row.value}</p>
                    <div className={`mt-4 h-1.5 rounded-sm ${toneBarClass(row.tone)}`} />
                  </div>
                ))}
              </div>
            </section>

            <section className="rounded-lg border border-[#d8dee4] bg-[#111517] p-5 text-white">
              <h2 className="text-xl font-semibold">Protocol Path</h2>
              <div className="mt-5 grid gap-3">
                {authFlow.map((step) => (
                  <div key={step.title} className="grid grid-cols-[2.5rem_1fr] gap-3 rounded-md border border-white/15 bg-white/5 p-3">
                    <span className="flex h-10 w-10 items-center justify-center rounded-md bg-[#087f8c] text-sm font-bold">
                      {step.order}
                    </span>
                    <div>
                      <p className="font-semibold">{step.title}</p>
                      <p className="mt-1 text-sm leading-6 text-[#d5dde4]">{step.detail}</p>
                      <p className="mt-2 font-mono text-xs text-[#f6c453]">{step.endpoint}</p>
                    </div>
                  </div>
                ))}
              </div>
            </section>
          </div>
        </div>
      </section>

      <section className="mx-auto grid w-full max-w-7xl gap-6 px-5 py-8 sm:px-8 lg:grid-cols-[0.95fr_1.05fr] lg:px-10">
        <div className="grid gap-6">
          <Panel title="Implementation Slices">
            <ul className="grid gap-2">
              {implementationSlices.map((slice) => (
                <li key={slice} className="flex items-center gap-3 rounded-md border border-[#dbe1e7] bg-white px-3 py-3 text-sm font-medium">
                  <span className="h-2.5 w-2.5 rounded-sm bg-[#087f8c]" />
                  {slice}
                </li>
              ))}
            </ul>
          </Panel>

          <Panel title="DPoP Proof Checks">
            <div className="grid gap-3 sm:grid-cols-2">
              {dpopProofChecks.map((check) => (
                <div key={check.claim} className="rounded-md border border-[#dbe1e7] bg-white p-3">
                  <p className="font-mono text-sm font-semibold text-[#c2410c]">{check.claim}</p>
                  <p className="mt-2 text-sm leading-6 text-[#4d5965]">{check.validation}</p>
                </div>
              ))}
            </div>
          </Panel>
        </div>

        <div className="grid gap-6">
          <Panel title="Backend Surface">
            <div className="grid gap-4">
              {apiGroups.map((group) => (
                <section key={group.name} className="rounded-md border border-[#dbe1e7] bg-white p-4">
                  <h3 className="text-sm font-semibold uppercase text-[#56616f]">{group.name}</h3>
                  <div className="mt-3 grid gap-2">
                    {group.routes.map((route) => (
                      <code key={route} className="rounded-md bg-[#f2f4f7] px-3 py-2 font-mono text-xs text-[#1f2933]">
                        {route}
                      </code>
                    ))}
                  </div>
                </section>
              ))}
            </div>
          </Panel>

          <div className="grid gap-6 xl:grid-cols-2">
            <Panel title="Resource API">
              <ul className="grid gap-3">
                {todoApiSurface.map((item) => (
                  <li key={item.scope} className="rounded-md border border-[#dbe1e7] bg-white p-3">
                    <p className="font-mono text-xs font-semibold text-[#087f8c]">{item.scope}</p>
                    <p className="mt-2 text-sm leading-6 text-[#4d5965]">{item.behavior}</p>
                  </li>
                ))}
              </ul>
            </Panel>

            <Panel title="Portfolio Playground">
              <ul className="grid gap-3">
                {playgroundScenarios.map((scenario) => (
                  <li key={scenario.name} className="rounded-md border border-[#dbe1e7] bg-white p-3">
                    <p className="font-semibold text-[#161a1d]">{scenario.name}</p>
                    <p className="mt-2 text-sm leading-6 text-[#4d5965]">{scenario.result}</p>
                  </li>
                ))}
              </ul>
            </Panel>
          </div>

          <Panel title="Admin Authorization">
            <div className="grid gap-3 sm:grid-cols-3">
              {adminAuditChecks.map((check) => (
                <div key={check.name} className="rounded-md border border-[#dbe1e7] bg-white p-3">
                  <p className="font-semibold text-[#161a1d]">{check.name}</p>
                  <p className="mt-2 text-sm leading-6 text-[#4d5965]">{check.rule}</p>
                </div>
              ))}
            </div>
          </Panel>
        </div>
      </section>
    </main>
  );
}

function Panel({
  title,
  children,
}: Readonly<{
  title: string;
  children: React.ReactNode;
}>) {
  return (
    <section className="rounded-lg border border-[#d8dee4] bg-[#fbfcfd] p-5">
      <h2 className="text-lg font-semibold text-[#161a1d]">{title}</h2>
      <div className="mt-4">{children}</div>
    </section>
  );
}

function toneBarClass(tone: "teal" | "coral" | "gold" | "green") {
  switch (tone) {
    case "teal":
      return "bg-[#087f8c]";
    case "coral":
      return "bg-[#c2410c]";
    case "gold":
      return "bg-[#b7791f]";
    case "green":
      return "bg-[#166534]";
  }
}
