FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY time-reports/chronos.time-reports.core/chronos.time-reports.core.csproj ./time-reports/chronos.time-reports.core/
COPY time-reports/chronos.time-reports.api/chronos.time-reports.api.csproj ./time-reports/chronos.time-reports.api/
COPY shared/chronos.shared.configuration/chronos.shared.configuration.csproj ./shared/chronos.shared.configuration/
COPY shared/chronos.shared.exceptions/chronos.shared.exceptions.csproj ./shared/chronos.shared.exceptions/
COPY shared/chronos.shared.identity-context/chronos.shared.identity-context.csproj ./shared/chronos.shared.identity-context/
COPY shared/chronos.shared.messaging/chronos.shared.messaging.csproj ./shared/chronos.shared.messaging/
COPY shared/chronos.shared.messaging.rabbit-mq/chronos.shared.messaging.rabbit-mq.csproj ./shared/chronos.shared.messaging.rabbit-mq/

RUN dotnet restore ./time-reports/chronos.time-reports.api/chronos.time-reports.api.csproj

COPY time-reports/ ./time-reports/
COPY shared/ ./shared/
RUN dotnet publish ./time-reports/chronos.time-reports.api/chronos.time-reports.api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build-env /app/publish .

ENV TZ="Europe/Warsaw"

RUN adduser --disabled-password --gecos "" appuser && \
    chown -R appuser:appuser /app

USER appuser

EXPOSE 5003

ENTRYPOINT ["dotnet", "chronos.time-reports.api.dll"]
