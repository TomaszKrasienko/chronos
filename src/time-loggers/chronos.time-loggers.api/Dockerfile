FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/time-loggers/chronos.time-loggers.api/chronos.time-loggers.api.csproj", "src/time-loggers/chronos.time-loggers.api/"]
RUN dotnet restore "src/time-loggers/chronos.time-loggers.api/chronos.time-loggers.api.csproj"
COPY . .
WORKDIR "/src/src/time-loggers/chronos.time-loggers.api"
RUN dotnet build "./chronos.time-loggers.api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./chronos.time-loggers.api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "chronos.time-loggers.api.dll"]
