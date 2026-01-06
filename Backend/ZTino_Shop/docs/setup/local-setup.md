# Local Setup Guide

This document provides step-by-step instructions to run ZTino_Shop backend locally.

## Prerequisites

| Requirement | Version | Purpose |
|-------------|---------|---------|
| .NET SDK | 8.0+ | Runtime and build |
| SQL Server | 2019+ | Database |
| Redis | 6.0+ | Caching (optional) |
| IDE | VS/Rider/VS Code | Development |

### Install .NET SDK

Download from: https://dotnet.microsoft.com/download/dotnet/8.0

Verify installation:
```bash
dotnet --version
```

---

## Clone Repository

```bash
git clone <repository-url>
cd ZTino_Shop
```

---

## Configuration

### 1. Create Configuration File

Copy the template:
```bash
cd src/WebAPI
copy appsettings.template.json appsettings.json
```

### 2. Configure Database

Edit `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ZTinoShop;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**Options**:
- Windows Auth: `Trusted_Connection=True`
- SQL Auth: `User Id=sa;Password=YourPassword`

### 3. Configure JWT

```json
{
  "Jwt": {
    "Secret": "your-super-secret-key-at-least-32-characters",
    "ValidIssuer": "ZTinoShop",
    "ValidAudience": "ZTinoShopUsers",
    "AccessTokenExpiryMinutes": 15,
    "RefreshTokenExpiryDays": 7
  }
}
```

### 4. Optional: Redis

```json
{
  "Redis": {
    "ConnectionString": "localhost:6379"
  }
}
```

### 5. Optional: External Services

```json
{
  "Cloudinary": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret"
  },
  "FacebookAuth": {
    "AppId": "your-app-id",
    "AppSecret": "your-app-secret"
  },
  "GoogleAuth": {
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret"
  }
}
```

---

## Database Setup

### Create Database

If using SQL Server Management Studio:
1. Connect to server
2. Right-click Databases → New Database
3. Name: `ZTinoShop`

Or let EF Core create it:
```bash
# From solution root
dotnet ef database update --project src/Infrastructure --startup-project src/WebAPI
```

### Run Migrations

```bash
dotnet ef database update --project src/Infrastructure --startup-project src/WebAPI
```

---

## Build and Run

### Restore Dependencies

```bash
dotnet restore
```

### Build

```bash
dotnet build
```

### Run

```bash
cd src/WebAPI
dotnet run
```

### Verify

Open browser:
- Swagger UI: `https://localhost:<PORT>/swagger`
- API: `https://localhost:<PORT>/api/v1/products`

Port shown in console output (typically 5001 for HTTPS).

---

## Development Mode

### Hot Reload

```bash
dotnet watch run
```

Automatically rebuilds on file changes.

### Debug in IDE

**Visual Studio**:
1. Open `ZTino_Shop.sln`
2. Set `WebAPI` as startup project
3. Press F5

**Rider**:
1. Open solution
2. Select `WebAPI` configuration
3. Click Run/Debug

**VS Code**:
1. Open folder
2. Install C# extension
3. Press F5 (configure launch.json if needed)

---

## Running Tests

```bash
dotnet test
```

Or run specific project:
```bash
dotnet test tests/Application.Tests
```

---

## Common Issues

### Port Already in Use

Error: `Address already in use`

Solution:
- Change port in `Properties/launchSettings.json`
- Or kill process using the port

### Database Connection Failed

Error: `Cannot open database`

Solutions:
1. Verify SQL Server is running
2. Check connection string
3. Ensure database exists
4. Check firewall/network

### Migration Failed

Error: `migrations pending`

Solution:
```bash
dotnet ef database update --project src/Infrastructure --startup-project src/WebAPI
```

### Redis Connection Failed

Error: `Redis connection refused`

Solutions:
1. Start Redis server
2. Or remove Redis configuration (optional feature)

---

## Environment Variables

Override settings with environment variables:
```bash
set ConnectionStrings__DefaultConnection=your-connection-string
set Jwt__Secret=your-secret
dotnet run
```

---

## Quick Reference

| Task | Command |
|------|---------|
| Restore | `dotnet restore` |
| Build | `dotnet build` |
| Run | `dotnet run --project src/WebAPI` |
| Test | `dotnet test` |
| Add Migration | `dotnet ef migrations add <Name> --project src/Infrastructure --startup-project src/WebAPI` |
| Update Database | `dotnet ef database update --project src/Infrastructure --startup-project src/WebAPI` |
