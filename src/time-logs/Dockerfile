FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY time-logs/chronos.time-logs.core/chronos.time-logs.core.csproj ./time-logs/chronos.time-logs.core/
COPY time-logs/chronos.time-logs.api/chronos.time-logs.api.csproj ./time-logs/chronos.time-logs.api/
COPY shared/chronos.shared.configuration/chronos.shared.configuration.csproj ./shared/chronos.shared.configuration/
COPY shared/chronos.shared.exceptions/chronos.shared.exceptions.csproj ./shared/chronos.shared.exceptions/
COPY shared/chronos.shared.kernel/chronos.shared.kernel.csproj ./shared/chronos.shared.kernel/
COPY shared/chronos.shared.identity-context/chronos.shared.identity-context.csproj ./shared/chronos.shared.identity-context/
COPY shared/chronos.shared.messaging/chronos.shared.messaging.csproj ./shared/chronos.shared.messaging/
COPY shared/chronos.shared.messaging.rabbit-mq/chronos.shared.messaging.rabbit-mq.csproj ./shared/chronos.shared.messaging.rabbit-mq/

RUN dotnet restore ./time-logs/chronos.time-logs.api/chronos.time-logs.api.csproj

COPY time-logs/ ./time-logs/
COPY shared/ ./shared/
RUN dotnet publish ./time-logs/chronos.time-logs.api/chronos.time-logs.api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build-env /app/publish .

ENV TZ="Europe/Warsaw"

RUN adduser --disabled-password --gecos "" appuser && \
    chown -R appuser:appuser /app

USER appuser

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "chronos.time-logs.api.dll"]
