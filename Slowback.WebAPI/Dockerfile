FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Slowback.WebAPI/Slowback.WebAPI.csproj", "Slowback.WebAPI/"]
RUN dotnet restore "Slowback.WebAPI/Slowback.WebAPI.csproj"
COPY . .
WORKDIR "/src/Slowback.WebAPI"
RUN dotnet build "Slowback.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Slowback.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Slowback.WebAPI.dll"]
