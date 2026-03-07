FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY notifications/chronos.notifications.core/chronos.notifications.core.csproj ./notifications/chronos.notifications.core/
COPY notifications/chronos.notifications.api/chronos.notifications.api.csproj ./notifications/chronos.notifications.api/
COPY shared/chronos.shared.configuration/chronos.shared.configuration.csproj ./shared/chronos.shared.configuration/
COPY shared/chronos.shared.exceptions/chronos.shared.exceptions.csproj ./shared/chronos.shared.exceptions/
COPY shared/chronos.shared.messaging/chronos.shared.messaging.csproj ./shared/chronos.shared.messaging/
COPY shared/chronos.shared.messaging.rabbit-mq/chronos.shared.messaging.rabbit-mq.csproj ./shared/chronos.shared.messaging.rabbit-mq/

RUN dotnet restore ./notifications/chronos.notifications.api/chronos.notifications.api.csproj

COPY notifications/ ./notifications/
COPY shared/ ./shared/
RUN dotnet publish ./notifications/chronos.notifications.api/chronos.notifications.api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build-env /app/publish .

ENV TZ="Europe/Warsaw"

RUN adduser --disabled-password --gecos "" appuser && \
    chown -R appuser:appuser /app

USER appuser

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "chronos.notifications.api.dll"]
