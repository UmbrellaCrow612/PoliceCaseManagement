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
COPY ["src/evidence/Evidence.API/Evidence.API.csproj", "src/evidence/Evidence.API/"]
COPY ["libs/Cache.Abstractions/Cache.Abstractions.csproj", "libs/Cache.Abstractions/"]
COPY ["libs/Cache.Redis/Cache.Redis.csproj", "libs/Cache.Redis/"]
COPY ["libs/Grpc.JwtInterceptor/Grpc.JwtInterceptor.csproj", "libs/Grpc.JwtInterceptor/"]
COPY ["libs/Mapper/Mapper.csproj", "libs/Mapper/"]
COPY ["libs/Validator/Validator.csproj", "libs/Validator/"]
COPY ["src/sharedLogging/SharedLogging.csproj", "src/sharedLogging/"]
COPY ["src/evidence/Evidence.Application/Evidence.Application.csproj", "src/evidence/Evidence.Application/"]
COPY ["libs/Events.Core/Events.Core.csproj", "libs/Events.Core/"]
COPY ["libs/StorageProvider.Abstractions/StorageProvider.Abstractions.csproj", "libs/StorageProvider.Abstractions/"]
COPY ["libs/StorageProvider.AWS/StorageProvider.AWS.csproj", "libs/StorageProvider.AWS/"]
COPY ["src/evidence/Evidence.Core/Evidence.Core.csproj", "src/evidence/Evidence.Core/"]
COPY ["libs/Pagination.Abstractions/Pagination.Abstractions.csproj", "libs/Pagination.Abstractions/"]
COPY ["libs/Results.Abstractions/Results.Abstractions.csproj", "libs/Results.Abstractions/"]
COPY ["libs/SoftDelete.Abstractions/SoftDelete.Abstractions.csproj", "libs/SoftDelete.Abstractions/"]
COPY ["src/evidence/Evidence.Infrastructure/Evidence.Infrastructure.csproj", "src/evidence/Evidence.Infrastructure/"]
RUN dotnet restore "./src/evidence/Evidence.API/Evidence.API.csproj"
COPY . .
WORKDIR "/src/src/evidence/Evidence.API"
RUN dotnet build "./Evidence.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Evidence.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Evidence.API.dll"]