#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PaySky.Api/PaySky.Api.csproj", "PaySky.Api/"]
COPY ["PaySky.Application/PaySky.Application.csproj", "PaySky.Application/"]
COPY ["PaySky.Domain/PaySky.Domain.csproj", "PaySky.Domain/"]
COPY ["PaySky.Shared/PaySky.Shared.csproj", "PaySky.Shared/"]
COPY ["PaySky.Infrastructure/PaySky.Infrastructure.csproj", "PaySky.Infrastructure/"]
RUN dotnet restore "./PaySky.Api/PaySky.Api.csproj"
COPY . .
WORKDIR "/src/PaySky.Api"
RUN dotnet build "./PaySky.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PaySky.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaySky.Api.dll"]