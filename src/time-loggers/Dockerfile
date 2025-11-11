FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY chronos.time-loggers.core/chronos.time-loggers.core.csproj ./chronos.time-loggers.core/
COPY chronos.time-loggers.api/chronos.time-loggers.api.csproj ./chronos.time-loggers.api/

RUN dotnet restore ./chronos.time-loggers.api/chronos.time-loggers.api.csproj

COPY . .
RUN dotnet publish ./chronos.time-loggers.api/chronos.time-loggers.api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "chronos.time-loggers.api.dll"]

ENV TZ="Europe/Warsaw"

EXPOSE 8081
EXPOSE 8080