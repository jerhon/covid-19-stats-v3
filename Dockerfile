FROM mcr.microsoft.com/dotnet/sdk:5.0.102-ca-patch-buster-slim AS build-env
WORKDIR /app/

## Node.js dependencies / application
# Install Node.js as we need it to build our CRA with the .NET project
RUN apt-get update \
    &&  curl -sL https://deb.nodesource.com/setup_14.x | bash - \
    && apt-get install -y nodejs

# Copy the necessary npm package files
COPY src/Honlsoft.CovidApp/ClientApp/package.json src/Honlsoft.CovidApp/ClientApp/package.json
COPY src/Honlsoft.CovidApp/ClientApp/package-lock.json src/Honlsoft.CovidApp/ClientApp/package-lock.json
COPY restore-packages.ps1 .

# Restore the packages
RUN pwsh -f restore-packages.ps1

## Dotnet packages
# Copy csproj and restore nuget package as distinct layers
COPY Honlsoft.CovidApp.sln .
COPY src/*/*.csproj .
COPY test/*/*.csproj .
COPY move-subprojects.ps1 .

# Unfortunately, docker doesn't give a way to copy files and preserve subdirectories.
# Thus, this script will place the .csproj files based on the naming conventions for
# the projects.
RUN pwsh -f move-subprojects.ps1

# Restore our .NET packages
RUN dotnet restore

# Actual compilation
# Copy everything else and build
COPY . ./

# Just compile the entry point
WORKDIR /app/src/Honlsoft.CovidApp/
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Honlsoft.CovidApp.dll"]
