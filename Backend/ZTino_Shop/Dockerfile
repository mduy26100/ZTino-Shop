# ===== BUILD STAGE =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files first for layer caching
COPY src/Domain/Domain.csproj src/Domain/
COPY src/Application.Common/Application.Common.csproj src/Application.Common/
COPY src/Application/Application.csproj src/Application/
COPY src/Infrastructure/Infrastructure.csproj src/Infrastructure/
COPY src/WebAPI/WebAPI.csproj src/WebAPI/

# Restore dependencies
RUN dotnet restore src/WebAPI/WebAPI.csproj

# Copy all source code
COPY . .

# Publish the application
RUN dotnet publish src/WebAPI/WebAPI.csproj -c Release -o /app/publish --no-restore

# Install EF Core tools and create migration bundle
RUN dotnet tool install --global dotnet-ef --version 8.0.*
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet ef migrations bundle \
    --project src/Infrastructure/Infrastructure.csproj \
    --startup-project src/WebAPI/WebAPI.csproj \
    --output /app/publish/efbundle \
    --self-contained \
    --force

# ===== RUNTIME STAGE =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app

# Install ICU for globalization support
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Copy published app and migration bundle
COPY --from=build /app/publish .

# Copy entrypoint script
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh efbundle

# Configure ASP.NET Core
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

ENTRYPOINT ["./entrypoint.sh"]
