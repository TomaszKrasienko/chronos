FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY time-loggers/chronos.time-loggers.core/chronos.time-loggers.core.csproj ./time-loggers/chronos.time-loggers.core/
COPY time-loggers/chronos.time-loggers.api/chronos.time-loggers.api.csproj ./time-loggers/chronos.time-loggers.api/
COPY shared/chronos.shared.configuration/chronos.shared.configuration.csproj ./shared/chronos.shared.configuration/
COPY shared/chronos.shared.exceptions/chronos.shared.exceptions.csproj ./shared/chronos.shared.exceptions/
COPY shared/chronos.shared.messaging/chronos.shared.messaging.csproj ./shared/chronos.shared.messaging/
COPY shared/chronos.shared.messaging.rabbit-mq/chronos.shared.messaging.rabbit-mq.csproj ./shared/chronos.shared.messaging.rabbit-mq/

RUN dotnet restore ./time-loggers/chronos.time-loggers.api/chronos.time-loggers.api.csproj

COPY time-loggers/ ./time-loggers/
COPY shared/ ./shared/
RUN dotnet publish ./time-loggers/chronos.time-loggers.api/chronos.time-loggers.api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build-env /app/publish .

ENV TZ="Europe/Warsaw"

RUN adduser --disabled-password --gecos "" appuser && \
    chown -R appuser:appuser /app

USER appuser

EXPOSE 5002

ENTRYPOINT ["dotnet", "chronos.time-loggers.api.dll"]