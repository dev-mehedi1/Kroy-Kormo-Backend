# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY kroy-kormo-backend.sln .
COPY kroy-kormo-backend/kroy-kormo-backend.csproj kroy-kormo-backend/

# Restore dependencies
RUN dotnet restore kroy-kormo-backend.sln

# Copy everything else
COPY . .

# Publish with explicit project file path
RUN dotnet publish "kroy-kormo-backend/kroy-kormo-backend.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "kroy-kormo-backend.dll"]