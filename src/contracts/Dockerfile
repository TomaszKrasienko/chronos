FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY contracts/chronos.contracts.core/chronos.contracts.core.csproj ./contracts/chronos.contracts.core/
COPY contracts/chronos.contracts.api/chronos.contracts.api.csproj ./contracts/chronos.contracts.api/
COPY shared/chronos.shared.configuration/chronos.shared.configuration.csproj ./shared/chronos.shared.configuration/
COPY shared/chronos.shared.exceptions/chronos.shared.exceptions.csproj ./shared/chronos.shared.exceptions/
COPY shared/chronos.shared.kernel/chronos.shared.kernel.csproj ./shared/chronos.shared.kernel/

RUN dotnet restore ./contracts/chronos.contracts.api/chronos.contracts.api.csproj

COPY contracts/ ./contracts/
COPY shared/ ./shared/
RUN dotnet publish ./contracts/chronos.contracts.api/chronos.contracts.api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build-env /app/publish .

ENV TZ="Europe/Warsaw"

RUN adduser --disabled-password --gecos "" appuser && \
    chown -R appuser:appuser /app

USER appuser

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "chronos.contracts.api.dll"]
