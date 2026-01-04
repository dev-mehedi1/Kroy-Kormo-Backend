# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Copy the solution file from root
COPY kroy-kormo-backend.sln ./
# Copy the csproj file from subdirectory
COPY kroy-kormo-backend/kroy-kormo-backend.csproj ./kroy-kormo-backend/

# Restore dependencies
RUN dotnet restore

# Copy everything else
COPY . .

# Publish the project
WORKDIR /src/kroy-kormo-backend
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "kroy-kormo-backend.dll"]