# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

WORKDIR /app

COPY ServerSightAPI.sln ./
COPY ServerSightAPI/*.csproj ./ServerSightAPI/
COPY ServerSightAPI.Tests.Integration/*.csproj ./ServerSightAPI.Tests.Integration/

COPY ./ ./app

# copy configuration of test to app (so we can do migrations for the database)
RUN cp ServerSightAPI.Tests.Integration/appsettings.json ServerSightAPI/appsettings.json
RUN dotnet tool install dotnet-ef --global
RUN dotnet restore
RUN dotnet build --no-restore

RUN dotnet ef database update

ENTRYPOINT ["dotnet", "test", "--no-build", "--verbosity normal"]