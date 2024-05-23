# Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY . ./
RUN dotnet restore src/Movies.Service.API/Movies.Service.API.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish src/Movies.Domain/Movies.Domain.csproj --no-restore -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Movies.Service.API.dll"]
