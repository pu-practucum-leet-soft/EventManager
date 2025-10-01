# EventManager

Full-stack Event Management application consisting of:

- **Server**: ASP.NET Core 8 Web API (JWT auth, Entity Framework Core, SQL Server, Identity)
- **Client**: React 19 + TypeScript + Vite + React Query + Redux Toolkit

## Prerequisites

| Component          | Required                     | Notes                                                |
| ------------------ | ---------------------------- | ---------------------------------------------------- |
| Node.js            | >= 20.x                      | For the frontend (Vite)                              |
| pnpm / npm / yarn  | (one)                        | Commands below use `npm` but any works               |
| .NET SDK           | 8.0.x                        | For building/running the API                         |
| SQL Server         | Local / Docker               | Connection string currently targets `localhost,1433` |
| OpenSSL (optional) | For local HTTPS cert trust   | Vite dev uses `vite-plugin-mkcert`                   |
| Docker (optional)  | For containerized deployment | Dockerfile exists for server                         |

## Repository Structure

```
EventManager/
  client/        # React frontend
  server/        # ASP.NET Core backend
```

## Backend (server)

### Configuration

Configuration files: `server/appsettings.json` and `appsettings.Development.json`.

Important sections:

- `ConnectionStrings:DefaultConnection`
- `JwtSettings` (Issuer, Audience, Secret, ExpiryMinutes)
- `RefreshTokenSettings`

> WARNING: The JWT secret is currently committed. In production move it to environment variables / secret store.

### Database

The app uses EF Core with SQL Server. Migrations live in `server/Data/Migrations`.

To apply migrations automatically on run, ensure the database exists. If it does not, create it or run:

```bash
# From repo root
cd server
# Create database (ensure SQL Server instance is running)
dotnet ef database update
```

(If `dotnet-ef` tool is missing: `dotnet tool install --global dotnet-ef`.)

### Seeding

Roles are seeded on startup (`RoleDataSeeder.SeedRolesAsync`). Ensure the database user has rights.

### Running the API

```bash
cd server
# Restore & run (HTTPS + Swagger in Development)
dotnet run
```

By default it will:

- Bind to HTTPS (Kestrel) (check console output for the exact port, often 5001/7057 style)
- Expose Swagger UI at `/swagger`
- Enable CORS for `https://localhost:5173`

If you need to override the connection string or JWT values temporarily:

```bash
DOTNET_ENVIRONMENT=Development \
ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=EventManagerDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;" \
JwtSettings__Secret="YourSuperSecretKey" \
dotnet run
```

### Docker (Backend)

A `Dockerfile` is present under `server/`.
Example build & run:

```bash
cd server
docker build -t eventmanager-api .
docker run -e ASPNETCORE_ENVIRONMENT=Development -p 8080:8080 -p 8081:8081 eventmanager-api
```

You will still need a SQL Server instance accessible from inside the container (link a container network or use host networking).

## Frontend (client)

### Install Dependencies

```bash
cd client
npm install
```

### Development Run

```bash
npm run dev
```

This starts Vite (with mkcert) typically at `https://localhost:5173`.
If it's the first time using `vite-plugin-mkcert`, you may have to trust the generated certificates (follow terminal prompts).

### Build / Preview

```bash
npm run build
npm run preview
```

### Environment Variables

If you introduce runtime configuration (e.g., API base URL), create a `.env` in `client/`:

```
VITE_API_BASE_URL=https://localhost:7057
```

And consume via `import.meta.env.VITE_API_BASE_URL`.

## API Consumption Pattern

The client uses:

- Axios instance (see `client/src/queries/axios.ts`)
- React Query for data fetching (`useQuery`, `useMutation`)
- Redux Toolkit for global auth / state

Ensure the Axios base URL matches the backend HTTPS address (adjust if ports differ).

## Auth Flow (High-Level)

1. User logs in → receives JWT (and possibly refresh token if implemented).
2. JWT sent in `Authorization: Bearer <token>` header for protected routes.
3. On `401` + `Token-Expired: true` header, trigger refresh / logout logic (implement in Axios interceptor if not already).

## Adding a New Migration

```bash
cd server
dotnet ef migrations add MeaningfulName
# Apply it
dotnet ef database update
```

## Quick Start Summary

```bash
# Backend
cd server
(dotnet ef database update) # if first time
dotnet run

# Frontend (new terminal)
cd client
npm install
npm run dev
```

Open the browser at: https://localhost:5173
Swagger UI (API docs): https://localhost:<api-https-port>/swagger
