FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Slowback.SampleProject.Data.Migration/Slowback.SampleProject.Data.Migration.csproj", "Slowback.SampleProject.Data.Migration/"]
RUN dotnet restore "Slowback.SampleProject.Data.Migration/Slowback.SampleProject.Data.Migration.csproj"
COPY . .
WORKDIR "/src/Slowback.SampleProject.Data.Migration"
RUN dotnet build "Slowback.SampleProject.Data.Migration.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Slowback.SampleProject.Data.Migration.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Slowback.SampleProject.Data.Migration.dll"]
