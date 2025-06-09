# DotNetCore Sample

This repository contains a multi-layered ASP.NET Core solution.

## Solution Layers

- **HCore.Domain** – Domain entities and repository interfaces.
- **HCore.Application** – Business logic, DTOs, and dependency injection helpers.
- **HCore.Infrastructure** – Entity Framework Core context, migrations, identity, and other infrastructure services.
- **HCore.API** – ASP.NET Core Web API project that hosts the application, configures Swagger, authentication, and seeds data.

## Prerequisites

- [.NET SDK 8.0](https://dotnet.microsoft.com/) or newer
- SQL Server (local or remote) for the application's database

## Build

```bash
# Restore packages and compile all projects
dotnet build
```

## Run Database Migrations

Use `dotnet ef` to create/update the database defined in `appsettings.json`.

```bash
# Apply migrations using the API as the startup project
# This ensures configuration such as the connection string is loaded

dotnet ef database update \
    --project HCore.Infrastructure \
    --startup-project HCore.API
```

## Start the API

```bash
# Launch the ASP.NET Core API

dotnet run --project HCore.API
```

The API will automatically seed initial data on first run.

## Configuration via Environment Variables

Settings from `appsettings.json` can be overridden by environment variables using the `:` notation.
Common variables include:

- `ConnectionStrings__DefaultConnection` – database connection string
- `JwtSettings__Secret` – JWT signing key
- `JwtSettings__Issuer` – token issuer
- `JwtSettings__Audience` – token audience
- `JwtSettings__ExpiryMinutes` – token lifetime
- `ASPNETCORE_ENVIRONMENT` – selects the environment (e.g., `Development`)

These variables allow customizing configuration without modifying files, which is useful in containerized or cloud environments.