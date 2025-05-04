# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/identity/Identity.API/Identity.API.csproj", "src/identity/Identity.API/"]
COPY ["libs/CORS/CORS.csproj", "libs/CORS/"]
COPY ["libs/Mapper/Mapper.csproj", "libs/Mapper/"]
COPY ["src/shared/Authorization/Authorization.csproj", "src/shared/Authorization/"]
COPY ["src/shared/Caching/Caching.csproj", "src/shared/Caching/"]
COPY ["src/shared/Logging/Logging.csproj", "src/shared/Logging/"]
COPY ["src/identity/Identity.Application/Identity.Application.csproj", "src/identity/Identity.Application/"]
COPY ["src/shared/Events/Events.csproj", "src/shared/Events/"]
COPY ["src/identity/Identity.Core/Identity.Core.csproj", "src/identity/Identity.Core/"]
COPY ["src/identity/Identity.Infrastructure/Identity.Infrastructure.csproj", "src/identity/Identity.Infrastructure/"]
RUN dotnet restore "./src/identity/Identity.API/Identity.API.csproj"
COPY . .
WORKDIR "/src/src/identity/Identity.API"
RUN dotnet build "./Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.API.dll"]