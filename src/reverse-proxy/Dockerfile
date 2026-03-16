FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY reverse-proxy/chronos.reverse-proxy/chronos.reverse-proxy.csproj ./reverse-proxy/chronos.reverse-proxy/

RUN dotnet restore ./reverse-proxy/chronos.reverse-proxy/chronos.reverse-proxy.csproj

COPY reverse-proxy/ ./reverse-proxy/
RUN dotnet publish ./reverse-proxy/chronos.reverse-proxy/chronos.reverse-proxy.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build-env /app/publish .

ENV TZ="Europe/Warsaw"

RUN adduser --disabled-password --gecos "" appuser && \
    chown -R appuser:appuser /app

USER appuser

EXPOSE 8080

ENTRYPOINT ["dotnet", "chronos.reverse-proxy.dll"]
