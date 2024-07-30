FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Slowback.Data.Core.Migrator/Slowback.Data.Core.Migrator.csproj", "Slowback.Data.Core.Migrator/"]
RUN dotnet restore "Slowback.Data.Core.Migrator/Slowback.Data.Core.Migrator.csproj"
COPY . .
WORKDIR "/src/Slowback.Data.Core.Migrator"
RUN dotnet build "Slowback.Data.Core.Migrator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Slowback.Data.Core.Migrator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Slowback.Data.Core.Migrator.dll"]
